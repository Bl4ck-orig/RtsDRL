using System.Collections.Generic;
using System.Linq;

namespace ReinforcementLearning
{
    public abstract class BuildWorkshopOperation : EnemyOperation
    {
        public BuildWorkshopOperation()
        {
        }

        public BuildWorkshopOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            _stats.IdlingGhouls -= GhoulAmountHandler.GetAmountOfGhoulsForBuilding((int)_stats.IdlingGhouls);
            IncreaseUnusedWorkshops(_stats);
        }

        protected abstract void IncreaseUnusedWorkshops(EnvironmentRts _stats);


        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)GetUnfinishedWorkshopsAmountOfStats(_stats),
                (int)_stats.TotalGhouls,
                (int)_stats.IdlingGhouls);

        protected abstract double GetUnfinishedWorkshopsAmountOfStats(EnvironmentRts _stats);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _unfinishedWorkshopsInRangeAndNotInDanger,
            int _totalGhouls, 
            int _idlingGhouls)
        {
            if (!GhoulAmountHandler.HasEnoughIdlingGhoulsForBuilding(_totalGhouls, _idlingGhouls))
                return false;

            if (_unfinishedWorkshopsInRangeAndNotInDanger <= 0)
                return false;

            return true;
        }
    }
}
