namespace ReinforcementLearning
{
    public struct Experience<T>
    {
        public readonly T State;
        public readonly int Action;
        public readonly double Reward;
        public readonly T NextState;
        public readonly double IsFailure;

        public Experience(T state, int action, double reward, T nextState, double isFailure)
        {
            State = state;
            Action = action;
            Reward = reward;
            NextState = nextState;
            IsFailure = isFailure;
        }
    }
}
