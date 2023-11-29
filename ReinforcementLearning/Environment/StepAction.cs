using System;
using System.Collections.Generic;
using System.Linq;

namespace ReinforcementLearning
{
    public readonly struct StepAction<T>
    {
        private readonly List<(double, T)> transitionProbabilities;
        private readonly Dictionary<T, double> transitionRewards;

        public StepAction(List<(double, T)> transitionProbabilities, Dictionary<T, double> transitionRewards)
        {
            if (transitionProbabilities.Any(x => !transitionRewards.ContainsKey(x.Item2)))
                throw new ArgumentException("Transition probabilites and transition rewards do not match");

            if (transitionRewards.Any(x => !transitionProbabilities.Any(y => y.Item2.Equals(x.Key))))
                throw new ArgumentException("Transition probabilites and transition rewards do not match");

            this.transitionProbabilities = transitionProbabilities
                .OrderBy(x => x.Item1)
                .ToList();

            this.transitionRewards = transitionRewards;
        }

        public (T NextState, double Reward) Act(Random _prng)
        {
            T nextState = GetNextState(transitionProbabilities, _prng.NextDouble());

            return (nextState, transitionRewards[nextState]);
        }

        private static T GetNextState(List<(double, T)> transitionProbabilities, double _random)
        {
            double total = 0f;
            int transitionIndex = 0;

            for (int i = 0; i < transitionProbabilities.Count; i++)
            {
                total += transitionProbabilities[i].Item1;

                if (_random > total)
                    continue;

                transitionIndex = i;
                break;
            }

            return transitionProbabilities[transitionIndex].Item2;
        }
    }
}
