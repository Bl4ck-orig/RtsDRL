using System;
using System.Collections.Generic;

namespace ReinforcementLearning
{
    public abstract class Environment
    {
        private Random prng = new Random();

        protected abstract List<int> ObservationSpace { get; set; }
        protected abstract List<int> ActionSpace  { get; set; }
        public abstract int State { get; protected set; }
        protected abstract List<int> InitialStates { get; set; }
        protected abstract List<int> TerminalStates { get; set; }
        protected abstract Dictionary<StateAction, StepAction> P { get; set; } 


        public int ObservationSpaceSize { get => ObservationSpace.Count; }

        public int ActionSpaceSize { get => ActionSpace.Count; }

        public int Reset()
        {
            return State = InitialStates[prng.Next(InitialStates.Count)];
        }

        public StepResult Step(int _action)
        {
            (int, double) actResult = P[new StateAction(State, _action)].Act(prng);
            bool done = TerminalStates.Contains(actResult.Item1);
            return new StepResult(actResult.Item1, actResult.Item2, done);
        }

    }
}
