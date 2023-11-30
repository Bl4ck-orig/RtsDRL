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
            _stats.Variables[EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger].Value++;
        }

        protected override double GetUnfinishedWorkshopsAmountOfStats(EnvironmentRts _stats)
        {
            return _stats.Variables[EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger].Value;
        }
        
    }
}
