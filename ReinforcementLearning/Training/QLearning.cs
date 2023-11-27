using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReinforcementLearning.Training
{
    public class QLearning
    {
        private Random random = new Random();

        public QLearning(QLearningArgs _args)
        {
            int nS = _args.Environment.ObservationSpace.Count;
            int nA = _args.Environment.ActionSpace.Count;
            double[,] Q = CreateQTable(nA, nS);



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

                bool done = false;
            }
        }

        public double[,] CreateQTable(int _actionSpaceSize, int _observationsSpaceSize)
        {
            return new double[_actionSpaceSize, _observationsSpaceSize];
        }

        public int SelectAction(int _state, double[,] _qTable, double _epsilon)
        {
            if (random.NextDouble() > _epsilon)
            {
                return Commons.ArgMax(_qTable, _state);
            }
            else
            {
                return random.Next(_qTable.GetLength(1));
            }
        }
    }
}
