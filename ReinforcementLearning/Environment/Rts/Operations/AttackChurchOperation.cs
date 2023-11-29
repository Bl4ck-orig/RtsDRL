namespace ReinforcementLearning
{
    public class AttackChurchOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.AttackChurch; }

        public AttackChurchOperation()
        {
        }

        public AttackChurchOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            GhoulsAttackApplicationValues ghoulsAttackApplicationValues = new GhoulsAttackApplicationValues(
                GhoulAmountHandler.GetAmountOfGhoulsForAttack((int)_stats.IdlingGhouls),
                (int)_stats.GhoulsWithWeapons);

            _stats.AttackingGhouls += ghoulsAttackApplicationValues.GhoulsWithoutWeaponForAttack;
            _stats.AttackingGhoulsWithWeapons += ghoulsAttackApplicationValues.GhoulsWithWeaponForAttack;
            _stats.Churches--;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.Churches, (int)_stats.TotalGhouls, (int)_stats.IdlingGhouls);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _churches, int _totalGhouls, int _idlingGhouls)
        {
            if (_churches <= 0)
                return false;

            if (!GhoulAmountHandler.HasEnoughIdlingGhoulsForAttack(_totalGhouls, _idlingGhouls))
                return false;

            return true;
        }
    }
}
