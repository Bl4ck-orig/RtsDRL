namespace ReinforcementLearning
{
    public interface IEnemyOperationGhoulAmountHandler
    {
        bool HasEnoughIdlingGhoulsForBuilding(int _totalGhouls, int _idlingGhouls);

        bool HasEnoughIdlingGhoulsForAttack(int _totalGhouls, int _idlingGhouls);

        bool HasEnoughIdlingGhoulsForTribeTakeOver(int _totalGhouls, int _idlingGhouls);

        bool HasEnoughIdlingGhoulsForDefensiveTribeTakeOver(int _totalGhouls, int _idlingGhouls);


        int GetAmountOfGhoulsForBuilding(int _idlingGhouls);

        int GetAmountOfGhoulsForAttack(int _idlingGhouls);

        int GetAmountOfGhoulsToPickupWeapon(int _idlingGhouls);

        int GetAmountOfGhoulsForTribeTakeOver(int _idlingGhouls);
    }
}
