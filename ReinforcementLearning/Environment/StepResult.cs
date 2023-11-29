namespace ReinforcementLearning
{
    public struct StepResult<T>
    {
        public T NextState { get; private set; }
        public double Reward { get; private set; }
        public bool Done { get; set; }

        public StepResult(T nextState, double reward, bool done)
        {
            NextState = nextState;
            Reward = reward;
            Done = done;
        }
    }
}
