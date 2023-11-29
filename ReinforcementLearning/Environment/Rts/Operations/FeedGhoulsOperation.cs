using System;

namespace ReinforcementLearning
{
    public class FeedGhoulsOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.FeedGhouls; }

        public FeedGhoulsOperation()
        {
        }

        public FeedGhoulsOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            _stats.HungryIdlingGhouls = Math.Max(0, _stats.HungryIdlingGhouls - _stats.UnassignedFoodsInRangeAndNotInDanger);
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.HungryIdlingGhouls, (int)_stats.UnassignedFoodsInRangeAndNotInDanger);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _hungryGhouls, int _unassignedFoodsInRangeAndNotInDanger)
        {
            if (_hungryGhouls == 0)
                return false;

            if (_unassignedFoodsInRangeAndNotInDanger == 0)
                return false;

            return true;
        }
    }
}
