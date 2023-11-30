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
            _stats.Variables[EEnemyInput.UnbalancedTribes].Value = 0;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.Variables[EEnemyInput.IdlingGhouls].Value, 
                (int)_stats.Variables[EEnemyInput.UnbalancedTribes].Value);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _idlingGhouls, int _unbalancedTribes) => 
            _idlingGhouls > _unbalancedTribes && _unbalancedTribes > 0;
    }
}
