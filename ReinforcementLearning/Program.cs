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
        private static string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\31_12.bin";
        private static string fileNameReward = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Rewards.txt";

        private static int batchSize = 512;
        private static double learnRate = 0.0000001f; 
        private static double maxMinutes = 360f;
        private static int timeStepLimit = 100;

        static void Main(string[] args)
        {
            //RunQLearning();
            //RunNfq();
            //ExportRewardData(fileNameReward);
            //TestModel(fileName);
            TestModelFull(fileName);
        }


        private static void RunQLearning()
        {
            QLearningResult qLearningResult = QLearning.Learn(new QLearningArgs(new EnvironemntFrozenLake()));

            Console.WriteLine("\n" + qLearningResult.ToString());

            Console.ReadLine();
        }

        private static void RunNfq()
        {

            InputManager.ListenInputs();
            
            var initialStates = new List<Dictionary<EEnemyInput, double>>() 
            { 
                StartStates.initialStateStandard,
                //StartStates.initialStateLateGame,
                //StartStates.initialStateLateGameDefending,
                //StartStates.initialStateLateGameAttacking,
                //StartStates.initialStateMidGame,
                //StartStates.shouldTryDefend,
                //StartStates.shouldAttack,
                //StartStates.shouldEat,
                //StartStates.shouldTryBalanceTribes,
            };

            Random prng = new Random();
            var environment = new EnvironmentRts(initialStates);

            EGreedyStrategy trainingStrategy = new EGreedyStrategy(0.5f);
            GreedyStrategy evaluationStrategy = new GreedyStrategy();

            NfqArgs nfqArgs = new NfqArgs(environment,
                evaluationStrategy,
                trainingStrategy,
                _learnRate: learnRate,
                _batchSize: batchSize,
                _maxMinutes: maxMinutes,
                _timeStepLimit: timeStepLimit);

            Nfq nfq = new Nfq(nfqArgs);
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
            Console.WriteLine("Standard");
            TestModelState(_filename, StartStates.initialStateStandard);
            Console.WriteLine("Standard");
            TestModelState(_filename, StartStates.initialStateStandard);
            Console.WriteLine("Standard");
            TestModelState(_filename, StartStates.initialStateStandard);
            Console.WriteLine("Standard");
            TestModelState(_filename, StartStates.initialStateStandard);
            Console.WriteLine("Standard");
            TestModelState(_filename, StartStates.initialStateStandard);
            Console.WriteLine("Standard");
            TestModelState(_filename, StartStates.initialStateStandard);

            //Console.WriteLine("Late Game");
            //TestModelState(_filename, StartStates.initialStateLateGame);
            //Console.WriteLine("Late Game Defending");
            //TestModelState(_filename, StartStates.initialStateLateGameDefending);
            //Console.WriteLine("Late Game Attacking");
            //TestModelState(_filename, StartStates.initialStateLateGameAttacking);
            //Console.WriteLine("Mid Game");
            //TestModelState(_filename, StartStates.initialStateMidGame);
            //Console.WriteLine("Should Defend");
            //TestModelState(_filename, StartStates.shouldTryDefend);
            //Console.WriteLine("Should Attack");
            //TestModelState(_filename, StartStates.shouldAttack);
            //Console.WriteLine("Should Eat");
            //TestModelState(_filename, StartStates.shouldEat);
            //Console.WriteLine("Should balance tribes");
            //TestModelState(_filename, StartStates.shouldTryBalanceTribes);
            Console.ReadKey();
        }


        private static void TestModelState(string _filename, Dictionary<EEnemyInput, double> _state)
        {
            if (!File.Exists(_filename))
                throw new ArgumentException("File name not existant");

            TrainingResult trainingResult = Serializer.DeserializeObject(_filename);

            NeuralNetwork nn = new NeuralNetwork(trainingResult);

            Random prngTest = new Random();
            Random prngDummy = new Random();
            GreedyStrategy greedy = new GreedyStrategy();

            EnvironmentRts rtsTest = new EnvironmentRts(new List<Dictionary<EEnemyInput, double>>() { _state });
            EnvironmentRts rtsDummy = new EnvironmentRts(new List<Dictionary<EEnemyInput, double>>() { _state });

            rtsTest.Reset(true, prngTest, true, timeStepLimit);
            rtsDummy.Reset(true, prngDummy, true, timeStepLimit);

            double cumulativeRewardTest = 0f;
            double cumulativeRewardDummy = 0f;
            bool done = false;

            StepResult<double[]> testResult = default;
            StepResult<double[]> dummyResult = default;

            while (!done)
            {
                var action = greedy.SelectAction(rtsTest.State, nn, prngTest);
                testResult = rtsTest.Step(action, prngTest);
                dummyResult = rtsTest.Step((int)EEnemyOperation.None, prngDummy);

                Console.WriteLine((EEnemyOperation)action);

                cumulativeRewardTest += testResult.Reward;
                cumulativeRewardDummy += dummyResult.Reward;

                done = testResult.IsTruncated || testResult.Done;
                done |= dummyResult.IsTruncated || dummyResult.Done;
            }

            Console.WriteLine("[DUMMY] Cumulative Reward: " + cumulativeRewardDummy + "\t Final Reward: " + dummyResult.Reward.ToString());
            Console.WriteLine("[TEST]  Cumulative Reward: " + cumulativeRewardTest + "\t Final Reward: " + testResult.Reward.ToString());

        }



    }
}
