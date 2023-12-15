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

            double amountOfGhoulsForBuilding =
                GhoulAmountHandler.GetAmountOfGhoulsForBuilding((int)_stats.Variables[EEnemyInput.IdlingGhouls].Value);

            _stats.Variables[EEnemyInput.BuildingTribeGhouls].Value += amountOfGhoulsForBuilding;
            _stats.Variables[EEnemyInput.IdlingGhouls].Value -= amountOfGhoulsForBuilding;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.Variables[EEnemyInput.UnfinishedTribesInRangeAndNotInDanger].Value, 
                (int)_stats.Variables[EEnemyInput.TotalGhouls].Value, 
                (int)_stats.Variables[EEnemyInput.IdlingGhoulsNotHungry].Value);
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
