using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace ReinforcementLearning
{
    public abstract class Environment<T>
    {
        public abstract int ObservationSpaceSize { get; }
        public abstract int ActionSpaceSize { get; }
        public abstract T State { get; protected set; }
        protected abstract List<T> InitialStates { get; set; }
        protected Random prng { get; set; }
        protected int StepsCount { get; private set; } = 0;

        private int timeStepLimit;
        private bool hasTimeStepLimit;
        private int currentInitialState;

        public virtual T Reset(bool _hasTimeStepLimit, bool _initialStateRandom, int _seed = -1, int _timeStepLimit = 0)
        {
            prng = _seed == -1 ? new Random() : new Random(_seed);
            return Reset(_hasTimeStepLimit, _initialStateRandom, _timeStepLimit);
        }

        public virtual T Reset(bool _hasTimeStepLimit, bool _initialStateRandom, Random _prng, int _timeStepLimit = 0)
        {
            prng = _prng;
            return Reset(_hasTimeStepLimit, _initialStateRandom, _timeStepLimit);
        }

        protected virtual T Reset(bool _hasTimeStepLimit, bool _initialStateRandom, int _timeStepLimit = 0)
        {
            timeStepLimit = _timeStepLimit;
            hasTimeStepLimit = _hasTimeStepLimit;
            StepsCount = 0;
            if (_initialStateRandom)
                return State = InitialStates[prng.Next(InitialStates.Count)];
            else
                return State = InitialStates[currentInitialState++ % InitialStates.Count];
        }

        public StepResult<T> Step(int _action)
        {
            var actResult = Act(_action);
            State = actResult.NextState;
            StepsCount++;
            bool isTruncated = !actResult.IsTerminal && (hasTimeStepLimit && StepsCount >= timeStepLimit);
            return new StepResult<T>(actResult.NextState, actResult.Reward, actResult.IsTerminal, isTruncated);
        }

        protected abstract (T NextState, double Reward, bool IsTerminal) Act(int _action);

    }
}
