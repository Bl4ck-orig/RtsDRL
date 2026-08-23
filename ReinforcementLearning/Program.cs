using ReinforcementLearning.Training;
using ReinforcementLearning.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Utilities;

namespace ReinforcementLearning
{
    internal class Program
    {
        private static string fileNameNoExt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Model";
        private static string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\11_Model_0_2024_01_19-17_24.bin";
        private static string fileNameReward = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Rewards.txt";

        private static int batchSize = 1024;
        private static double learnRate = 0.0001f;
        private static double maxMinutes = 120f;
        private static int timeStepLimit = 100;
        private static double exploration = 0.21f;
        private static int epochs = 60;
        private static double gradientClippingThreshold = 600f;
        private static double minZeroConvergeThreshold = 0.00001f;
        private static bool fixNan = false;
        // Undiscounted returns make the Bellman operator a non-contraction, which is unstable
        // in combination with the 100 step truncation limit.
        private static double gamma = 0.99;
        private static bool clipValuesFirst = false;
        private static int hiddenLayerNodesAmount = 50;
        private static int hiddenLayersAmount = 3;

        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                RunHeadless(args);
                return;
            }

            //RunQLearning();
            ContinueNfq(fileName);
            //RunNfq();
            //ExportRewardData(fileNameReward);
            //TestModel(fileName);
            //TestModelFull(fileName);
        }

        #region Headless -----------------------------------------------------------------
        private static List<Dictionary<EEnemyInput, double>> TrainingInitialStates()
        {
            return new List<Dictionary<EEnemyInput, double>>()
            {
                StartStates.initialStateStandard,
                StartStates.initialStateLateGame,
                StartStates.initialStateLateGameDefending,
                StartStates.initialStateLateGameAttacking,
                StartStates.initialStateMidGame,
                StartStates.shouldTryDefend,
                StartStates.shouldAttack,
                StartStates.shouldEat,
                StartStates.shouldTryBalanceTribes,
            };
        }

        private static void RunHeadless(string[] _args)
        {
            switch (_args[0].ToLowerInvariant())
            {
                case "train":
                    TrainHeadless(_args);
                    break;
                case "eval":
                    EvaluateHeadless(_args);
                    break;
                case "baselines":
                    BaselinesHeadless(_args);
                    break;
                case "verify":
                    SelfCheck.Run();
                    break;
                default:
                    Console.WriteLine("usage: train <fixed|legacy> <minutes> <outputFile> [seed=N] [gamma=G] [penalty=P] [eps=E] [epsmin=E] [epsdecay=N] [continue=file]");
                    Console.WriteLine("       eval <modelFile> [repeats] [seed] [epsilon]");
                    Console.WriteLine("       baselines [repeats] [seed]");
                    Console.WriteLine("       verify");
                    break;
            }
        }

        private static Dictionary<string, string> ParseOptions(string[] _args, int _from)
        {
            var options = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (int i = _from; i < _args.Length; i++)
            {
                int split = _args[i].IndexOf('=');

                if (split <= 0)
                    continue;

                options[_args[i].Substring(0, split)] = _args[i].Substring(split + 1);
            }

            return options;
        }

        private static double Option(Dictionary<string, string> _options, string _key, double _fallback)
        {
            return _options.ContainsKey(_key)
                ? double.Parse(_options[_key], CultureInfo.InvariantCulture)
                : _fallback;
        }

        private static void TrainHeadless(string[] _args)
        {
            bool legacy = _args[1].Equals("legacy", StringComparison.OrdinalIgnoreCase);
            double minutes = double.Parse(_args[2], CultureInfo.InvariantCulture);
            string outputFile = _args[3];

            var options = ParseOptions(_args, 4);

            int seed = (int)Option(options, "seed", 1);
            double usedGamma = Option(options, "gamma", gamma);
            double stepPenalty = Option(options, "penalty", 0.0);
            double epsilonStart = Option(options, "eps", exploration);
            double epsilonMin = Option(options, "epsmin", epsilonStart);
            long epsilonDecaySteps = (long)Option(options, "epsdecay", 0);
            string continueFrom = options.ContainsKey("continue") ? options["continue"] : null;

            EnvironmentRts.StepPenalty = stepPenalty;

            NfqArgs nfqArgs = new NfqArgs(new EnvironmentRts(TrainingInitialStates()),
                new GreedyStrategy(),
                new EGreedyStrategy(epsilonStart, epsilonMin, epsilonDecaySteps),
                _learnRate: learnRate,
                _batchSize: batchSize,
                _maxMinutes: minutes,
                _timeStepLimit: timeStepLimit,
                _gradientClippingThreshold: gradientClippingThreshold,
                _fixNan: fixNan,
                _clipValuesFirst: clipValuesFirst,
                _minZeroConvergeThreshold: minZeroConvergeThreshold,
                _epochs: epochs,
                _hiddenLayerNodesAmount: hiddenLayerNodesAmount,
                _hiddenLayersAmount: hiddenLayersAmount,
                _gamma: usedGamma,
                _seed: seed,
                _legacyTarget: legacy);

            Console.WriteLine("[TRAIN] variant=" + (legacy ? "legacy" : "fixed")
                + " minutes=" + minutes.ToString(CultureInfo.InvariantCulture)
                + " seed=" + seed
                + " gamma=" + usedGamma.ToString(CultureInfo.InvariantCulture)
                + " penalty=" + stepPenalty.ToString(CultureInfo.InvariantCulture)
                + " eps=" + epsilonStart.ToString(CultureInfo.InvariantCulture)
                + "->" + epsilonMin.ToString(CultureInfo.InvariantCulture)
                + " over " + epsilonDecaySteps + " steps"
                + " lr=" + learnRate.ToString(CultureInfo.InvariantCulture)
                + " batch=" + batchSize
                + " epochs=" + epochs
                + " net=" + hiddenLayersAmount + "x" + hiddenLayerNodesAmount);

            if (!string.IsNullOrEmpty(continueFrom))
                Console.WriteLine("[TRAIN] continuing from: " + continueFrom);

            Nfq nfq = string.IsNullOrEmpty(continueFrom)
                ? new Nfq(nfqArgs)
                : new Nfq(nfqArgs, new NeuralNetwork(Serializer.DeserializeObject(continueFrom)));

            NfqResult result = nfq.Train();

            Serializer.SerializeObject(outputFile, result.ToNeuralNetworkResults());

            Console.WriteLine("[TRAIN] reason: " + result.EndReason);
            Console.WriteLine("[TRAIN] episodes: " + result.EpisodeRewards.Count
                + "  optimisation steps: " + result.GradientMagnitudes.Count);

            PrintGradientSummary(result.GradientMagnitudes);
            PrintRewardSummary(result.EpisodeOutcomes);

            Console.WriteLine("[TRAIN] saved: " + outputFile);
        }

        private static void PrintGradientSummary(List<double> _magnitudes)
        {
            if (_magnitudes.Count == 0)
                return;

            int tailSize = Math.Min(500, _magnitudes.Count);
            var tail = _magnitudes.Skip(_magnitudes.Count - tailSize).ToList();

            Console.WriteLine("[TRAIN] gradient magnitude  first=" + Format(_magnitudes[0])
                + "  max=" + Format(_magnitudes.Max())
                + "  mean(last " + tailSize + ")=" + Format(tail.Average()));
        }

        private static void PrintRewardSummary(List<double> _episodeOutcomes)
        {
            if (_episodeOutcomes.Count == 0)
                return;

            int tailSize = Math.Min(300, _episodeOutcomes.Count);
            var tail = _episodeOutcomes.Skip(_episodeOutcomes.Count - tailSize).ToList();

            Console.WriteLine("[TRAIN] outcome (+1 won, -1 lost, 0 step limit)  mean(all)=" + Format(_episodeOutcomes.Average())
                + "  mean(last " + tailSize + ")=" + Format(tail.Average())
                + "  wins=" + tail.Count(x => x > 0.5)
                + "  losses=" + tail.Count(x => x < -0.5)
                + "  of " + tailSize);
        }

        private static string Format(double _value) => _value.ToString("G6", CultureInfo.InvariantCulture);

        private static void EvaluateHeadless(string[] _args)
        {
            string modelFile = _args[1];
            int repeats = _args.Length > 2 ? int.Parse(_args[2], CultureInfo.InvariantCulture) : 30;
            int seed = _args.Length > 3 ? int.Parse(_args[3], CultureInfo.InvariantCulture) : 1000;
            double epsilon = _args.Length > 4 ? double.Parse(_args[4], CultureInfo.InvariantCulture) : 0.0;

            NeuralNetwork nn = new NeuralNetwork(Serializer.DeserializeObject(modelFile));
            IStrategy policy = epsilon > 0 ? (IStrategy)new EGreedyStrategy(epsilon) : new GreedyStrategy();
            Random prng = new Random(seed);

            int actionCount = Enum.GetValues(typeof(EEnemyOperation)).Length;
            long[] actionHistogram = new long[actionCount];

            Console.WriteLine("[EVAL] model=" + modelFile + " repeats=" + repeats + " seed=" + seed + " epsilon=" + epsilon.ToString(CultureInfo.InvariantCulture));
            Console.WriteLine("[EVAL] " + "state".PadRight(32) + "model     noop      random");

            double modelTotal = 0.0, noOpTotal = 0.0, randomTotal = 0.0;
            var states = StartStates.StartStatesByLabel;

            foreach (var state in states)
            {
                double modelSum = 0.0, noOpSum = 0.0, randomSum = 0.0;

                for (int r = 0; r < repeats; r++)
                {
                    int episodeSeed = seed + r;

                    modelSum += RunEpisode(s =>
                    {
                        int action = policy.SelectAction(s, nn, prng);
                        actionHistogram[action]++;
                        return action;
                    }, state.Value, episodeSeed);

                    noOpSum += RunEpisode(s => (int)EEnemyOperation.None, state.Value, episodeSeed);
                    randomSum += RunEpisode(s => prng.Next(actionCount), state.Value, episodeSeed);
                }

                modelTotal += modelSum / repeats;
                noOpTotal += noOpSum / repeats;
                randomTotal += randomSum / repeats;

                Console.WriteLine("[EVAL] " + state.Key.PadRight(32)
                    + Format(modelSum / repeats).PadRight(10)
                    + Format(noOpSum / repeats).PadRight(10)
                    + Format(randomSum / repeats));
            }

            Console.WriteLine("[EVAL] " + "TOTAL".PadRight(32)
                + Format(modelTotal / states.Count).PadRight(10)
                + Format(noOpTotal / states.Count).PadRight(10)
                + Format(randomTotal / states.Count));

            long totalActions = actionHistogram.Sum();
            Console.WriteLine("[EVAL] action distribution over " + totalActions + " decisions:");

            for (int i = 0; i < actionCount; i++)
            {
                if (actionHistogram[i] == 0)
                    continue;

                Console.WriteLine("[EVAL]   " + ((EEnemyOperation)i).ToString().PadRight(28)
                    + Format(100.0 * actionHistogram[i] / totalActions) + " %");
            }
        }

        /// <summary>
        /// Measures what the environment rewards at all: every constant action policy plus a
        /// uniformly random one, so a learned policy can be put in relation to them.
        /// </summary>
        private static void BaselinesHeadless(string[] _args)
        {
            int repeats = _args.Length > 1 ? int.Parse(_args[1], CultureInfo.InvariantCulture) : 30;
            int seed = _args.Length > 2 ? int.Parse(_args[2], CultureInfo.InvariantCulture) : 1000;

            int actionCount = Enum.GetValues(typeof(EEnemyOperation)).Length;
            var states = StartStates.StartStatesByLabel;

            Console.WriteLine("[BASE] mean final reward over " + states.Count + " start states x "
                + repeats + " episodes");

            for (int a = 0; a < actionCount; a++)
            {
                int action = a;
                double total = 0.0;

                foreach (var state in states)
                {
                    double sum = 0.0;

                    for (int r = 0; r < repeats; r++)
                        sum += RunEpisode(s => action, state.Value, seed + r);

                    total += sum / repeats;
                }

                Console.WriteLine("[BASE] always " + ((EEnemyOperation)a).ToString().PadRight(28)
                    + Format(total / states.Count));
            }

            Random prng = new Random(seed);
            double randomTotal = 0.0;

            foreach (var state in states)
            {
                double sum = 0.0;

                for (int r = 0; r < repeats; r++)
                    sum += RunEpisode(s => prng.Next(actionCount), state.Value, seed + r);

                randomTotal += sum / repeats;
            }

            Console.WriteLine("[BASE] " + "uniform random".PadRight(35) + Format(randomTotal / states.Count));
        }

        /// <summary>
        /// Runs one episode from the given start state and returns the reward of the final
        /// step, which is +1 for a win, -1 for a defeat and 0 when the step limit is hit.
        /// </summary>
        private static double RunEpisode(Func<double[], int> _selectAction,
            Dictionary<EEnemyInput, double> _state,
            int _seed)
        {
            EnvironmentRts env = new EnvironmentRts(new List<Dictionary<EEnemyInput, double>>() { _state });
            env.Reset(true, true, _seed, timeStepLimit);

            bool done = false;
            StepResult<double[]> result = default;

            while (!done)
            {
                result = env.Step(_selectAction(env.State));
                done = result.IsTruncated || result.Done;
            }

            // The outcome rather than the reward, so the numbers stay comparable no matter how
            // the reward is shaped: +1 won, -1 lost, 0 step limit reached.
            if (!result.Done)
                return 0.0;

            return result.Reward > 0.0 ? 1.0 : -1.0;
        }
        #endregion -----------------------------------------------------------------

        private static void RunQLearning()
        {
            QLearningResult qLearningResult = QLearning.Learn(new QLearningArgs(new EnvironemntFrozenLake()));

            Console.WriteLine("\n" + qLearningResult.ToString());

            Console.ReadLine();
        }

        private static void ContinueNfq(string _filename)
        {
            TrainingResult trainingResult = Serializer.DeserializeObject(_filename);

            NeuralNetwork nn = new NeuralNetwork(trainingResult);

            RunNfq(nn);
        }

        private static void RunNfq(NeuralNetwork _nn = null)
        {
            InputManager.ListenInputs();
            
            var initialStates = new List<Dictionary<EEnemyInput, double>>() 
            { 
                StartStates.initialStateStandard,
                StartStates.initialStateLateGame,
                StartStates.initialStateLateGameDefending,
                StartStates.initialStateLateGameAttacking,
                StartStates.initialStateMidGame,
                StartStates.shouldTryDefend,
                StartStates.shouldAttack,
                StartStates.shouldEat,
                StartStates.shouldTryBalanceTribes,
            };

            Random prng = new Random();
            var environment = new EnvironmentRts(initialStates);

            EGreedyStrategy trainingStrategy = new EGreedyStrategy(exploration);
            GreedyStrategy evaluationStrategy = new GreedyStrategy();

            NfqArgs nfqArgs = new NfqArgs(environment,
                evaluationStrategy,
                trainingStrategy,
                _learnRate: learnRate,
                _batchSize: batchSize,
                _maxMinutes: maxMinutes,
                _timeStepLimit: timeStepLimit,
                _gradientClippingThreshold: gradientClippingThreshold,
                _fixNan: fixNan,
                _clipValuesFirst: clipValuesFirst,
                _minZeroConvergeThreshold: minZeroConvergeThreshold,
                _epochs: epochs,
                _hiddenLayerNodesAmount: hiddenLayerNodesAmount,
                _hiddenLayersAmount: hiddenLayersAmount,
                _gamma: gamma);
            
            Nfq nfq = _nn == null ? new Nfq(nfqArgs) : new Nfq(nfqArgs, _nn);

            NfqResult result = nfq.Train();

            string file = fileNameNoExt + "_0_" + DateTime.Now.ToString("yyyy_MM_dd-HH_mm") + ".bin";
            for (int i = 0; File.Exists(file); i++)
                file = fileNameNoExt + "_" + i + "_" + DateTime.Now.ToString("yyyy_MM_dd-HH_mm") + ".bin";

            Serializer.SerializeObject(file, result.ToNeuralNetworkResults());

            Console.WriteLine("\n" + result.EndReason + " Model saved as: " + file);
            
            Console.ReadKey();
        }

        private static void ExportRewardData(string _filename)
        {
            if (!File.Exists(_filename))
                throw new ArgumentException("File name not existant");

            TrainingResult _trainingResult = Serializer.DeserializeObject(_filename);

            DataExporter.ExportData(_trainingResult.episodeRewards, _filename);
        }

        private static void TestModel(string _filename)
        {
            if (!File.Exists(_filename))
                throw new ArgumentException("File name not existant");

            TrainingResult trainingResult = Serializer.DeserializeObject(_filename);

            NeuralNetwork nn = new NeuralNetwork(trainingResult);

            Random prng = new Random();
            GreedyStrategy greedy = new GreedyStrategy();

            Console.WriteLine("Should defend: " + (EEnemyOperation)greedy.SelectAction(StartStates.shouldTryDefend.Values.ToArray(), nn, prng));
            Console.WriteLine("Should attack: " + (EEnemyOperation)greedy.SelectAction(StartStates.shouldAttack.Values.ToArray(), nn, prng));
            Console.WriteLine("Should eat: " + (EEnemyOperation)greedy.SelectAction(StartStates.shouldEat.Values.ToArray(), nn, prng));
            Console.WriteLine("Should balance tribes: " + (EEnemyOperation)greedy.SelectAction(StartStates.shouldTryBalanceTribes.Values.ToArray(), nn, prng));

            Console.ReadKey();
        }


        private static void TestModelFull(string _filename)
        {
            int i = 1;

            var states = StartStates.StartStatesByLabel;

            Console.WriteLine(_filename + "\n\n");

            double modelTotalReward = 0;
            double noOpTotalReward = 0;
            double randomTotalReward = 0;

            foreach (var state in states)
            {
                Console.WriteLine(state.Key);
                var nextResult = TestState(_filename, i++, state.Value);

                modelTotalReward += nextResult.ModelLastReward;
                noOpTotalReward += nextResult.NoOpLastReward;
                randomTotalReward += nextResult.RandomLastReward;

                Console.WriteLine("\t[RESULT]  Final Reward: " + nextResult.ModelLastReward);
                Console.WriteLine("\t[NO OP] Final Reward: " + nextResult.NoOpLastReward);
                Console.WriteLine("\t[RANDOM]  Final Reward: " + nextResult.RandomLastReward + "\n");
            }

            Console.WriteLine("\n\nTotal:");
            Console.WriteLine("\t[RESULT]  Final Reward: " + (modelTotalReward / states.Count));
            Console.WriteLine("\t[NO OP] Final Reward: " + (noOpTotalReward / states.Count));
            Console.WriteLine("\t[RANDOM]  Final Reward: " + (randomTotalReward / states.Count));

            Console.ReadKey();
        }

        private static TestStateResult TestState(string _filename, int _seed, Dictionary<EEnemyInput, double> _state)
        {
            var modelResult = TestModelState(_filename, _seed, _state);
            var dummyResult = TestModelStateDummy(_seed, _state);
            var randomResult = TestModelStateRandom(_seed, _state);

            return new TestStateResult(modelResult.CumulativeReward, 
                modelResult.LastReward,
                dummyResult.CumulativeReward,
                dummyResult.LastReward,
                randomResult.CumulativeReward,
                randomResult.LastReward);
        }

        private static (double CumulativeReward, double LastReward) TestModelState(string _filename, int _seed, Dictionary<EEnemyInput, double> _state)
        {
            if (!File.Exists(_filename))
                throw new ArgumentException("File name not existant");

            TrainingResult trainingResult = Serializer.DeserializeObject(_filename);

            if(trainingResult.gradientMagnitudes != null && trainingResult.gradientMagnitudes.Length > 0)
            {
                var reversedMagnitudes = trainingResult.gradientMagnitudes.Reverse().ToList();
            }

            NeuralNetwork nn = new NeuralNetwork(trainingResult);

            Random prngTest = new Random();
            GreedyStrategy greedy = new GreedyStrategy();

            EnvironmentRts env = new EnvironmentRts(new List<Dictionary<EEnemyInput, double>>() { _state });

            env.Reset(true, true, _seed, timeStepLimit);

            double cumulativeReward = 0f;
            bool done = false;

            StepResult<double[]> result = default;

            while (!done)
            {
                var action = greedy.SelectAction(env.State, nn, prngTest);

                result = env.Step(action);

                cumulativeReward += result.Reward;

                done = result.IsTruncated || result.Done;
            }

            return (cumulativeReward, result.Reward);
        }

        private static (double CumulativeReward, double LastReward) TestModelStateDummy(int _seed, Dictionary<EEnemyInput, double> _state)
        {
            EnvironmentRts env = new EnvironmentRts(new List<Dictionary<EEnemyInput, double>>() { _state });

            env.Reset(true, true, _seed, timeStepLimit);

            double cumulativeReward = 0f;
            bool done = false;

            StepResult<double[]> result = default;

            while (!done)
            {
                result = env.Step((int)EEnemyOperation.None);

                cumulativeReward += result.Reward;

                done = result.IsTruncated || result.Done;
            }

            return (cumulativeReward, result.Reward);
        }

        private static (double CumulativeReward, double LastReward) TestModelStateRandom(int _seed, Dictionary<EEnemyInput, double> _state)
        {
            Random prng = new Random();
            EnvironmentRts env = new EnvironmentRts(new List<Dictionary<EEnemyInput, double>>() { _state });

            env.Reset(true, true, _seed, timeStepLimit);

            double cumulativeReward = 0f;
            bool done = false;

            StepResult<double[]> result = default;
            int operationsAmount = Enum.GetValues(typeof(EEnemyOperation)).Cast<EEnemyOperation>().Count();

            while (!done)
            {
                result = env.Step(prng.Next(operationsAmount));

                cumulativeReward += result.Reward;

                done = result.IsTruncated || result.Done;
            }

            return (cumulativeReward, result.Reward);
        }
    }
}
