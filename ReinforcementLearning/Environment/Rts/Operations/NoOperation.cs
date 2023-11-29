namespace ReinforcementLearning
{
    public class NoOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.None; }

        public NoOperation()
        {
        }

        public NoOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) => true;

        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
        }
    }
}
