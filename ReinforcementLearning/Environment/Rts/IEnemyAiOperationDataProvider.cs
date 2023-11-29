namespace ReinforcementLearning
{
    public interface IEnemyAiOperationDataChangedHandler
    {
        void WorkshopHasBeenAssigned(int _enterable);

        void FoodHasBeenAssigned(int _food);

        void WeaponHasBeenAssigned(int _weapon);

        void BuildingHasBeenPlaced();
    }
}
