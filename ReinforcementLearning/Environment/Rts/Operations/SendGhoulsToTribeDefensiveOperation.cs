using System.Linq;

namespace ReinforcementLearning
{
    public class SendGhoulsToTribeDefensiveOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.SendGhoulsToTribeDefensive; }

        public SendGhoulsToTribeDefensiveOperation()
        {
        }

        public SendGhoulsToTribeDefensiveOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            _stats.Variables[EEnemyInput.TribesDefensive].Value++;
            _stats.Variables[EEnemyInput.IdlingGhouls].Value -= 
                GhoulAmountHandler.GetAmountOfGhoulsForTribeTakeOver((int)_stats.Variables[EEnemyInput.IdlingGhouls].Value);
        }

        [System.Obsolete("Not usable in this simulation")]
        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) => false;
            //IsOperationPossible((int)_stats.Variables[EEnemyInput.TotalGhouls].Value, 
            //    (int)_stats.Variables[EEnemyInput.IdlingGhouls].Value);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _totalGhouls, int _idlingGhouls)
        {
            if (!GhoulAmountHandler.HasEnoughIdlingGhoulsForDefensiveTribeTakeOver(_totalGhouls, _idlingGhouls))
                return false;

            return true;
        }
    }
}
