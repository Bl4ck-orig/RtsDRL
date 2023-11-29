using System;
using System.Collections.Generic;

namespace ReinforcementLearning
{
    public abstract class Environment<T>
    {
        private Random prng = new Random();

        protected abstract List<T> ObservationSpace { get; set; }
        protected abstract List<T> ActionSpace  { get; set; }
        public abstract T State { get; protected set; }
        protected abstract List<T> InitialStates { get; set; }
        protected abstract List<T> TerminalStates { get; set; }
        protected abstract Dictionary<StateAction<T>, StepAction<T>> P { get; set; } 


        public int ObservationSpaceSize { get => ObservationSpace.Count; }

        public int ActionSpaceSize { get => ActionSpace.Count; }

        public T Reset()
        {
            return State = InitialStates[prng.Next(InitialStates.Count)];
        }

        public StepResult<T> Step(T _action)
        {
            (T, double) actResult = P[new StateAction<T>(State, _action)].Act(prng);
            State = actResult.Item1;
            bool done = TerminalStates.Contains(actResult.Item1);
            return new StepResult<T>(actResult.Item1, actResult.Item2, done);
        }

    }
}
