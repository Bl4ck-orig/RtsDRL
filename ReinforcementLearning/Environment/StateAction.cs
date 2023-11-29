
namespace ReinforcementLearning
{
    public readonly struct StateAction<T>
    {
        public readonly T State;
        public readonly int Action;

        public StateAction(T state, int action)
        {
            State = state;
            Action = action;
        }
    }
}
