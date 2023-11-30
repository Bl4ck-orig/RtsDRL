namespace ReinforcementLearning
{
    public class EnemyVariable
    {
        public double Value { get; set; }
        public double Reward { get; set; }

        public EnemyVariable(double value, double reward)
        {
            Value = value;
            Reward = reward;
        }
    }
}
