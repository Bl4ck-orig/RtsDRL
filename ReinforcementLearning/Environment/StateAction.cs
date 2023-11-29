
namespace ReinforcementLearning
{
    public readonly struct StateAction<T>
    {
        public readonly T State;
        public readonly T Action;

        public StateAction(T state, T action)
        {
            State = state;
            Action = action;
        }
    }
}
