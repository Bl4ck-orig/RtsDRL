using System.Collections.Generic;

namespace ReinforcementLearning
{
    public static class StartStates
    {

        public static Dictionary<string, Dictionary<EEnemyInput, double>> StartStatesByLabel
        {
            get
            {
                return new Dictionary<string, Dictionary<EEnemyInput, double>>()
                {
                    { "InitialStateStandard", initialStateStandard},
                    { "InitialStateLateGame", initialStateLateGame},
                    { "InitialStateLateGameDefending", initialStateLateGameDefending},
                    { "InitialStateLateGameAttacking", initialStateLateGameAttacking},
                    { "InitialStateMidGame", initialStateMidGame},
                    { "ShouldTryDefend", shouldTryDefend},
                    { "ShouldAttack", shouldAttack},
                    { "ShouldEat", shouldEat},
                    { "ShouldTryBalanceTribes", shouldTryBalanceTribes},
                };                
            }
        }

        public static Dictionary<EEnemyInput, double> initialStateStandard = new Dictionary<EEnemyInput, double>()
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

        public static Dictionary<EEnemyInput, double> initialStateLateGame = new Dictionary<EEnemyInput, double>()
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

        public static Dictionary<EEnemyInput, double> initialStateLateGameDefending = new Dictionary<EEnemyInput, double>()
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
                { EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.TribesDefensive, 3.0f },
                { EEnemyInput.TribesAggresive, 3.0f },
                { EEnemyInput.Churches, 1.0f },
                { EEnemyInput.UnbalancedTribes, 0.0f },
                { EEnemyInput.BuildingTribeGhouls, 0.0f },
                { EEnemyInput.BuildingWorkshopGhouls, 0.0f}
            };

        public static Dictionary<EEnemyInput, double> initialStateLateGameAttacking = new Dictionary<EEnemyInput, double>()
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
                { EEnemyInput.UnfinishedTribesInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, 1.0f },
                { EEnemyInput.TribesDefensive, 3.0f },
                { EEnemyInput.TribesAggresive, 3.0f },
                { EEnemyInput.Churches, 1.0f },
                { EEnemyInput.UnbalancedTribes, 0.0f },
                { EEnemyInput.BuildingTribeGhouls, 0.0f },
                { EEnemyInput.BuildingWorkshopGhouls, 0.0f }
            };

        public static Dictionary<EEnemyInput, double> initialStateMidGame = new Dictionary<EEnemyInput, double>()
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
                { EEnemyInput.TribesDefensive, 2.0f },
                { EEnemyInput.TribesAggresive, 2.0f },
                { EEnemyInput.Churches, 1.0f },
                { EEnemyInput.UnbalancedTribes, 0.0f },
                { EEnemyInput.BuildingTribeGhouls, 2.0f },
                { EEnemyInput.BuildingWorkshopGhouls, 2.0f }
            };

        public static Dictionary<EEnemyInput, double> shouldTryDefend = new Dictionary<EEnemyInput, double>()
            {
                { EEnemyInput.TotalGhouls, 20.0f },
                { EEnemyInput.IdlingGhouls, 16.0f },
                { EEnemyInput.GhoulsInDanger, 4.0f },
                { EEnemyInput.HungryIdlingGhouls, 0.0f },
                { EEnemyInput.GhoulsWithWeapons, 8.0f },
                { EEnemyInput.AttackingGhouls, 0.0f },
                { EEnemyInput.AttackingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.DefendingGhouls, 0.0f },
                { EEnemyInput.GhoulsInWorkshops, 0.0f },
                { EEnemyInput.DefendingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.IdlingGhoulsWithWeapon, 8.0f },
                { EEnemyInput.IdlingGhoulsNotHungry, 16.0f },
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
                { EEnemyInput.BuildingWorkshopGhouls, 0.0f  },
            };

        public static Dictionary<EEnemyInput, double> shouldAttack = new Dictionary<EEnemyInput, double>()
        {
                { EEnemyInput.TotalGhouls, 20.0f  },
                { EEnemyInput.IdlingGhouls, 20.0f },
                { EEnemyInput.GhoulsInDanger, 0.0f },
                { EEnemyInput.HungryIdlingGhouls, 0.0f },
                { EEnemyInput.GhoulsWithWeapons, 8.0f },
                { EEnemyInput.AttackingGhouls, 0.0f },
                { EEnemyInput.AttackingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.DefendingGhouls, 0.0f },
                { EEnemyInput.GhoulsInWorkshops, 0.0f },
                { EEnemyInput.DefendingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.IdlingGhoulsWithWeapon, 8.0f },
                { EEnemyInput.IdlingGhoulsNotHungry, 20.0f },
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
                { EEnemyInput.BuildingWorkshopGhouls, 0.0f },
        };

        public static Dictionary<EEnemyInput, double> shouldEat = new Dictionary<EEnemyInput, double>()
        {
                { EEnemyInput.TotalGhouls, 20.0f  },
                { EEnemyInput.IdlingGhouls, 0.0f },
                { EEnemyInput.GhoulsInDanger, 0.0f },
                { EEnemyInput.HungryIdlingGhouls, 20.0f },
                { EEnemyInput.GhoulsWithWeapons, 0.0f },
                { EEnemyInput.AttackingGhouls, 0.0f },
                { EEnemyInput.AttackingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.DefendingGhouls, 0.0f },
                { EEnemyInput.GhoulsInWorkshops, 0.0f },
                { EEnemyInput.DefendingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.IdlingGhoulsWithWeapon, 8.0f },
                { EEnemyInput.IdlingGhoulsNotHungry, 0.0f },
                { EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UsedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnassignedFoodsInRangeAndNotInDanger, 20.0f },
                { EEnemyInput.UnfinishedTribesInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.TribesDefensive, 1.0f },
                { EEnemyInput.TribesAggresive, 1.0f },
                { EEnemyInput.Churches, 0.0f },
                { EEnemyInput.UnbalancedTribes, 0.0f },
                { EEnemyInput.BuildingTribeGhouls, 0.0f },
                { EEnemyInput.BuildingWorkshopGhouls, 0.0f },
        };

        public static Dictionary<EEnemyInput, double> shouldTryBalanceTribes = new Dictionary<EEnemyInput, double>()
        {
                { EEnemyInput.TotalGhouls, 20.0f },
                { EEnemyInput.IdlingGhouls, 20.0f },
                { EEnemyInput.GhoulsInDanger, 0.0f },
                { EEnemyInput.HungryIdlingGhouls, 0.0f },
                { EEnemyInput.GhoulsWithWeapons, 8.0f },
                { EEnemyInput.AttackingGhouls, 0.0f },
                { EEnemyInput.AttackingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.DefendingGhouls, 0.0f },
                { EEnemyInput.GhoulsInWorkshops, 0.0f },
                { EEnemyInput.DefendingGhoulsWithWeapons, 0.0f },
                { EEnemyInput.IdlingGhoulsWithWeapon, 0.0f },
                { EEnemyInput.IdlingGhoulsNotHungry, 20.0f },
                { EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UsedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnassignedFoodsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnfinishedTribesInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, 0.0f },
                { EEnemyInput.TribesDefensive, 4.0f },
                { EEnemyInput.TribesAggresive, 1.0f },
                { EEnemyInput.Churches, 0.0f },
                { EEnemyInput.UnbalancedTribes, 4.0f },
                { EEnemyInput.BuildingTribeGhouls, 0.0f },
                { EEnemyInput.BuildingWorkshopGhouls, 0.0f  },
        };

    }
}
