using ReinforcementLearning.Training;
using ReinforcementLearning.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace ReinforcementLearning
{
    internal class Program
    {
        private static string fileNameNoExt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Model";



        static void Main(string[] args)
        {
            //RunQLearning();

            RunNfq();

            //TestModel(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\FILE);
        }

        private static void RunQLearning()
        {
            QLearningResult qLearningResult = QLearning.Learn(new QLearningArgs(new EnvironemntFrozenLake()));

            Console.WriteLine("\n" + qLearningResult.ToString());

            Console.ReadLine();
        }

        private static void RunNfq()
        {
            Dictionary<EEnemyInput, double> initialStateStandard = new Dictionary<EEnemyInput, double>()
            {
                { EEnemyInput.TotalGhouls, 10.0f },                                          
                { EEnemyInput.IdlingGhouls, 10.0f },
                { EEnemyInput.GhoulsInDanger, 0.0f },
                { EEnemyInput.HungryIdlingGhouls, 0.0f },
                { EEnemyInput.GhoulsWithWeapons, 0.0f },
                { EEnemyInput.AttackingGhouls, 0.0f },
                { EEnemyInput.AttackingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.DefendingGhouls, 0.0f },
                { EEnemyInput.GhoulsInWorkshops, 0.0f },
                { EEnemyInput.DefendingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.IdlingGhoulsWithWeapon, 0.0f },
                { EEnemyInput.IdlingGhoulsNotHungry, 10.0f  },
                { EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UsedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnassignedFoodsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnfinishedTribesInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.TribesDefensive, 1.0f },
                { EEnemyInput.TribesAggresive, 1.0f },
                { EEnemyInput.Churches, 0.0f },
                { EEnemyInput.UnbalancedTribes, 0.0f },
                { EEnemyInput.BuildingTribeGhouls, 0.0f },
                { EEnemyInput.BuildingWorkshopGhouls, 0.0f}
            };

            Dictionary<EEnemyInput, double> initialStateLateGame = new Dictionary<EEnemyInput, double>()
            {
                { EEnemyInput.TotalGhouls, 27.0f },
                { EEnemyInput.IdlingGhouls, 15.0f },
                { EEnemyInput.GhoulsInDanger, 5.0f },
                { EEnemyInput.HungryIdlingGhouls, 5.0f },
                { EEnemyInput.GhoulsWithWeapons, 8.0f },
                { EEnemyInput.AttackingGhouls, 0.0f },
                { EEnemyInput.AttackingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.DefendingGhouls, 0.0f },
                { EEnemyInput.GhoulsInWorkshops, 2.0f },
                { EEnemyInput.DefendingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.IdlingGhoulsWithWeapon, 8.0f },
                { EEnemyInput.IdlingGhoulsNotHungry, 10.0f  },
                { EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger, 5.0f },
                { EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger, 2.0f },
                { EEnemyInput.UsedWorkshopsInRangeAndNotInDanger, 2.0f },
                { EEnemyInput.UnassignedFoodsInRangeAndNotInDanger, 10.0f },
                { EEnemyInput.UnfinishedTribesInRangeAndNotInDanger, 1.0f },
                { EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, 1.0f },
                { EEnemyInput.TribesDefensive, 3.0f },
                { EEnemyInput.TribesAggresive, 3.0f },
                { EEnemyInput.Churches, 1.0f },
                { EEnemyInput.UnbalancedTribes, 0.0f },
                { EEnemyInput.BuildingTribeGhouls, 0.0f },
                { EEnemyInput.BuildingWorkshopGhouls, 0.0f}
            };
            
            Dictionary<EEnemyInput, double> initialStateLateGameDefending = new Dictionary<EEnemyInput, double>()
            {
                { EEnemyInput.TotalGhouls, 32.0f },
                { EEnemyInput.IdlingGhouls, 15.0f },
                { EEnemyInput.GhoulsInDanger, 5.0f },
                { EEnemyInput.HungryIdlingGhouls, 5.0f },
                { EEnemyInput.GhoulsWithWeapons, 11.0f },
                { EEnemyInput.AttackingGhouls, 0.0f },
                { EEnemyInput.AttackingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.DefendingGhouls, 2.0f },
                { EEnemyInput.GhoulsInWorkshops, 2.0f },
                { EEnemyInput.DefendingGhoulsWithWeapons, 3.0f },
                { EEnemyInput.IdlingGhoulsWithWeapon, 8.0f },
                { EEnemyInput.IdlingGhoulsNotHungry, 10.0f  },
                { EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger, 5.0f },
                { EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger, 2.0f },
                { EEnemyInput.UsedWorkshopsInRangeAndNotInDanger, 2.0f },
                { EEnemyInput.UnassignedFoodsInRangeAndNotInDanger, 10.0f },
                { EEnemyInput.UnfinishedTribesInRangeAndNotInDanger, 1.0f },
                { EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, 1.0f },
                { EEnemyInput.TribesDefensive, 3.0f },
                { EEnemyInput.TribesAggresive, 3.0f },
                { EEnemyInput.Churches, 1.0f },
                { EEnemyInput.UnbalancedTribes, 0.0f },
                { EEnemyInput.BuildingTribeGhouls, 0.0f },
                { EEnemyInput.BuildingWorkshopGhouls, 0.0f}
            };
            
            Dictionary<EEnemyInput, double> initialStateLateGameAttacking = new Dictionary<EEnemyInput, double>()
            {
                { EEnemyInput.TotalGhouls, 32.0f },
                { EEnemyInput.IdlingGhouls, 15.0f },
                { EEnemyInput.GhoulsInDanger, 5.0f },
                { EEnemyInput.HungryIdlingGhouls, 5.0f },
                { EEnemyInput.GhoulsWithWeapons, 11.0f },
                { EEnemyInput.AttackingGhouls, 2.0f },
                { EEnemyInput.AttackingGhoulsWithWeapons, 3.0f },
                { EEnemyInput.DefendingGhouls, 0.0f },
                { EEnemyInput.GhoulsInWorkshops, 2.0f },
                { EEnemyInput.DefendingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.IdlingGhoulsWithWeapon, 8.0f },
                { EEnemyInput.IdlingGhoulsNotHungry, 10.0f  },
                { EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger, 5.0f },
                { EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger, 2.0f },
                { EEnemyInput.UsedWorkshopsInRangeAndNotInDanger, 2.0f },
                { EEnemyInput.UnassignedFoodsInRangeAndNotInDanger, 10.0f },
                { EEnemyInput.UnfinishedTribesInRangeAndNotInDanger, 1.0f },
                { EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, 1.0f },
                { EEnemyInput.TribesDefensive, 3.0f },
                { EEnemyInput.TribesAggresive, 3.0f },
                { EEnemyInput.Churches, 1.0f },
                { EEnemyInput.UnbalancedTribes, 0.0f },
                { EEnemyInput.BuildingTribeGhouls, 0.0f },
                { EEnemyInput.BuildingWorkshopGhouls, 0.0f }
            };
            
            Dictionary<EEnemyInput, double> initialStateMidGame = new Dictionary<EEnemyInput, double>()
            {
                { EEnemyInput.TotalGhouls, 20.0f },
                { EEnemyInput.IdlingGhouls, 10.0f },
                { EEnemyInput.GhoulsInDanger, 1.0f },
                { EEnemyInput.HungryIdlingGhouls, 5.0f },
                { EEnemyInput.GhoulsWithWeapons, 1.0f },
                { EEnemyInput.AttackingGhouls, 0.0f },
                { EEnemyInput.AttackingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.DefendingGhouls, 0.0f },
                { EEnemyInput.GhoulsInWorkshops, 2.0f },
                { EEnemyInput.DefendingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.IdlingGhoulsWithWeapon, 2.0f },
                { EEnemyInput.IdlingGhoulsNotHungry, 10.0f },
                { EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger, 2.0f },
                { EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UsedWorkshopsInRangeAndNotInDanger, 1.0f },
                { EEnemyInput.UnassignedFoodsInRangeAndNotInDanger, 7.0f },
                { EEnemyInput.UnfinishedTribesInRangeAndNotInDanger, 1.0f },
                { EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, 1.0f },
                { EEnemyInput.TribesDefensive, 1.0f },
                { EEnemyInput.TribesAggresive, 1.0f },
                { EEnemyInput.Churches, 1.0f },
                { EEnemyInput.UnbalancedTribes, 0.0f },
                { EEnemyInput.BuildingTribeGhouls, 2.0f },
                { EEnemyInput.BuildingWorkshopGhouls, 2.0f }
            };

            var initialStates = new List<Dictionary<EEnemyInput, double>>() { initialStateStandard,
                    initialStateLateGame,
                    initialStateLateGameDefending,
                    initialStateLateGameAttacking,
                    initialStateMidGame };

            Random prng = new Random();
            int batchSize = 1024;
            var environment = new EnvironmentRts(initialStates);

            EGreedyStrategy trainingStrategy = new EGreedyStrategy(0.5f);
            GreedyStrategy evaluationStrategy = new GreedyStrategy();

            NfqArgs nfqArgs = new NfqArgs(environment,
                evaluationStrategy,
                trainingStrategy,
                batchSize: batchSize,
                maxMinutes: 0.01f);//480f);

            Nfq nfq = new Nfq(nfqArgs);
            NfqResult result = nfq.Train();

            string file = fileNameNoExt + "_0_" + DateTime.Now.ToString("yyyy_MM_dd-HH_mm") + ".bin";
            for (int i = 0; File.Exists(file); i++)
                file = fileNameNoExt + "_" + i + "_" + DateTime.Now.ToString("yyyy_MM_dd-HH_mm") + ".bin";

            Serializer.SerializeObject(file, result.ToNeuralNetworkResults());

            Console.WriteLine("\n" + result.EndReason + " Model saved as: " + file);
            
            Console.ReadKey();
        }

        private static void TestModel(string _filename)
        {
            if (!File.Exists(_filename))
                throw new ArgumentException("File name not existant");


            TrainingResult _trainingResult = Serializer.DeserializeObject(_filename);
            NeuralNetwork nn = new NeuralNetwork(_trainingResult);

            double[] shouldTryDefend = new double[]
            {
                20.0f, //EEnemyInput.TotalGhouls
                16.0f, //EEnemyInput.IdlingGhouls
                4.0f, //EEnemyInput.GhoulsInDanger
                0.0f, //EEnemyInput.HungryIdlingGhouls
                8.0f, //EEnemyInput.GhoulsWithWeapons
                0.0f, //EEnemyInput.AttackingGhouls
                0.0f, //EEnemyInput.AttackingGhoulsWithWeapons
                0.0f, //EEnemyInput.DefendingGhouls
                0.0f, //EEnemyInput.GhoulsInWorkshops
                0.0f, //EEnemyInput.DefendingGhoulsWithWeapons
                8.0f, //EEnemyInput.IdlingGhoulsWithWeapon
                16.0f, //EEnemyInput.IdlingGhoulsNotHungry
                0.0f, //EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UsedWorkshopsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnassignedFoodsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnfinishedTribesInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger
                1.0f, //EEnemyInput.TribesDefensive
                1.0f, //EEnemyInput.TribesAggresive
                0.0f, //EEnemyInput.Churches
                0.0f, //EEnemyInput.UnbalancedTribes
                0.0f, //EEnemyInput.BuildingTribeGhouls
                0.0f, //EEnemyInput.BuildingWorkshopGhouls
            };

            double[] shouldAttack = new double[]
            {
                20.0f, //EEnemyInput.TotalGhouls
                20.0f, //EEnemyInput.IdlingGhouls
                0.0f, //EEnemyInput.GhoulsInDanger
                0.0f, //EEnemyInput.HungryIdlingGhouls
                8.0f, //EEnemyInput.GhoulsWithWeapons
                0.0f, //EEnemyInput.AttackingGhouls
                0.0f, //EEnemyInput.AttackingGhoulsWithWeapons
                0.0f, //EEnemyInput.DefendingGhouls
                0.0f, //EEnemyInput.GhoulsInWorkshops
                0.0f, //EEnemyInput.DefendingGhoulsWithWeapons
                8.0f, //EEnemyInput.IdlingGhoulsWithWeapon
                20.0f, //EEnemyInput.IdlingGhoulsNotHungry
                0.0f, //EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UsedWorkshopsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnassignedFoodsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnfinishedTribesInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger
                1.0f, //EEnemyInput.TribesDefensive
                1.0f, //EEnemyInput.TribesAggresive
                0.0f, //EEnemyInput.Churches
                0.0f, //EEnemyInput.UnbalancedTribes
                0.0f, //EEnemyInput.BuildingTribeGhouls
                0.0f, //EEnemyInput.BuildingWorkshopGhouls
            };

            double[] shouldEat = new double[]
            {
                20.0f, //EEnemyInput.TotalGhouls
                20.0f, //EEnemyInput.IdlingGhouls
                0.0f, //EEnemyInput.GhoulsInDanger
                20.0f, //EEnemyInput.HungryIdlingGhouls
                0.0f, //EEnemyInput.GhoulsWithWeapons
                0.0f, //EEnemyInput.AttackingGhouls
                0.0f, //EEnemyInput.AttackingGhoulsWithWeapons
                0.0f, //EEnemyInput.DefendingGhouls
                0.0f, //EEnemyInput.GhoulsInWorkshops
                0.0f, //EEnemyInput.DefendingGhoulsWithWeapons
                8.0f, //EEnemyInput.IdlingGhoulsWithWeapon
                0.0f, //EEnemyInput.IdlingGhoulsNotHungry
                0.0f, //EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UsedWorkshopsInRangeAndNotInDanger
                20.0f, //EEnemyInput.UnassignedFoodsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnfinishedTribesInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger
                1.0f, //EEnemyInput.TribesDefensive
                1.0f, //EEnemyInput.TribesAggresive
                0.0f, //EEnemyInput.Churches
                0.0f, //EEnemyInput.UnbalancedTribes
                0.0f, //EEnemyInput.BuildingTribeGhouls
                0.0f, //EEnemyInput.BuildingWorkshopGhouls
            };

            double[] shouldTryBalanceTribes = new double[]
            {
                20.0f, //EEnemyInput.TotalGhouls
                20.0f, //EEnemyInput.IdlingGhouls
                0.0f, //EEnemyInput.GhoulsInDanger
                0.0f, //EEnemyInput.HungryIdlingGhouls
                8.0f, //EEnemyInput.GhoulsWithWeapons
                0.0f, //EEnemyInput.AttackingGhouls
                0.0f, //EEnemyInput.AttackingGhoulsWithWeapons
                0.0f, //EEnemyInput.DefendingGhouls
                0.0f, //EEnemyInput.GhoulsInWorkshops
                0.0f, //EEnemyInput.DefendingGhoulsWithWeapons
                0.0f, //EEnemyInput.IdlingGhoulsWithWeapon
                20.0f, //EEnemyInput.IdlingGhoulsNotHungry
                0.0f, //EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UsedWorkshopsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnassignedFoodsInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnfinishedTribesInRangeAndNotInDanger
                0.0f, //EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger
                1.0f, //EEnemyInput.TribesDefensive
                1.0f, //EEnemyInput.TribesAggresive
                0.0f, //EEnemyInput.Churches
                4.0f, //EEnemyInput.UnbalancedTribes
                0.0f, //EEnemyInput.BuildingTribeGhouls
                0.0f, //EEnemyInput.BuildingWorkshopGhouls
            };

            Random prng = new Random();
            GreedyStrategy greedy = new GreedyStrategy();

            Console.WriteLine("Should defend: " + (EEnemyOperation)greedy.SelectAction(shouldTryDefend, nn, prng));
            Console.WriteLine("Should attack: " + (EEnemyOperation)greedy.SelectAction(shouldAttack, nn, prng));
            Console.WriteLine("Should eat: " + (EEnemyOperation)greedy.SelectAction(shouldEat, nn, prng));
            Console.WriteLine("Should balance tribes: " + (EEnemyOperation)greedy.SelectAction(shouldTryBalanceTribes, nn, prng));

            Console.ReadKey();
        }
    }
}
