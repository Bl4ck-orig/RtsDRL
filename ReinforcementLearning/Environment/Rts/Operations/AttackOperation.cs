namespace ReinforcementLearning
{
    public class AttackOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.Attack; }

        public AttackOperation()
        {
        }

        public AttackOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
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
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.TotalGhouls, (int)_stats.IdlingGhouls);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _totalGhouls, int _idlingGhouls)
        {
            if (!GhoulAmountHandler.HasEnoughIdlingGhoulsForAttack(_totalGhouls, _idlingGhouls))
                return false;

            return true;
        }
    }
}
