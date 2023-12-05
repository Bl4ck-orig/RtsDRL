namespace ReinforcementLearning
{
    public class AttackOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.Attack; }

        public AttackOperation()
        {
        }

        public AttackOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            GhoulsAttackApplicationValues ghoulsAttackApplicationValues = new GhoulsAttackApplicationValues(
                GhoulAmountHandler.GetAmountOfGhoulsForAttack((int)_stats.Variables[EEnemyInput.IdlingGhouls].Value),
                (int)_stats.Variables[EEnemyInput.GhoulsWithWeapons].Value);

            int totalGhouls = ghoulsAttackApplicationValues.GhoulsWithWeaponForAttack +
                ghoulsAttackApplicationValues.GhoulsWithoutWeaponForAttack;
            _stats.Variables[EEnemyInput.IdlingGhouls].Value -= totalGhouls;
            _stats.Variables[EEnemyInput.IdlingGhoulsNotHungry].Value -= totalGhouls;
            _stats.Variables[EEnemyInput.IdlingGhoulsWithWeapon].Value -= ghoulsAttackApplicationValues.GhoulsWithWeaponForAttack;
            _stats.Variables[EEnemyInput.AttackingGhouls].Value += ghoulsAttackApplicationValues.GhoulsWithoutWeaponForAttack;
            _stats.Variables[EEnemyInput.AttackingGhoulsWithWeapons].Value += ghoulsAttackApplicationValues.GhoulsWithWeaponForAttack;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.Variables[EEnemyInput.TotalGhouls].Value, 
                (int)_stats.Variables[EEnemyInput.IdlingGhouls].Value);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _totalGhouls, int _idlingGhouls)
        {
            if (!GhoulAmountHandler.HasEnoughIdlingGhoulsForAttack(_totalGhouls, _idlingGhouls))
                return false;

            return true;
        }
    }
}
