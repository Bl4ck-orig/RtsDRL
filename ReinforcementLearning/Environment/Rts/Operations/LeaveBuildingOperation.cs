namespace ReinforcementLearning
{
    public class LeaveBuildingOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.LeaveBuilding; }

        public LeaveBuildingOperation()
        {
        }

        public LeaveBuildingOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            _stats.IdlingGhouls++;
            _stats.GhoulsInWorkshops--;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.GhoulsInWorkshops);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _ghoulsInBuilding)
        {
            if (_ghoulsInBuilding == 0)
                return false;

            return true;
        }
    }
}
