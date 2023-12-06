namespace ReinforcementLearning
{
    public abstract class BuildWorkshopOperation : EnemyOperation
    {
        public BuildWorkshopOperation()
        {
        }

        public BuildWorkshopOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            double amountOfGhoulsForBuilding =
                GhoulAmountHandler.GetAmountOfGhoulsForBuilding((int)_stats.Variables[EEnemyInput.IdlingGhouls].Value);

            _stats.Variables[EEnemyInput.BuildingWorkshopGhouls].Value += amountOfGhoulsForBuilding;
            _stats.Variables[EEnemyInput.IdlingGhouls].Value -= amountOfGhoulsForBuilding;
            _stats.Variables[EEnemyInput.IdlingGhoulsNotHungry].Value -= amountOfGhoulsForBuilding;
            //IncreaseUnusedWorkshops(_stats);
        }

        protected abstract void IncreaseUnusedWorkshops(EnvironmentRts _stats);


        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)GetUnfinishedWorkshopsAmountOfStats(_stats),
                (int)_stats.Variables[EEnemyInput.TotalGhouls].Value,
                (int)_stats.Variables[EEnemyInput.IdlingGhoulsNotHungry].Value);

        protected abstract double GetUnfinishedWorkshopsAmountOfStats(EnvironmentRts _stats);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _unfinishedWorkshopsInRangeAndNotInDanger,
            int _totalGhouls, 
            int _idlingGhouls)
        {
            if (!GhoulAmountHandler.HasEnoughIdlingGhoulsForBuilding(_totalGhouls, _idlingGhouls))
                return false;

            if (_unfinishedWorkshopsInRangeAndNotInDanger <= 0)
                return false;

            return true;
        }
    }
}
