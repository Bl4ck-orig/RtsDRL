
namespace ReinforcementLearning
{
    public class DefendOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.Defend; }

        public DefendOperation()
        {
        }

        public DefendOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            GhoulsAttackApplicationValues ghoulsAttackApplicationValues = new GhoulsAttackApplicationValues(
                GhoulAmountHandler.GetAmountOfGhoulsForAttack((int)_stats.Variables[EEnemyInput.IdlingGhouls].Value),
                (int)_stats.Variables[EEnemyInput.GhoulsWithWeapons].Value);

            _stats.Variables[EEnemyInput.IdlingGhouls].Value -= ghoulsAttackApplicationValues.GhoulsWithoutWeaponForAttack;
            _stats.Variables[EEnemyInput.IdlingGhoulsWithWeapon].Value -= ghoulsAttackApplicationValues.GhoulsWithWeaponForAttack;
            _stats.Variables[EEnemyInput.DefendingGhouls].Value += ghoulsAttackApplicationValues.GhoulsWithoutWeaponForAttack;
            _stats.Variables[EEnemyInput.DefendingGhoulsWithWeapons].Value += ghoulsAttackApplicationValues.GhoulsWithWeaponForAttack;
            _stats.Variables[EEnemyInput.GhoulsInDanger].Value--;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.Variables[EEnemyInput.GhoulsInDanger].Value, 
                (int)_stats.Variables[EEnemyInput.IdlingGhouls].Value);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _ghoulsInDanger, int _idlingGhouls)
        {
            if (_ghoulsInDanger == 0)
                return false;

            if (_idlingGhouls == 0)
                return false;

            return true;
        }
    }
}
