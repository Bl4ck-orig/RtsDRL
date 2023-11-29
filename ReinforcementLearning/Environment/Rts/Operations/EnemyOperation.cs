using System;
using System.Numerics;

namespace ReinforcementLearning
{
    public abstract class EnemyOperation
    {
        public abstract EEnemyOperation OperationType { get; }

        protected IEnemyAiOperationDataChangedHandler DataChangedHandler { get; private set; }

        protected IEnemyOperationGhoulAmountHandler GhoulAmountHandler { get; private set; }

        public EnemyOperation()
        {
        }

        public EnemyOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler, IEnemyOperationGhoulAmountHandler _ghoulAmountHandler)
        {
            DataChangedHandler = _dataChangedHandler;
            GhoulAmountHandler = _ghoulAmountHandler;
        }

        #region Simplified -----------------------------------------------------------------
        public bool TryApplySimplifiedOperation(EnvironmentRts _stats)
        {
            if (!IsSimplifiedOperationPossible(_stats))
                return false;
            ApplySimplifiedOperation(_stats);
            return true;
        }

        public abstract bool IsSimplifiedOperationPossible(EnvironmentRts _stats);

        public abstract void ApplySimplifiedOperation(EnvironmentRts _stats);
        #endregion -----------------------------------------------------------------

        #region Ghouls For Attack -----------------------------------------------------------------
        protected (int WithWeapon, int NoWeapon) GetGhoulsForAttack(int _ghoulsForAttackAmount, EnvironmentRts _data, Vector3 _target)
        {
            int ghoulsWithWeapon = Math.Min(_ghoulsForAttackAmount, (int)_data.IdlingGhoulsWithWeapon);
            int ghoulsWithoutWeapon = Math.Max(_ghoulsForAttackAmount - ghoulsWithWeapon, 0);
            int withWeapon = Math.Max(ghoulsWithWeapon, 0);
            int noWeapon = Math.Max(ghoulsWithoutWeapon, 0);
            return (withWeapon, noWeapon);
        }
        #endregion -----------------------------------------------------------------

        protected struct GhoulsAttackApplicationValues
        {
            public int GhoulsWithoutWeaponForAttack { get; private set; }

            public int GhoulsWithWeaponForAttack { get; private set; }

            public GhoulsAttackApplicationValues(int _ghoulsForAttackAmount, int _ghoulsWithWeapons)
            {
                GhoulsWithoutWeaponForAttack = Math.Max(0, _ghoulsForAttackAmount - _ghoulsWithWeapons);
                GhoulsWithWeaponForAttack = _ghoulsForAttackAmount - GhoulsWithoutWeaponForAttack;
            }
        }
    }
}
