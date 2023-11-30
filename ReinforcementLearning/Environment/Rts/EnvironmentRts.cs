﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ReinforcementLearning
{
    public class EnvironmentRts : Environment<double[]>
    {
        public const int MAX_WEAPONS = 40;
        private const int MAX_GHOULS = 40;

        public const double AMOUNT_OF_GHOULS_FOR_BUILDING_PERCENT = 0.15f;
        public const double AMOUNT_OF_GHOULS_FOR_ATTACK_PERCENT = 0.25f;
        public const double AMOUNT_OF_GHOULS_FOR_PICKUP_WEAPONS_PERCENT = 0.25f;
        public const double AMOUNT_OF_GHOULS_FOR_TRIBE_TAKEOVER_PERCENT = 0.25f;
        public const int MIN_AMOUNT_OF_GHOULS_FOR_TRIBE_TAKE_OVER = 5;
        public const int MIN_AMOUNT_OF_GHOULS_FOR_DEFENSIVE_TRIBE_TAKE_OVER = 2;
        public const int MIN_AMOUNT_OF_GHOULS_FOR_ATTACK = 5;

        private const double DEFEAT_REWARD = -50.0f;
        private const double VICTORY_REWARD = 50.0f;

        private const double GHOULS_IN_DANGER_INCREASE_CHANCE = 0.01f;
        private const double GHOULS_IN_DANGER_KILL_CHANCE = 0.05f;
        private const double GHOULS_START_HUNGRY_CHANCE_PER_GHOUL = 0.03f;
        private const double REDUCE_GHOULS_IN_DANGER_CHANCE_PER_DEFENDING_GHOUL = 0.1f;
        private const double REDUCE_GHOULS_IN_DANGER_CHANCE_PER_DEFENDING_GHOUL_WITH_WEAPON = 0.15f;
        private const double INCREASE_WEAPONS_CHANCE_PER_WORKING_GHOUL = 0.03f;
        private const double INCREASE_TRIBES_CHANCE_PER_BUILDING_GHOUL = 0.05f;
        private const double INCREASE_WORKSHOP_CHANCE_PER_BUILDING_GHOUL = 0.05f;
        private const double REPRODUCTION_CHANCE_PER_TRIBE = 0.1f;
        private const double REPRODUCTION_CHANCE_REDUCTION_PER_UNBALANCED_TRIBE = 0.02f;
        private const double INCREASE_FOODS_CHANCE_PER_GHOUL = 0.02f;
        private const double INCREASE_FOODS_CHANCE_PER_TRIBE = 0.05f;
        private const double DECREASE_ATTACKING_GHOUL_BY_DEATH_CHANCE = 0.02f;
        private const double DECREASE_UNUSED_AGRESSIVE_TRIBES_CHANCE_PER_ATTACKING_GHOUL = 0.02f;
        private const double DECREASE_UNUSED_AGRESSIVE_TRIBES_CHANCE_PER_ATTACKING_WITH_WEAPON_GHOUL = 0.05f;

        public override int ObservationSpaceSize => State.Length;

        public override double[] State { get => GetStateVector(); protected set => SetState(value); }

        private void SetState(double[] value)
        {
            for(int i = 0; i < value.Length; i++)
            {
                Variables[(EEnemyInput)i].Value = value[i];
            }
        }

        private double[] GetStateVector()
        {
            return Variables.Values.Select(x => x.Value).ToArray();
        }

        public override int ActionSpaceSize => actions.Length;

        protected override List<double[]> InitialStates { get; set; } = new List<double[]>();

        protected override int TimeStepLimit { get; }

        public Dictionary<EWeapon,int> AliveWeaponInstances { get; internal set; }

        public Dictionary<EEnemyInput, EnemyVariable> Variables { get; private set; } = new Dictionary<EEnemyInput, EnemyVariable>()
        {
            {EEnemyInput.TotalGhouls, new EnemyVariable(0.0f, 0.0f) },
            {EEnemyInput.IdlingGhouls, new EnemyVariable(0.0f, -0.02f) },
            {EEnemyInput.GhoulsInDanger, new EnemyVariable(0.0f, -0.05f) },
            {EEnemyInput.HungryIdlingGhouls, new EnemyVariable(0.0f, 0.0f) },
            {EEnemyInput.GhoulsWithWeapons, new EnemyVariable(0.0f, 0.005f) },
            {EEnemyInput.AttackingGhouls, new EnemyVariable(0.0f, 0.025f) },
            {EEnemyInput.AttackingGhoulsWithWeapons, new EnemyVariable(0.0f, 0.05f) },
            {EEnemyInput.DefendingGhouls, new EnemyVariable(0.0f, 0.03f) },
            {EEnemyInput.GhoulsInWorkshops, new EnemyVariable(0.0f, 0.01f) },
            {EEnemyInput.DefendingGhoulsWithWeapons, new EnemyVariable(0.0f, 0.045f) },
            {EEnemyInput.IdlingGhoulsWithWeapon, new EnemyVariable(0.0f, 0.0f) },
            {EEnemyInput.IdlingGhoulsNotHungry, new EnemyVariable(0.0f, 0.0f) },
            {EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger, new EnemyVariable(0.0f, 0.05f) },
            {EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger, new EnemyVariable(0.0f, 0.03f) },
            {EEnemyInput.UsedWorkshopsInRangeAndNotInDanger, new EnemyVariable(0.0f, 0.04f) },
            {EEnemyInput.UnassignedFoodsInRangeAndNotInDanger, new EnemyVariable(0.0f, -0.05f) },
            {EEnemyInput.UnfinishedTribesInRangeAndNotInDanger, new EnemyVariable(0.0f, 0.01f) },
            {EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, new EnemyVariable(0.0f, 0.01f) },
            {EEnemyInput.TribesDefensive, new EnemyVariable(0.0f, 0.1f) },
            {EEnemyInput.TribesAggresive, new EnemyVariable(0.0f, -0.1f) },
            {EEnemyInput.Churches, new EnemyVariable(0.0f, -0.05f) },
            {EEnemyInput.UnbalancedTribes, new EnemyVariable(0.0f, -0.01f) },
            {EEnemyInput.BuildingTribeGhouls, new EnemyVariable(0.0f, 0.01f) },
            {EEnemyInput.BuildingWorkshopGhouls, new EnemyVariable(0.0f, 0.01f) }
        };

        private Dictionary<EEnemyOperation, EnemyOperation> enemyOperations = new Dictionary<EEnemyOperation, EnemyOperation>();
        private EEnemyOperation[] actions = Enum.GetValues(typeof(EEnemyOperation)) as EEnemyOperation[];

        public EnvironmentRts(List<double[]> _initialStates, int _timeStepLimit = 200)
        {
            TimeStepLimit = _timeStepLimit;
            InitialStates = _initialStates;

            EnemyOperationGhoulAmountHandler enemyOperationGhoulAmountHandler = new EnemyOperationGhoulAmountHandler();

            enemyOperations = actions
                .ToDictionary(k => k, v => OperationFactory.CreateOperation(v, null, enemyOperationGhoulAmountHandler));
        }

        protected override (double[] NextState, double Reward, bool IsTerminal) Act(int _action, Random _prng)
        {
            double currentReward = GetStateReward(State);

            if (enemyOperations[(EEnemyOperation)_action].IsSimplifiedOperationPossible(this))
                enemyOperations[(EEnemyOperation)_action].ApplySimplifiedOperation(this);

            Tick(_prng);

            bool defeat = Variables[EEnemyInput.TotalGhouls].Value == 0;
            bool victory = Variables[EEnemyInput.TribesAggresive].Value == 0;

            double deltaReward = GetStateReward(State) - currentReward;
            if (defeat)
                deltaReward += DEFEAT_REWARD;
            if (victory)
                deltaReward += VICTORY_REWARD;

            return (State, deltaReward, defeat || victory);
        }

        private void Tick(Random _prng)
        {
            if (ShouldIncreaseFoods(_prng))
                IncreaseFoods();

            if (ShouldIncreaseTribes(_prng))
                IncreaseTribes();

            if (ShouldIncreaseWorkshops(_prng))
                IncreaseWorkshops();

            if (ShouldDecreaseAggressiveTribes(_prng))
                DecreaseAggressiveTribes();

            if (ShouldDecreaseAttackingGhouls(_prng))
                DecreaseAttackingGhouls();

            if (CanGhoulsReproduce(_prng))
                Reproduction();

            if (ShouldIncreaseGhoulsInDanger(_prng))
                IncreaseGhoulsInDanger();

            if (ShouldIncreaseGhoulsHungry(_prng))
                IncreaseGhoulsHungry();

            if (ShouldDecreaseGhoulsInDanger(_prng))
                DecreaseGhoulsInDanger();

            if (ShouldIncreaseWeapons(_prng))
                IncreaseWeapons();

            if (ShouldDecreaseGhoulsInDangerByDeath(_prng))
                DecreaseGhoulsInDangerByDeath();

            TrySetUnbalancedTribes();
        }

        #region Foods & Hunger------------------------------------------------------------------------
        private bool ShouldIncreaseFoods(Random _prng)
        {
            if (_prng.NextDouble() >
                INCREASE_FOODS_CHANCE_PER_TRIBE * Variables[EEnemyInput.TribesDefensive].Value +
                INCREASE_FOODS_CHANCE_PER_GHOUL * Variables[EEnemyInput.TotalGhouls].Value)
                return false;

            return true;
        }

        private void IncreaseFoods()
        {
            Variables[EEnemyInput.UnassignedFoodsInRangeAndNotInDanger].Value++;
        }

        private bool ShouldIncreaseGhoulsHungry(Random _prng) =>
            _prng.NextDouble() < GHOULS_START_HUNGRY_CHANCE_PER_GHOUL * Variables[EEnemyInput.IdlingGhoulsNotHungry].Value;

        private void IncreaseGhoulsHungry()
        {
            Variables[EEnemyInput.IdlingGhoulsNotHungry].Value--;
            Variables[EEnemyInput.IdlingGhouls].Value--;
            Variables[EEnemyInput.HungryIdlingGhouls].Value++;
        }

        #endregion Foods & Hunger ------------------------------------------------------------------------

        #region Building Increase ------------------------------------------------------------------------
        private bool ShouldIncreaseTribes(Random _prng)
        {
            if (Variables[EEnemyInput.UnfinishedTribesInRangeAndNotInDanger].Value <= 0)
                return false;

            if (_prng.NextDouble() >
                Variables[EEnemyInput.BuildingWorkshopGhouls].Value *
                INCREASE_TRIBES_CHANCE_PER_BUILDING_GHOUL)
                return false;

            return true;
        }

        private void IncreaseTribes()
        {
            Variables[EEnemyInput.UnfinishedTribesInRangeAndNotInDanger].Value--;
            Variables[EEnemyInput.TribesDefensive].Value++;
            Variables[EEnemyInput.UnbalancedTribes].Value = Variables[EEnemyInput.TribesDefensive].Value;

            ReduceBuildingGhouls(EEnemyInput.BuildingWorkshopGhouls, EEnemyInput.TribesDefensive);
        }

        private bool ShouldIncreaseWorkshops(Random _prng)
        {
            if (Variables[EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger].Value <= 0)
                return false;

            if (_prng.NextDouble() > 
                Variables[EEnemyInput.BuildingWorkshopGhouls].Value * 
                INCREASE_WORKSHOP_CHANCE_PER_BUILDING_GHOUL)
                return false;

            return true;
        }

        private void IncreaseWorkshops()
        {
            Variables[EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger].Value--;
            Variables[EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger].Value++;

            ReduceBuildingGhouls(EEnemyInput.BuildingWorkshopGhouls, EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger);
        }

        private void ReduceBuildingGhouls(EEnemyInput _buildingGhouls, EEnemyInput _unfinishedBuildingType)
        {
            double buildingGhoulsToReduce = 0.0f;

            if (Variables[_buildingGhouls].Value > 0)
                buildingGhoulsToReduce = Math.Ceiling(Variables[_buildingGhouls].Value * 0.5f);
            else
                buildingGhoulsToReduce = Variables[_buildingGhouls].Value;

            Variables[EEnemyInput.IdlingGhouls].Value += buildingGhoulsToReduce;
            Variables[_buildingGhouls].Value -= buildingGhoulsToReduce;
        }
        #endregion Building Increase ------------------------------------------------------------------------

        #region Attacks ------------------------------------------------------------------------
        private bool ShouldDecreaseAggressiveTribes(Random _prng)
        {
            if (Variables[EEnemyInput.TribesAggresive].Value <= 0)
                return false;

            if (Variables[EEnemyInput.AttackingGhouls].Value <= 0 && Variables[EEnemyInput.AttackingGhoulsWithWeapons].Value <= 0)
                return false;

            if (_prng.NextDouble() > 
                Variables[EEnemyInput.AttackingGhouls].Value * DECREASE_UNUSED_AGRESSIVE_TRIBES_CHANCE_PER_ATTACKING_GHOUL +
                Variables[EEnemyInput.AttackingGhoulsWithWeapons].Value * DECREASE_UNUSED_AGRESSIVE_TRIBES_CHANCE_PER_ATTACKING_WITH_WEAPON_GHOUL)
                return false;

            return true;
        }

        private void DecreaseAggressiveTribes()
        {
            Variables[EEnemyInput.TribesAggresive].Value--;
        }

        private bool ShouldDecreaseAttackingGhouls(Random _prng)
        {
            if (Variables[EEnemyInput.TribesAggresive].Value <= 0)
                return false;

            if (Variables[EEnemyInput.AttackingGhouls].Value <= 0 && Variables[EEnemyInput.AttackingGhoulsWithWeapons].Value <= 0)
                return false;

            if (_prng.NextDouble() >
                Variables[EEnemyInput.AttackingGhouls].Value * DECREASE_ATTACKING_GHOUL_BY_DEATH_CHANCE +
                Variables[EEnemyInput.AttackingGhoulsWithWeapons].Value * DECREASE_ATTACKING_GHOUL_BY_DEATH_CHANCE)
                return false;

            return true;
        }

        private void DecreaseAttackingGhouls()
        {
            if(Variables[EEnemyInput.AttackingGhouls].Value > 0)
                Variables[EEnemyInput.AttackingGhouls].Value--;
            else
                Variables[EEnemyInput.AttackingGhoulsWithWeapons].Value--;
            Variables[EEnemyInput.TotalGhouls].Value--;
        }
        #endregion Attacks ------------------------------------------------------------------------

        #region Ghouls In Danger ------------------------------------------------------------------------
        private bool ShouldDecreaseGhoulsInDanger(Random _prng) =>
          _prng.NextDouble() <
           REDUCE_GHOULS_IN_DANGER_CHANCE_PER_DEFENDING_GHOUL * Variables[EEnemyInput.DefendingGhouls].Value +
           REDUCE_GHOULS_IN_DANGER_CHANCE_PER_DEFENDING_GHOUL_WITH_WEAPON * Variables[EEnemyInput.DefendingGhoulsWithWeapons].Value;

        private void DecreaseGhoulsInDanger()
        {
            Variables[EEnemyInput.GhoulsInDanger].Value--;
        }

        private bool ShouldDecreaseGhoulsInDangerByDeath(Random _prng)
        {
            if (Variables[EEnemyInput.GhoulsInDanger].Value <= 0)
                return false;

            if (_prng.NextDouble() > Variables[EEnemyInput.GhoulsInDanger].Value * GHOULS_IN_DANGER_KILL_CHANCE)
                return false;

            return true;
        }

        private void DecreaseGhoulsInDangerByDeath()
        {
            Variables[EEnemyInput.GhoulsInDanger].Value--;
            Variables[EEnemyInput.TotalGhouls].Value--;
        }

        private bool ShouldIncreaseGhoulsInDanger(Random _prng) => _prng.NextDouble() < GHOULS_IN_DANGER_INCREASE_CHANCE;

        private void IncreaseGhoulsInDanger()
        {
            Variables[EEnemyInput.GhoulsInDanger].Value++;
        }
        #endregion Ghouls In Danger ------------------------------------------------------------------------

        #region Weapons ------------------------------------------------------------------------
        private bool ShouldIncreaseWeapons(Random _prng)
        {
            if (Variables[EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger].Value >= MAX_WEAPONS)
                return false;

            if (_prng.NextDouble() > Variables[EEnemyInput.GhoulsInWorkshops].Value * INCREASE_WEAPONS_CHANCE_PER_WORKING_GHOUL)
                return false;

            return true;
        }

        private void IncreaseWeapons()
        {
            Variables[EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger].Value++;
        }
        #endregion Weapons ------------------------------------------------------------------------

        #region Balance Tribs ------------------------------------------------------------------------
        private void TrySetUnbalancedTribes()
        {
            if (Variables[EEnemyInput.TribesDefensive].Value <= 1)
                return;

            if (Variables[EEnemyInput.TotalGhouls].Value % Variables[EEnemyInput.TribesDefensive].Value == 0)
                return;

            Variables[EEnemyInput.UnbalancedTribes].Value = Variables[EEnemyInput.TribesDefensive].Value;
        }
        #endregion Balance Tribs ------------------------------------------------------------------------

        #region Reproduction ------------------------------------------------------------------------

        private bool CanGhoulsReproduce(Random _prng)
        {
            if (Variables[EEnemyInput.IdlingGhoulsNotHungry].Value < 1)
                return false;

            if (Variables[EEnemyInput.TribesDefensive].Value <= 0)
                return false;

            if (Variables[EEnemyInput.TotalGhouls].Value >= MAX_GHOULS)
                return false;

            if (_prng.NextDouble() > Variables[EEnemyInput.TribesDefensive].Value * REPRODUCTION_CHANCE_PER_TRIBE -
                Variables[EEnemyInput.UnbalancedTribes].Value * REPRODUCTION_CHANCE_REDUCTION_PER_UNBALANCED_TRIBE)
                return false;

            return true;
        }

        private void Reproduction()
        {
            Variables[EEnemyInput.TotalGhouls].Value++;
            Variables[EEnemyInput.IdlingGhoulsNotHungry].Value++;
            Variables[EEnemyInput.IdlingGhouls].Value++;
        }
        #endregion Reproduction ------------------------------------------------------------------------

        private double GetStateReward(double[] _state)
        {
            double sum = 0.0f;

            for(int i = 0; i < _state.Length; i++)
            {
                sum += Variables[(EEnemyInput)i].Reward * _state[i];
            }

            return sum;
        }

        
    }
}
