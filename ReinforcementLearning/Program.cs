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
        private static string fileNameNoExt = Directory.GetCurrentDirectory() + "\\Models\\Model";
        private static string fileName = Directory.GetCurrentDirectory() + "\\Models" +
            "\\1st_Model.bin";
        private static string fileName1st = Directory.GetCurrentDirectory() + "\\Models" +
            "\\1st_Model.bin";
        private static string fileName2nd = Directory.GetCurrentDirectory() + "\\Models" +
            "\\2nd_Model.bin";
        private static string fileName3rd = Directory.GetCurrentDirectory() + "\\Models" +
            "\\3rd_Model.bin";
        private static string fileName4th = Directory.GetCurrentDirectory() + "\\Models" +
            "\\4th_Model.bin";
        private static string fileNameAttack = Directory.GetCurrentDirectory() + "\\Models" +
            "\\AttackModel.bin";

        private static string fileNameReward = Directory.GetCurrentDirectory() + "\\Data" +
            "\\Model_Rewards.txt";
        private static string fileNameMagnitudes = Directory.GetCurrentDirectory() + "\\Data" +
            "\\Model_Magnitudes.txt";

        private static int batchSize = 1024;
        private static double learnRate = 0.0001f;
        private static double maxMinutes = 480f;
        private static int timeStepLimit = 100;
        private static double exploration = 0.15f;
        private static int epochs = 60;
        private static double gradientClippingThreshold = 600f;
        private static double minZeroConvergeThreshold = 0.00001f;
        private static bool fixNan = false;
        private static double gamma = 1f;
        private static bool clipValuesFirst = false;
        private static int hiddenLayerNodesAmount = 60;
        private static int hiddenLayersAmount = 3;

        static void Main(string[] args)
        {
            /*
             * Die folgenden beiden Befehle können auskommentiert werden, und den Lernprozess zu starten oder fortzuführen.
             */
            //ContinueNfq(fileName);
            //RunNfq();

            /*
             * Die folgenden beiden Befehle können auskommentiert werden, und die Reward oder Magnitude Daten zu exportieren.
             * Diese können in dem "PlotResult" Projekt geplottet werden. So wurden die Daten exportiert für die Plots aus dem E-Learning Dokument.
             */
            //ExportRewardData(fileName, fileNameReward);
            //ExportMagnitudeData(fileName, fileNameMagnitudes);

            /*
             * Die folgende Methode dient als Evaluation.
             */
            TestModelFull(fileName4th);
        }

        /// <summary>
        /// Setzt das Lernen des Models fort
        /// </summary>
        /// <param name="_filename">Dateipfad des Models</param>
        private static void ContinueNfq(string _filename)
        {
            TrainingResult trainingResult = Serializer.DeserializeObject(_filename);

            NeuralNetwork nn = new NeuralNetwork(trainingResult);

            RunNfq(nn);
        }

        /// <summary>
        /// Starte den Lernprozess
        /// </summary>
        /// <param name="_nn"></param>
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

        /// <summary>
        /// Gradient Längen des Lernprozesses exportieren.
        /// </summary>
        /// <param name="_filename">Dateipfad der Import Datei</param>
        /// <param name="_newFile">Dateipfad der Export Datei </param>
        /// <exception cref="ArgumentException"></exception>
        private static void ExportMagnitudeData(string _filename, string _newFile)
        {
            if (!File.Exists(_filename))
                throw new ArgumentException("File name not existant");

            TrainingResult _trainingResult = Serializer.DeserializeObject(_filename);

            DataExporter.ExportData(_trainingResult.gradientMagnitudes, _newFile);
        }

        /// <summary>
        /// Rewards des Lernprozesses exportieren.
        /// </summary>
        /// <param name="_filename">Dateipfad der Import Datei</param>
        /// <param name="_newFile">Dateipfad der Export Datei </param>
        /// <exception cref="ArgumentException"></exception>
        private static void ExportRewardData(string _filename, string _newFile)
        {
            if (!File.Exists(_filename))
                throw new ArgumentException("File name not existant");

            TrainingResult _trainingResult = Serializer.DeserializeObject(_filename);

            DataExporter.ExportData(_trainingResult.episodeRewards, _newFile);
        }

        /// <summary>
        /// Tested ein Model auf den erzielten Reward anhand einer durchlaufenen Episode. Zum Vergleich wird ein Model,
        /// welches nichts tut mit dem Namen "NO OP" und ein Model welches zufällige Entscheidungen trifft "RANDOM" benutzt.
        /// </summary>
        /// <param name="_filename"></param>
        private static void TestModelFull(string _filename)
        {
            int seed = 31853;

            var states = StartStates.StartStatesByLabel;

            Console.WriteLine(_filename + "\n\n");

            double modelTotalReward = 0;
            double noOpTotalReward = 0;
            double randomTotalReward = 0;

            foreach (var state in states)
            {
                Console.WriteLine(state.Key);
                var nextResult = TestState(_filename, seed++, StartStates.initialStateStandard);

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
                var reversedRewards = trainingResult.episodeRewards.Reverse().ToList();
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
                var action = prng.Next(operationsAmount);

                result = env.Step(action);

                cumulativeReward += result.Reward;

                done = result.IsTruncated || result.Done;
            }

            return (cumulativeReward, result.Reward);
        }
    }
}
