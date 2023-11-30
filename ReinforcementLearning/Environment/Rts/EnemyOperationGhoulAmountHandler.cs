using System;

namespace ReinforcementLearning
{
    public class EnemyOperationGhoulAmountHandler : IEnemyOperationGhoulAmountHandler
    {
        private int minAmountOfGhoulsForTribeTakeOver;
        private int minAmountOfGhoulsForDefensiveTribeTakeOver;
        private int minAmountOfGhoulsForAttack;

        private double amountOfGhoulsForBuildingPercent = 0.5f;
        private double amountOfGhoulsForAttackPercent = 0.5f;
        private double amountOfGhoulsForPickupWeaponsPercent = 0.5f;
        private double amountOfGhoulsForTribeTakeoverPercent = 0.5f;

        public EnemyOperationGhoulAmountHandler()
        {
            amountOfGhoulsForBuildingPercent = EnvironmentRts.AMOUNT_OF_GHOULS_FOR_BUILDING_PERCENT;
            amountOfGhoulsForAttackPercent = EnvironmentRts.AMOUNT_OF_GHOULS_FOR_ATTACK_PERCENT;
            amountOfGhoulsForPickupWeaponsPercent = EnvironmentRts.AMOUNT_OF_GHOULS_FOR_PICKUP_WEAPONS_PERCENT;
            amountOfGhoulsForTribeTakeoverPercent = EnvironmentRts.AMOUNT_OF_GHOULS_FOR_TRIBE_TAKEOVER_PERCENT;
            minAmountOfGhoulsForTribeTakeOver = EnvironmentRts.MIN_AMOUNT_OF_GHOULS_FOR_TRIBE_TAKE_OVER;
            minAmountOfGhoulsForDefensiveTribeTakeOver = EnvironmentRts.MIN_AMOUNT_OF_GHOULS_FOR_DEFENSIVE_TRIBE_TAKE_OVER;
            minAmountOfGhoulsForAttack = EnvironmentRts.MIN_AMOUNT_OF_GHOULS_FOR_ATTACK;
        }

        public bool HasEnoughIdlingGhoulsForBuilding(int _totalGhouls, int _idlingGhouls) =>
            _idlingGhouls != 0 && _idlingGhouls > (int)Math.Ceiling(_totalGhouls * amountOfGhoulsForBuildingPercent);

        public bool HasEnoughIdlingGhoulsForAttack(int _totalGhouls, int _idlingGhouls) =>
            _idlingGhouls > minAmountOfGhoulsForAttack && _idlingGhouls > (int)Math.Ceiling(_totalGhouls * amountOfGhoulsForAttackPercent);

        public bool HasEnoughIdlingGhoulsForTribeTakeOver(int _totalGhouls, int _idlingGhouls) =>
            _idlingGhouls > minAmountOfGhoulsForTribeTakeOver && _idlingGhouls > (int)Math.Ceiling(_totalGhouls * amountOfGhoulsForTribeTakeoverPercent);

        public bool HasEnoughIdlingGhoulsForDefensiveTribeTakeOver(int _totalGhouls, int _idlingGhouls) =>
             _idlingGhouls > minAmountOfGhoulsForDefensiveTribeTakeOver && _idlingGhouls > (int)Math.Ceiling(_totalGhouls * amountOfGhoulsForTribeTakeoverPercent);


        public int GetAmountOfGhoulsForBuilding(int _idlingGhouls) =>
            (_idlingGhouls == 0) ? 0 : (int)Math.Ceiling(_idlingGhouls * amountOfGhoulsForBuildingPercent);

        public int GetAmountOfGhoulsForAttack(int _idlingGhouls) =>
            (_idlingGhouls == 0) ? 0 : (int)Math.Ceiling(_idlingGhouls * amountOfGhoulsForAttackPercent);

        public int GetAmountOfGhoulsToPickupWeapon(int _idlingGhouls) =>
            (_idlingGhouls == 0) ? 0 : (int)Math.Ceiling(_idlingGhouls * amountOfGhoulsForPickupWeaponsPercent);

        public int GetAmountOfGhoulsForTribeTakeOver(int _idlingGhouls) =>
            (_idlingGhouls == 0) ? 0 : (int)Math.Ceiling(_idlingGhouls * amountOfGhoulsForTribeTakeoverPercent);
    }
}
