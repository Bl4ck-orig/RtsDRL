namespace ReinforcementLearning
{
    public class EnemyOperationPredictionResult
    {
        public EEnemyOperation ChosenOperation { get; private set; }

        public int OperationAmount { get; private set; }

        public EnemyOperationPredictionResult(EEnemyOperation chosenOperation, int operationAmount)
        {
            ChosenOperation = chosenOperation;
            OperationAmount = operationAmount;
        }
    }
}
