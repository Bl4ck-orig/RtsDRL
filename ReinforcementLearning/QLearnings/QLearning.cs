using System;
using System.Collections.Generic;

namespace ReinforcementLearning.Training
{
    public class QLearning
    {
        private static Random random = new Random();

        public static QLearningResult Learn(QLearningArgs _args)
        {
            int nS = _args.Environment.ObservationSpaceSize;
            int nA = _args.Environment.ActionSpaceSize;
            double[,] qTable = CreateQTable(nS, nA);

            var alphas = Exploration.DecaySchedule(_args.InitAlpha, 
                _args.MinAlpha, 
                _args.AlphaDecayRatio, 
                _args.NEpisodes);

            var epsilons = Exploration.DecaySchedule(_args.InitEpsilon, 
                _args.MinEpsilon, 
                _args.EpsilonDecayRatio, 
                _args.NEpisodes);

            for(int e = 0; e < _args.NEpisodes; e++)
            {
                Dialogue.PrintProgress(e, _args.NEpisodes, e == 0);

                int state = _args.Environment.Reset();
                bool done = false;
                while (!done)
                {
                    int action = SelectAction(random, state, qTable, epsilons[e]);
                    StepResult<int> stepResult = _args.Environment.Step(action);
                    done = stepResult.Done;
                    double tdTarget = stepResult.Reward + _args.Gamma * Commons.GetMaxValueOfRow(qTable, stepResult.NextState);
                    if(done)
                        tdTarget = 0;
                    double tdError = tdTarget - qTable[state, action];
                    qTable[state, action] = qTable[state, action] + alphas[e] * tdError;
                    state = stepResult.NextState;
                }
            }

            double[] stateValueFunction = Commons.FlattenMax(qTable);

            Dictionary<int, int> pi = GetPolicy(qTable);

            return new QLearningResult(qTable, stateValueFunction, pi);
        }

        public static double[,] CreateQTable(int _observationsSpaceSize, int _actionSpaceSize)
        {
            return new double[_observationsSpaceSize, _actionSpaceSize];
        }

        

        public static int SelectAction(Random _random, int _state, double[,] _qTable, double _epsilon)
        {
            if (_random.NextDouble() > _epsilon)
            {
                return Commons.ArgMax(_qTable, _state);
            }
            else
            {
                return _random.Next(_qTable.GetLength(1));
            }
        }

        private static Dictionary<int, int> GetPolicy(double[,] _qTable)
        {
            Dictionary<int, int> policy = new Dictionary<int, int>();
            for (int s = 0; s < _qTable.GetLength(0); s++)
            {
                policy[s] = Commons.ArgMax(_qTable, s);
            }
            return policy;
        }

    }
}
