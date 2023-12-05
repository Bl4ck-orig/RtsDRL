using System;

namespace ReinforcementLearning
{
    public class PickupWeaponsOperation : EnemyOperation
    {
        public override EEnemyOperation OperationType { get => EEnemyOperation.PickupWeapons; }

        public PickupWeaponsOperation()
        {
        }

        public PickupWeaponsOperation(IEnemyAiOperationDataChangedHandler _dataChangedHandler,
            IEnemyOperationGhoulAmountHandler _ghoulAmountHandler) : base(_dataChangedHandler, _ghoulAmountHandler)
        {
        }

        #region Simplified -----------------------------------------------------------------
        public override void ApplySimplifiedOperation(EnvironmentRts _stats)
        {
            int weaponsToAssign = Math.Min((int)_stats.Variables[EEnemyInput.IdlingGhouls].Value, 
                (int)_stats.Variables[EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger].Value);

            int ghoulsAmountToPickUpWeapon = GhoulAmountHandler.GetAmountOfGhoulsToPickupWeapon(weaponsToAssign);
            _stats.Variables[EEnemyInput.GhoulsWithWeapons].Value += ghoulsAmountToPickUpWeapon;
            _stats.Variables[EEnemyInput.IdlingGhoulsWithWeapon].Value += ghoulsAmountToPickUpWeapon;
            _stats.Variables[EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger].Value -= ghoulsAmountToPickUpWeapon;
        }

        public override bool IsSimplifiedOperationPossible(EnvironmentRts _stats) =>
            IsOperationPossible((int)_stats.Variables[EEnemyInput.IdlingGhouls].Value, 
                (int)_stats.Variables[EEnemyInput.IdlingGhoulsWithWeapon].Value, 
                (int)_stats.Variables[EEnemyInput.UnassignedWeaponsInRangeAndNotInDanger].Value);
        #endregion -----------------------------------------------------------------

        private bool IsOperationPossible(int _idlingGhouls, int _idlingGhoulsWithWeapon, int _unassignedWeaponsInRangeAndNotInDanger)
        {
            if (_idlingGhouls - _idlingGhoulsWithWeapon == 0)
                return false;

            if (_unassignedWeaponsInRangeAndNotInDanger == 0)
                return false;

            return true;
        }
    }
}
