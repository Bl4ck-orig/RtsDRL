namespace ReinforcementLearning
{
    public readonly struct StepResult<T>
    {
        public readonly T NextState;
        public readonly double Reward;
        public readonly bool Done;
        public readonly bool IsTruncated;

        public StepResult(T nextState, double reward, bool done, bool isTruncated)
        {
            NextState = nextState;
            Reward = reward;
            Done = done;
            IsTruncated = isTruncated;
        }
    }
}
