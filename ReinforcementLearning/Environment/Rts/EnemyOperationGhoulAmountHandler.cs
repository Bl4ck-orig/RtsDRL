using System;

namespace ReinforcementLearning
{
    public class EnemyOperationGhoulAmountHandler : IEnemyOperationGhoulAmountHandler
    {
        private int minAmountOfGhoulsForTribeTakeOver;
        private int minAmountOfGhoulsForDefensiveTribeTakeOver;
        private int minAmountOfGhoulsForAttack;

        private float amountOfGhoulsForBuildingPercent = 0.5f;
        private float amountOfGhoulsForAttackPercent = 0.5f;
        private float amountOfGhoulsForPickupWeaponsPercent = 0.5f;
        private float amountOfGhoulsForTribeTakeoverPercent = 0.5f;

        public EnemyOperationGhoulAmountHandler(RtsConfig _config)
        {
            amountOfGhoulsForBuildingPercent = _config.AmountOfGhoulsForBuildingPercent;
            amountOfGhoulsForAttackPercent = _config.AmountOfGhoulsForAttackPercent;
            amountOfGhoulsForPickupWeaponsPercent = _config.AmountOfGhoulsForPickupWeaponsPercent;
            amountOfGhoulsForTribeTakeoverPercent = _config.AmountOfGhoulsForTribeTakeoverPercent;
            minAmountOfGhoulsForTribeTakeOver = _config.MinAmountOfGhoulsForTribeTakeOver;
            minAmountOfGhoulsForDefensiveTribeTakeOver = _config.MinAmountOfGhoulsForDefensiveTribeTakeOver;
            minAmountOfGhoulsForAttack = _config.MinAmountOfGhoulsForAttack;
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
