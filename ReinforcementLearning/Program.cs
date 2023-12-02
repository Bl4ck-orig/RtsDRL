using ReinforcementLearning.Training;
using ReinforcementLearning.Utils;
using System;
using System.Collections.Generic;

namespace ReinforcementLearning
{
    internal class Program
    {
        private static string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Model.bin";

        static void Main(string[] args)
        {
            //RunQLearning();

            RunNfq();

            //TestModel(Serializer.DeserializeObject(fileName));
        }

        private static void RunQLearning()
        {
            QLearningResult qLearningResult = QLearning.Learn(new QLearningArgs(new EnvironemntFrozenLake()));

            Console.WriteLine("\n" + qLearningResult.ToString());

            Console.ReadLine();
        }

        private static void RunNfq()
        {
            Dictionary<EEnemyInput, double> initialState = new Dictionary<EEnemyInput, double>()
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

            Random prng = new Random();
            int batchSize = 1024;
            var environment = new EnvironmentRts(new List<Dictionary<EEnemyInput, double>>() { initialState });

            EGreedyStrategy trainingStrategy = new EGreedyStrategy(0.5f);
            GreedyStrategy evaluationStrategy = new GreedyStrategy();

            NfqArgs nfqArgs = new NfqArgs(environment,
                evaluationStrategy, 
                trainingStrategy, 
                batchSize: batchSize,
                maxMinutes: 0.01f);

            Nfq nfq = new Nfq(nfqArgs);
            NfqResult result = nfq.Train();

            Serializer.SerializeObject(fileName, result.ToNeuralNetworkResults());

            Console.WriteLine("\n" + result.EndReason + ". Model saved to Desktop.");
            
            Console.ReadKey();
        }

        private static void TestModel(TrainingResult _trainingResult)
        {
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
                8.0f, //EEnemyInput.GhoulsWithWeapons
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
