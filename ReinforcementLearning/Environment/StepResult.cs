namespace ReinforcementLearning
{
    public struct StepResult
    {
        public int NextState { get; private set; }
        public double Reward { get; private set; }
        public bool Done { get; set; }

        public StepResult(int nextState, double reward, bool done)
        {
            NextState = nextState;
            Reward = reward;
            Done = done;
        }
    }
}
