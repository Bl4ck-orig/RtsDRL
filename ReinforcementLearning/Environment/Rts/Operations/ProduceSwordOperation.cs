namespace ReinforcementLearning
{
    public class ProduceSwordOperation : ProduceWeaponOperation
    {
        public ProduceSwordOperation()
        {
        }

        public ProduceSwordOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler, IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        public override EEnemyOperation OperationType => EEnemyOperation.ProduceSwordWeapon;

        protected override EWeapon ProductionType => EWeapon.Sword;

        protected override int GetUnusedWorkshopsInRangeAndNotInDangerOfStats(EnvironmentRts _stats)
        {
            return (int)_stats.Variables[EEnemyInput.UnusedWorkshopsInRangeAndNotInDanger].Value;
        }
    }
}
