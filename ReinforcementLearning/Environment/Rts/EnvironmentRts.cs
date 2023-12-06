using System;
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

        private const double DEFEAT_REWARD = -1.0f;
        private const double VICTORY_REWARD = 1.0f;
        private const double OPERATION_NOT_POSSIBLE_CHOSEN_REWARD = -0.2f;

        private const double GHOULS_IN_DANGER_INCREASE_CHANCE = 0.1f;
        private const double GHOULS_IN_DANGER_KILL_CHANCE = 0.15f;
        private const double GHOULS_START_HUNGRY_CHANCE_PER_GHOUL = 0.1f;
        private const double REDUCE_GHOULS_IN_DANGER_CHANCE_PER_DEFENDING_GHOUL = 0.15f;
        private const double REDUCE_GHOULS_IN_DANGER_CHANCE_PER_DEFENDING_GHOUL_WITH_WEAPON = 0.17f;
        private const double INCREASE_WEAPONS_CHANCE_PER_WORKING_GHOUL = 0.25f;
        private const double INCREASE_TRIBES_CHANCE_PER_BUILDING_GHOUL = 0.05f;
        private const double INCREASE_WORKSHOP_CHANCE_PER_BUILDING_GHOUL = 0.05f;
        private const double REPRODUCTION_CHANCE_PER_TRIBE = 0.05f;
        private const double REPRODUCTION_CHANCE_REDUCTION_PER_UNBALANCED_TRIBE = 0.02f;
        private const double INCREASE_FOODS_CHANCE_PER_GHOUL = 0.02f;
        private const double INCREASE_FOODS_CHANCE_PER_TRIBE = 0.05f;
        private const double DECREASE_ATTACKING_GHOUL_BY_DEATH_CHANCE = 0.1f;
        private const double DECREASE_UNUSED_AGRESSIVE_TRIBES_CHANCE_PER_ATTACKING_GHOUL = 0.05f;
        private const double DECREASE_UNUSED_AGRESSIVE_TRIBES_CHANCE_PER_ATTACKING_WITH_WEAPON_GHOUL = 0.1f;

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

        public Dictionary<EEnemyInput, EnemyVariable> Variables { get; private set; } = new Dictionary<EEnemyInput, EnemyVariable>()
        {
            /* 0  */{EEnemyInput.TotalGhouls, new EnemyVariable(0.0f, 0.0f) },
            /* 1  */{EEnemyInput.IdlingGhouls, new EnemyVariable(0.0f, -0.02f) },
            /* 2  */{EEnemyInput.GhoulsInDanger, new EnemyVariable(0.0f, -0.05f) },
            /* 3  */{EEnemyInput.HungryIdlingGhouls, new EnemyVariable(0.0f, 0.0f) },
            /* 4  */{EEnemyInput.GhoulsWithWeapons, new EnemyVariable(0.0f, 0.005f) },
            /* 5  */{EEnemyInput.AttackingGhouls, new EnemyVariable(0.0f, 0.025f) },
            /* 6  */{EEnemyInput.AttackingGhoulsWithWeapons, new EnemyVariable(0.0f, 0.05f) },
            /* 7  */{EEnemyInput.DefendingGhouls, new EnemyVariable(0.0f, 0.03f) },
            /* 8  */{EEnemyInput.GhoulsInWorkshops, new EnemyVariable(0.0f, 0.01f) },
            /* 9  */{EEnemyInput.DefendingGhoulsWithWeapons, new EnemyVariable(0.0f, 0.045f) },
            /* 10 */{EEnemyInput.IdlingGhoulsWithWeapon, new EnemyVariable(0.0f, 0.0f) },
            /* 11 */{EEnemyInput.IdlingGhoulsNotHungry, new EnemyVariable(0.0f, 0.0f) },
            /* 12 */{EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger, new EnemyVariable(0.0f, 0.0f) },
            /* 13 */{EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger, new EnemyVariable(0.0f, 0.03f) },
            /* 14 */{EEnemyInput.UsedWorkshopsInRangeAndNotInDanger, new EnemyVariable(0.0f, 0.04f) },
            /* 15 */{EEnemyInput.UnassignedFoodsInRangeAndNotInDanger, new EnemyVariable(0.0f, -0.05f) },
            /* 16 */{EEnemyInput.UnfinishedTribesInRangeAndNotInDanger, new EnemyVariable(0.0f, 0.01f) },
            /* 17 */{EEnemyInput.UnfinishedWorkshopsInRangeAndNotInDanger, new EnemyVariable(0.0f, 0.01f) },
            /* 18 */{EEnemyInput.TribesDefensive, new EnemyVariable(0.0f, 0.1f) },
            /* 19 */{EEnemyInput.TribesAggresive, new EnemyVariable(0.0f, -0.1f) },
            /* 20 */{EEnemyInput.Churches, new EnemyVariable(0.0f, -0.05f) },
            /* 21 */{EEnemyInput.UnbalancedTribes, new EnemyVariable(0.0f, -0.01f) },
            /* 22 */{EEnemyInput.BuildingTribeGhouls, new EnemyVariable(0.0f, 0.01f) },
            /* 23 */{EEnemyInput.BuildingWorkshopGhouls, new EnemyVariable(0.0f, 0.01f) }
        };

        private Dictionary<EEnemyOperation, EnemyOperation> enemyOperations = new Dictionary<EEnemyOperation, EnemyOperation>();
        private EEnemyOperation[] actions = Enum.GetValues(typeof(EEnemyOperation)) as EEnemyOperation[]; 
        private int _void = 0;

        public EnvironmentRts(List<Dictionary<EEnemyInput, double>> _initialStates)
        {
            for(int i = 0; i < _initialStates.Count; i++)
            {
                InitialStates.Add(_initialStates[i].OrderBy(x => x.Key).Select(x => x.Value).ToArray());
            }

            EnemyOperationGhoulAmountHandler enemyOperationGhoulAmountHandler = new EnemyOperationGhoulAmountHandler();

            enemyOperations = actions
                .ToDictionary(k => k, v => OperationFactory.CreateOperation(v, null, enemyOperationGhoulAmountHandler));
        }


        protected override (double[] NextState, double Reward, bool IsTerminal) Act(int _action, Random _prng)
        {
            double currentReward = GetStateReward(State);

            EEnemyOperation action = (EEnemyOperation)_action;

            bool wasOperationSelectedPossible = false;
            if (wasOperationSelectedPossible = enemyOperations[action].IsSimplifiedOperationPossible(this))
                enemyOperations[action].ApplySimplifiedOperation(this);

            Tick(_prng);

            bool defeat = Variables[EEnemyInput.TotalGhouls].Value == 0;
            bool victory = Variables[EEnemyInput.TribesAggresive].Value == 0;

            double deltaReward = GetStateReward(State) - currentReward;
            if (defeat)
                deltaReward += DEFEAT_REWARD;
            if (victory)
                deltaReward += VICTORY_REWARD;
            if (wasOperationSelectedPossible)
                deltaReward += OPERATION_NOT_POSSIBLE_CHOSEN_REWARD;

            return (State, deltaReward, defeat || victory);
        }

        private void Tick(Random _prng)
        {
            // Debugging: 
            //if (StepsCount > 100)
            //{
            //    if (_void == 0)
            //        _void = 1;
            //}
            //else
            //    _void = 0;

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
                IncreaseGhoulsInDanger(_prng);

            if (ShouldIncreaseGhoulsHungry(_prng))
                IncreaseGhoulsHungry();

            if (ShouldDecreaseGhoulsInDanger(_prng))
                DecreaseGhoulsInDanger(_prng);

            if (ShouldIncreaseWeapons(_prng))
                IncreaseWeapons();

            if (ShouldDecreaseGhoulsInDangerByDeath(_prng))
                DecreaseGhoulsInDangerByDeath();

            TryFreeDefendingGhouls(_prng);

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
            IncreaseHungryIdlingGhouls();
        }

        private void IncreaseHungryIdlingGhouls()
        {
            Variables[EEnemyInput.HungryIdlingGhouls].Value++;
        }

        private void IncreaseNotHungryGhouls()
        {
            Variables[EEnemyInput.IdlingGhouls].Value++;
            Variables[EEnemyInput.IdlingGhoulsNotHungry].Value++;
        }

        #endregion Foods & Hunger ------------------------------------------------------------------------

        #region Building Increase ------------------------------------------------------------------------
        private bool ShouldIncreaseTribes(Random _prng)
        {
            if (Variables[EEnemyInput.UnfinishedTribesInRangeAndNotInDanger].Value <= 0)
                return false;

            if (_prng.NextDouble() >
                Variables[EEnemyInput.BuildingTribeGhouls].Value *
                INCREASE_TRIBES_CHANCE_PER_BUILDING_GHOUL)
                return false;

            return true;
        }

        private void IncreaseTribes()
        {
            Variables[EEnemyInput.UnfinishedTribesInRangeAndNotInDanger].Value--;
            Variables[EEnemyInput.TribesDefensive].Value++;
            Variables[EEnemyInput.UnbalancedTribes].Value = Variables[EEnemyInput.TribesDefensive].Value;

            ReduceBuildingGhouls(EEnemyInput.BuildingTribeGhouls);
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

            ReduceBuildingGhouls(EEnemyInput.BuildingWorkshopGhouls);
        }

        private void ReduceBuildingGhouls(EEnemyInput _buildingGhouls)
        {
            double buildingGhoulsToReduce = 0.0f;

            if (Variables[_buildingGhouls].Value > 0)
                buildingGhoulsToReduce = Math.Ceiling(Variables[_buildingGhouls].Value * 0.5f);
            else
                buildingGhoulsToReduce = Variables[_buildingGhouls].Value;

            Variables[EEnemyInput.IdlingGhouls].Value += buildingGhoulsToReduce;
            Variables[EEnemyInput.IdlingGhoulsNotHungry].Value += buildingGhoulsToReduce;
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
            Variables[EEnemyInput.GhoulsInDanger].Value > 0 &&
          _prng.NextDouble() <
           REDUCE_GHOULS_IN_DANGER_CHANCE_PER_DEFENDING_GHOUL * Variables[EEnemyInput.DefendingGhouls].Value +
           REDUCE_GHOULS_IN_DANGER_CHANCE_PER_DEFENDING_GHOUL_WITH_WEAPON * Variables[EEnemyInput.DefendingGhoulsWithWeapons].Value;

        private void DecreaseGhoulsInDanger(Random _prng)
        {
            Variables[EEnemyInput.GhoulsInDanger].Value--;
            if(_prng.NextDouble() > 0.5f)
            {
                Variables[EEnemyInput.IdlingGhouls].Value++;
                Variables[EEnemyInput.IdlingGhoulsNotHungry].Value++;
            }
            else
            {
                Variables[EEnemyInput.HungryIdlingGhouls].Value++;
            }
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

        private bool ShouldIncreaseGhoulsInDanger(Random _prng)
        {
            if (_prng.NextDouble() > GHOULS_IN_DANGER_INCREASE_CHANCE)
                return false;

            if (Variables[EEnemyInput.IdlingGhouls].Value < 0 &&
                Variables[EEnemyInput.HungryIdlingGhouls].Value < 0)
                return false;

            return true;
        }

        private void IncreaseGhoulsInDanger(Random _prng)
        {
            Variables[EEnemyInput.GhoulsInDanger].Value++;

            if(Variables[EEnemyInput.IdlingGhouls].Value <= 0)
            {
                IncreaseHungryIdlingGhouls();
                return;
            }
            
            if(Variables[EEnemyInput.HungryIdlingGhouls].Value <= 0)
            {
                IncreaseNotHungryGhouls();
                return;
            }

            if (_prng.NextDouble() > 0.5f)
                IncreaseNotHungryGhouls();
            else
                IncreaseHungryIdlingGhouls();
        }

        private void TryFreeDefendingGhouls(Random _prng)
        {
            if (Variables[EEnemyInput.GhoulsInDanger].Value > 0)
                return;

            bool done = false;

            for(int i = 0; i < Variables[EEnemyInput.DefendingGhouls].Value; i++)
            {
                if (_prng.NextDouble() > 0.5f)
                    IncreaseNotHungryGhouls();
                else
                    IncreaseHungryIdlingGhouls();
            }

            Variables[EEnemyInput.DefendingGhouls].Value = 0;

            for (int i = 0; i < Variables[EEnemyInput.DefendingGhoulsWithWeapons].Value; i++)
            {
                if (_prng.NextDouble() > 0.5f)
                    IncreaseNotHungryGhouls();
                else
                    IncreaseHungryIdlingGhouls();
            }

            Variables[EEnemyInput.DefendingGhoulsWithWeapons].Value = 0;
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

            if (_prng.NextDouble() < Variables[EEnemyInput.TribesDefensive].Value * REPRODUCTION_CHANCE_PER_TRIBE -
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
