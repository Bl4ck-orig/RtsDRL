namespace ReinforcementLearning
{
    public class BuildStaffWorkshopOperation : BuildWorkshopOperation
    {
        public BuildStaffWorkshopOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler, IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        public BuildStaffWorkshopOperation()
        {
        }

        public override EEnemyOperation OperationType { get => EEnemyOperation.BuildStaffWorkshop; }

        protected override void IncreaseUnusedWorkshops(EnvironmentRts _stats)
        {
            _stats.UnusedStaffWorkshopsInRangeAndNotInDanger++;
        }

        protected override double GetUnfinishedWorkshopsAmountOfStats(EnvironmentRts _stats)
        {
            return _stats.UnfinishedStaffWorkshopsInRangeAndNotInDanger;
        }

    }
}
