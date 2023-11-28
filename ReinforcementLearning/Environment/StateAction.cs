
namespace ReinforcementLearning
{
    public readonly struct StateAction
    {
        public readonly int State;
        public readonly int Action;

        public StateAction(int state, int action)
        {
            State = state;
            Action = action;
        }
    }
}
