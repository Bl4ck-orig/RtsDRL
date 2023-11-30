using ReinforcementLearning.Training;
using System;
using System.Collections.Generic;

namespace ReinforcementLearning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunNfq();
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

            NfqArgs nfqArgs = new NfqArgs(environment, evaluationStrategy, trainingStrategy, batchSize: batchSize);

            Nfq nfq = new Nfq(nfqArgs);
            NfqResult result = nfq.Train();
            
            Console.WriteLine("\n" + result.EndReason);
            
            Console.ReadLine();
        }
    }
}
