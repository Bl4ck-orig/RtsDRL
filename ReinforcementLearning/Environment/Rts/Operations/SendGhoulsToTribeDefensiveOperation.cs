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
            _stats.TribesDefensive++;
            _stats.IdlingGhouls -= GhoulAmountHandler.GetAmountOfGhoulsForTribeTakeOver((int)_stats.IdlingGhouls);
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.UnusedTribesDefensive, (int)_stats.TotalGhouls, (int)_stats.IdlingGhouls);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _unusedTribesDefensive, int _totalGhouls, int _idlingGhouls)
        {
            if (_unusedTribesDefensive == 0)
                return false;

            if (!GhoulAmountHandler.HasEnoughIdlingGhoulsForDefensiveTribeTakeOver(_totalGhouls, _idlingGhouls))
                return false;

            return true;
        }
    }
}
