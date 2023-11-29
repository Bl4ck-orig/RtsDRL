namespace ReinforcementLearning
{
    public class BuildSwordWorkshopOperation : BuildWorkshopOperation
    {
        public BuildSwordWorkshopOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler, IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        public BuildSwordWorkshopOperation()
        {
        }

        public override EEnemyOperation OperationType { get => EEnemyOperation.BuildSwordWorkshop; }

        protected override void IncreaseUnusedWorkshops(EnvironmentRts _stats)
        {
            _stats.UnusedSwordWorkshopsInRangeAndNotInDanger++;
        }

        protected override double GetUnfinishedWorkshopsAmountOfStats(EnvironmentRts _stats)
        {
            return _stats.UnfinishedSwordWorkshopsInRangeAndNotInDanger;
        }
        
    }
}
