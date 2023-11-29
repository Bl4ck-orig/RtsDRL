using System;
using System.Collections.Generic;

namespace ReinforcementLearning
{
    public abstract class Environment<T>
    {

        public abstract int ObservationSpaceSize { get; }
        public abstract int ActionSpaceSize { get; }
        protected abstract List<T> ObservationSpace { get; set; }
        protected abstract List<T> ActionSpace  { get; set; }
        public abstract T State { get; protected set; }
        protected abstract List<T> InitialStates { get; set; }
        protected abstract List<T> TerminalStates { get; set; }
        protected abstract Dictionary<StateAction<T>, StepAction<T>> P { get; set; } 
        protected abstract int TimeStepLimit { get; }


        private Random prng = new Random();
        private int stepsCount = 0;

        public T Reset()
        {
            stepsCount = 0;
            return State = InitialStates[prng.Next(InitialStates.Count)];
        }

        public StepResult<T> Step(int _action)
        {
            (T, double) actResult = P[new StateAction<T>(State, _action)].Act(prng);
            State = actResult.Item1;
            bool done = TerminalStates.Contains(actResult.Item1);
            stepsCount++;
            bool isTruncated = !done && stepsCount >= TimeStepLimit;
            return new StepResult<T>(actResult.Item1, actResult.Item2, done, isTruncated);
        }

    }
}
