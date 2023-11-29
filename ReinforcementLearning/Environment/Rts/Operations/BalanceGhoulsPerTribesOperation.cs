using System.Collections.Generic;

namespace ReinforcementLearning
{
    public class BalanceGhoulsPerTribesOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.BalanceGhoulsPerTribes; }

        public BalanceGhoulsPerTribesOperation()
        {
        }

        public BalanceGhoulsPerTribesOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            _stats.UnbalancedTribes = 0;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.IdlingGhouls, (int)_stats.UnbalancedTribes);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _idlingGhouls, int _unbalancedTribes) => 
            _idlingGhouls > _unbalancedTribes && _unbalancedTribes > 0;
    }
}
