using System;
using System.Collections.Generic;

namespace ReinforcementLearning
{
    public abstract class Environment<T>
    {
        public abstract int ObservationSpaceSize { get; }
        public abstract int ActionSpaceSize { get; }
        public abstract T State { get; protected set; }
        protected abstract List<T> InitialStates { get; set; }
        protected int StepsCount { get; private set; } = 0;

        private int timeStepLimit;
        private bool hasTimeStepLimit;

        public virtual T Reset(bool _hasTimeStepLimit, Random _prng, int _timeStepLimit = 0)
        {
            timeStepLimit = _timeStepLimit;
            hasTimeStepLimit = _hasTimeStepLimit;
            StepsCount = 0;
            return State = InitialStates[_prng.Next(InitialStates.Count)];
        }

        public StepResult<T> Step(int _action, Random _prng)
        {
            var actResult = Act(_action, _prng);
            State = actResult.NextState;
            StepsCount++;
            bool isTruncated = !actResult.IsTerminal && (hasTimeStepLimit && StepsCount >= timeStepLimit);
            return new StepResult<T>(actResult.NextState, actResult.Reward, actResult.IsTerminal, isTruncated);
        }

        protected abstract (T NextState, double Reward, bool IsTerminal) Act(int _action, Random _prng);

    }
}
