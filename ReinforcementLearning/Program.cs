using ReinforcementLearning.Training;
using ReinforcementLearning.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace ReinforcementLearning
{
    internal class Program
    {
        private static string fileNameNoExt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Model";
        private static string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Model_0_2024_01_16-16_36.bin";
        private static string fileNameReward = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Rewards.txt";

        private static int batchSize = 1024;
        private static double learnRate = 0.0001f;
        private static double maxMinutes = 480f;
        private static int timeStepLimit = 100;
        private static double exploration = 0.15f;
        private static int epochs = 80;
        private static double gradientClippingThreshold = 600f;
        private static double minZeroConvergeThreshold = 0.00001f;
        private static bool fixNan = false;
        private static bool clipValuesFirst = false;

        static void Main(string[] args)
        {
            //RunQLearning();
            //ContinueNfq(fileName);
            RunNfq();
            //ExportRewardData(fileNameReward);
            //TestModel(fileName);
            //TestModelFull(fileName);
        }

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
                _epochs: epochs);

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

            Console.WriteLine(_filename + "\n\n");
            Console.WriteLine("Standard");
            TestState(_filename, i++, StartStates.initialStateStandard);
            Console.WriteLine("Late Game");
            TestState(_filename, i++, StartStates.initialStateLateGame);
            Console.WriteLine("Late Game Defending");
            TestState(_filename, i++, StartStates.initialStateLateGameDefending);
            Console.WriteLine("Late Game Attacking");
            TestState(_filename, i++, StartStates.initialStateLateGameAttacking);
            Console.WriteLine("Mid Game");
            TestState(_filename, i++, StartStates.initialStateMidGame);
            Console.WriteLine("Should Defend");
            TestState(_filename, i++, StartStates.shouldTryDefend);
            Console.WriteLine("Should Attack");
            TestState(_filename, i++, StartStates.shouldAttack);
            Console.WriteLine("Should Eat");
            TestState(_filename, i++, StartStates.shouldEat);
            Console.WriteLine("Should balance tribes");
            TestState(_filename, i++, StartStates.shouldTryBalanceTribes);

            Console.ReadKey();
        }

        private static void TestState(string _filename, int _seed, Dictionary<EEnemyInput, double> _state)
        {
            var testModelResult = TestModelState(_filename, _seed, _state);
            var dummyResult = TestModelStateDummy(_seed, _state);
            var randomResult = TestModelStateRandom(_seed, _state);
            Console.WriteLine("[NO OP] Cumulative Reward: " + dummyResult.CumulativeReward + "\t Final Reward: " + dummyResult.LastReward);
            Console.WriteLine("[RANDOM]  Cumulative Reward: " + randomResult.CumulativeReward + "\t Final Reward: " + randomResult.LastReward);
            Console.WriteLine("[RESULT]  Cumulative Reward: " + testModelResult.CumulativeReward + "\t Final Reward: " + testModelResult.LastReward);
        }


        private static (double CumulativeReward, double LastReward) TestModelState(string _filename, int _seed, Dictionary<EEnemyInput, double> _state)
        {
            if (!File.Exists(_filename))
                throw new ArgumentException("File name not existant");

            TrainingResult trainingResult = Serializer.DeserializeObject(_filename);

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

                //Console.WriteLine((EEnemyOperation)action);

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
