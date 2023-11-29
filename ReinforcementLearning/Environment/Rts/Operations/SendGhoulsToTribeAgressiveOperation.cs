namespace ReinforcementLearning
{
    public class SendGhoulsToTribeAgressiveOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.SendGhoulsToTribeAggressive; }

        public SendGhoulsToTribeAgressiveOperation()
        {
        }

        public SendGhoulsToTribeAgressiveOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            _stats.TribesAggressive++;

            GhoulsAttackApplicationValues ghoulsAttackApplicationValues = new GhoulsAttackApplicationValues(
               GhoulAmountHandler.GetAmountOfGhoulsForAttack((int)_stats.IdlingGhouls),
               (int)_stats.GhoulsWithWeapons);

            _stats.AttackingGhouls += ghoulsAttackApplicationValues.GhoulsWithoutWeaponForAttack;
            _stats.AttackingGhoulsWithWeapons += ghoulsAttackApplicationValues.GhoulsWithWeaponForAttack;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.UnusedTribesAggressive, (int)_stats.TotalGhouls, (int)_stats.IdlingGhouls);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _unusedTribesAggressive, int _totalGhouls, int _idlingGhouls)
        {
            if (_unusedTribesAggressive == 0)
                return false;

            if (!GhoulAmountHandler.HasEnoughIdlingGhoulsForTribeTakeOver(_totalGhouls, _idlingGhouls))
                return false;

            return true;
        }
    }
}
