namespace ReinforcementLearning
{
    public abstract class ProduceWeaponOperation : EnemyOperation
    {
        protected abstract EWeapon ProductionType { get; }

        public ProduceWeaponOperation()
        {
        }

        public ProduceWeaponOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            _stats.IdlingGhouls--;
            _stats.GhoulsInWorkshops++;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible(_stats, GetUnusedWorkshopsInRangeAndNotInDangerOfStats(_stats), (int)_stats.IdlingGhoulsNotHungry);

        protected abstract int GetUnusedWorkshopsInRangeAndNotInDangerOfStats(EnvironmentRts _stats);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(EnvironmentRts _stats, int _unusedWorkshopsInRangeAndNotInDanger,int _idlingGhoulsNotHungry)
        {
            if (_idlingGhoulsNotHungry == 0)
                return false;

            if (_unusedWorkshopsInRangeAndNotInDanger <= 0)
                return false;

            if (_stats.AliveWeaponInstances[ProductionType] >= EnvironmentRts.MAX_WEAPONS)
                return false;

            return true;
        }
    }
}
