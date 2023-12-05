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
            _stats.Variables[EEnemyInput.IdlingGhouls].Value--;
            _stats.Variables[EEnemyInput.IdlingGhoulsNotHungry].Value--;
            _stats.Variables[EEnemyInput.GhoulsInWorkshops].Value++;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible(GetUnusedWorkshopsInRangeAndNotInDangerOfStats(_stats), 
                (int)_stats.Variables[EEnemyInput.IdlingGhoulsNotHungry].Value);

        protected abstract int GetUnusedWorkshopsInRangeAndNotInDangerOfStats(EnvironmentRts _stats);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _unusedWorkshopsInRangeAndNotInDanger,int _idlingGhoulsNotHungry)
        {
            if (_idlingGhoulsNotHungry == 0)
                return false;

            if (_unusedWorkshopsInRangeAndNotInDanger <= 0)
                return false;

            return true;
        }
    }
}
