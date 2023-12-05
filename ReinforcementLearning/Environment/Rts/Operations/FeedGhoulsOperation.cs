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
            double amountOfGhoulsToReduceHungry = 0f;

            if (_stats.Variables[EEnemyInput.HungryIdlingGhouls].Value >
               _stats.Variables[EEnemyInput.UnassignedFoodsInRangeAndNotInDanger].Value)
                amountOfGhoulsToReduceHungry = _stats.Variables[EEnemyInput.HungryIdlingGhouls].Value;
            else 
                amountOfGhoulsToReduceHungry = _stats.Variables[EEnemyInput.UnassignedFoodsInRangeAndNotInDanger].Value;

            _stats.Variables[EEnemyInput.HungryIdlingGhouls].Value -= amountOfGhoulsToReduceHungry;
            _stats.Variables[EEnemyInput.IdlingGhouls].Value += amountOfGhoulsToReduceHungry;
            _stats.Variables[EEnemyInput.IdlingGhoulsNotHungry].Value += amountOfGhoulsToReduceHungry;

        }
                

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.Variables[EEnemyInput.HungryIdlingGhouls].Value, 
                (int)_stats.Variables[EEnemyInput.UnassignedFoodsInRangeAndNotInDanger].Value);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _hungryGhouls, int _unassignedFoodsInRangeAndNotInDanger)
        {
            if (_hungryGhouls <= 0)
                return false;

            if (_unassignedFoodsInRangeAndNotInDanger <= 0)
                return false;

            return true;
        }
    }
}
