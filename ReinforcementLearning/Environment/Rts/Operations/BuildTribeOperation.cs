namespace ReinforcementLearning
{
    public class BuildTribeOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.BuildTribe; }

        public BuildTribeOperation()
        {
        }

        public BuildTribeOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            _stats.IdlingGhouls -= GhoulAmountHandler.GetAmountOfGhoulsForBuilding((int)_stats.IdlingGhouls);
            _stats.TribesDefensive++;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.UnfinishedTribesInRangeAndNotInDanger, (int)_stats.TotalGhouls, (int)_stats.IdlingGhouls);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _unfinishedTribesInRangeAndNotInDanger, int _totalGhouls, int _idlingGhouls)
        {
            if (!GhoulAmountHandler.HasEnoughIdlingGhoulsForBuilding(_totalGhouls, _idlingGhouls))
                return false;

            if (_unfinishedTribesInRangeAndNotInDanger == 0)
                return false;

            return true;
        }
    }
}
