
namespace ReinforcementLearning
{
    public class DefendOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.Defend; }

        public DefendOperation()
        {
        }

        public DefendOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            GhoulsAttackApplicationValues ghoulsAttackApplicationValues = new GhoulsAttackApplicationValues(
                GhoulAmountHandler.GetAmountOfGhoulsForAttack((int)_stats.IdlingGhouls),
                (int)_stats.GhoulsWithWeapons);

            _stats.DefendingGhouls += ghoulsAttackApplicationValues.GhoulsWithoutWeaponForAttack;
            _stats.DefendingGhoulsWithWeapons += ghoulsAttackApplicationValues.GhoulsWithWeaponForAttack;
            _stats.GhoulsInDanger--;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.GhoulsInDanger, (int)_stats.IdlingGhouls);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _ghoulsInDanger, int _idlingGhouls)
        {
            if (_ghoulsInDanger == 0)
                return false;

            if (_idlingGhouls == 0)
                return false;

            return true;
        }
    }
}
