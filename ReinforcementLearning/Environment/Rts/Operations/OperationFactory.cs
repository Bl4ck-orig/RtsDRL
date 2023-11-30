using System;

namespace ReinforcementLearning
{
    public static class OperationFactory
    {
        public static EnemyOperation CreateOperation(EEnemyOperation _operationType)
        {
            switch (_operationType)
            {
                case EEnemyOperation.None:
                    return new NoOperation();
                case EEnemyOperation.BuildTribe:
                    return new BuildTribeOperation();
                case EEnemyOperation.BuildStaffWorkshop:
                    return new BuildStaffWorkshopOperation();
                case EEnemyOperation.BuildSwordWorkshop:
                    return new BuildSwordWorkshopOperation();
                case EEnemyOperation.ProduceStaffWeapon:
                    return new ProduceStaffOperation();
                case EEnemyOperation.ProduceSwordWeapon:
                    return new ProduceSwordOperation();
                case EEnemyOperation.Attack:
                    return new AttackOperation();
                case EEnemyOperation.FeedGhouls:
                    return new FeedGhoulsOperation();
                case EEnemyOperation.AttackChurch:
                    return new AttackChurchOperation();
                case EEnemyOperation.Defend:
                    return new DefendOperation();
                case EEnemyOperation.PickupWeapons:
                    return new PickupWeaponsOperation();
                case EEnemyOperation.SendGhoulsToTribeDefensive:
                    return new SendGhoulsToTribeDefensiveOperation();
                case EEnemyOperation.LeaveBuilding:
                    return new LeaveBuildingOperation();
                default:
                    throw new ArgumentException("Operation not registered");
            }
        }

        public static EnemyOperation CreateOperation(EEnemyOperation _operationType, 
            IEnemyAiOperationDataChangedHandler _dataChangedHandler, IEnemyOperationGhoulAmountHandler _ghoulAmountHandler)
        {
            switch (_operationType)
            {
                case EEnemyOperation.None:
                    return new NoOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.BuildTribe:
                    return new BuildTribeOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.BuildSwordWorkshop:
                    return new BuildSwordWorkshopOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.BuildStaffWorkshop:
                    return new BuildStaffWorkshopOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.ProduceStaffWeapon:
                    return new ProduceStaffOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.ProduceSwordWeapon:
                    return new ProduceSwordOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.Attack:
                    return new AttackOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.FeedGhouls:
                    return new FeedGhoulsOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.AttackChurch:
                    return new AttackChurchOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.Defend:
                    return new DefendOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.PickupWeapons:
                    return new PickupWeaponsOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.SendGhoulsToTribeDefensive:
                    return new SendGhoulsToTribeDefensiveOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.LeaveBuilding:
                    return new LeaveBuildingOperation(_dataChangedHandler, _ghoulAmountHandler);
                case EEnemyOperation.BalanceGhoulsPerTribes:
                    return new BalanceGhoulsPerTribesOperation(_dataChangedHandler, _ghoulAmountHandler);
                default:
                    throw new ArgumentException("Operation not registered");
            }
        }
    }
}
