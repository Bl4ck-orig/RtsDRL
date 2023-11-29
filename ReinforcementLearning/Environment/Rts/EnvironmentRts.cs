using System;
using System.Collections.Generic;
using System.Linq;

namespace ReinforcementLearning
{
    public class EnvironmentRts : Environment<double[]>
    {
        public const int MAX_WEAPONS = 40;

        public override int ObservationSpaceSize => State.Length;

        public override double[] State { get => GetStateVector(); protected set => SetState(value); }

        private void SetState(double[] value)
        {
            int index = 0;

            TotalGhouls = value[index++];
            IdlingGhouls = value[index++];
            GhoulsInDanger = value[index++];
            HungryIdlingGhouls = value[index++];
            GhoulsWithWeapons = value[index++];
            AttackingGhouls = value[index++];
            AttackingGhoulsWithWeapons = value[index++];
            DefendingGhouls = value[index++];
            GhoulsInWorkshops = value[index++];
            DefendingGhoulsWithWeapons = value[index++];
            IdlingGhoulsWithWeapon = value[index++];
            IdlingGhoulsNotHungry = value[index++];
            UnassignedWeaponsInRangeAndNotInDanger = value[index++];
            UnusedStaffWorkshopsInRangeAndNotInDanger = value[index++];
            UnusedSwordWorkshopsInRangeAndNotInDanger = value[index++];
            UsedWorkshopsInRangeAndNotInDanger = value[index++];
            UnassignedFoodsInRangeAndNotInDanger = value[index++];
            UnfinishedTribesInRangeAndNotInDanger = value[index++];
            UnfinishedStaffWorkshopsInRangeAndNotInDanger = value[index++];
            UnfinishedSwordWorkshopsInRangeAndNotInDanger = value[index++];
            TribesDefensive = value[index++];
            UnusedTribesDefensive = value[index++];
            TribesAggressive = value[index++];
            UnusedTribesAggressive = value[index++];
            Churches = value[index++];
            UnbalancedTribes = value[index++];
        }

        private double[] GetStateVector()
        {
            return new double[]
            {
                TotalGhouls,
                IdlingGhouls,
                GhoulsInDanger,
                HungryIdlingGhouls,
                GhoulsWithWeapons,
                AttackingGhouls,
                AttackingGhoulsWithWeapons,
                DefendingGhouls,
                GhoulsInWorkshops,
                DefendingGhoulsWithWeapons,
                IdlingGhoulsWithWeapon,
                IdlingGhoulsNotHungry,
                UnassignedWeaponsInRangeAndNotInDanger,
                UnusedStaffWorkshopsInRangeAndNotInDanger,
                UnusedSwordWorkshopsInRangeAndNotInDanger,
                UsedWorkshopsInRangeAndNotInDanger,
                UnassignedFoodsInRangeAndNotInDanger,
                UnfinishedTribesInRangeAndNotInDanger,
                UnfinishedStaffWorkshopsInRangeAndNotInDanger,
                UnfinishedSwordWorkshopsInRangeAndNotInDanger,
                TribesDefensive,
                UnusedTribesDefensive,
                TribesAggressive,
                UnusedTribesAggressive,
                Churches,
                UnbalancedTribes,
            };
        }

        public override int ActionSpaceSize => actions.Length;

        protected override List<double[]> InitialStates { get; set; } = new List<double[]>();

        protected override int TimeStepLimit { get; }

        public Dictionary<EWeapon,int> AliveWeaponInstances { get; internal set; }

        public double TotalGhouls;
        public double IdlingGhouls;
        public double GhoulsInDanger;
        public double HungryIdlingGhouls;
        public double GhoulsWithWeapons;
        public double AttackingGhouls;
        public double AttackingGhoulsWithWeapons;
        public double DefendingGhouls;
        public double GhoulsInWorkshops;
        public double DefendingGhoulsWithWeapons;
        public double IdlingGhoulsWithWeapon;
        public double IdlingGhoulsNotHungry;
        public double UnassignedWeaponsInRangeAndNotInDanger;
        public double UnusedStaffWorkshopsInRangeAndNotInDanger;
        public double UnusedSwordWorkshopsInRangeAndNotInDanger;
        public double UsedWorkshopsInRangeAndNotInDanger;
        public double UnassignedFoodsInRangeAndNotInDanger;
        public double UnfinishedTribesInRangeAndNotInDanger;
        public double UnfinishedStaffWorkshopsInRangeAndNotInDanger;
        public double UnfinishedSwordWorkshopsInRangeAndNotInDanger;
        public double TribesDefensive;
        public double UnusedTribesDefensive;
        public double TribesAggressive;
        public double UnusedTribesAggressive;
        public double Churches;
        public double UnbalancedTribes;

        private Dictionary<EEnemyOperation, EnemyOperation> enemyOperations = new Dictionary<EEnemyOperation, EnemyOperation>();
        private EEnemyOperation[] actions = Enum.GetValues(typeof(EEnemyOperation)) as EEnemyOperation[];

        public EnvironmentRts(List<double[]> _initialStates, int _timeStepLimit = 200)
        {
            TimeStepLimit = _timeStepLimit;
            InitialStates = _initialStates;

            EnemyOperationGhoulAmountHandler enemyOperationGhoulAmountHandler = new EnemyOperationGhoulAmountHandler(new RtsConfig());

            enemyOperations = actions
                .ToDictionary(k => k, v => OperationFactory.CreateOperation(v, null, enemyOperationGhoulAmountHandler));
        }

        protected override (double[] NextState, double Reward, bool IsTerminal) Act(int _action, Random _prng)
        {
            Reward berechnen
        }
    }
}
