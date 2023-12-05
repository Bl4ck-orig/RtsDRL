namespace ReinforcementLearning
{
    public class ProduceStaffOperation : ProduceWeaponOperation
    {
        public ProduceStaffOperation()
        {
        }

        public ProduceStaffOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler, IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        public override EEnemyOperation OperationType => EEnemyOperation.ProduceStaffWeapon;

        protected override EWeapon ProductionType => EWeapon.Staff; 

        protected override int GetUnusedWorkshopsInRangeAndNotInDangerOfStats(EnvironmentRts _stats)
        {
            return (int)_stats.Variables[EEnemyInput.UsedWorkshopsInRangeAndNotInDanger].Value;
        }
    }
}
