using System;

namespace ReinforcementLearning
{
    public class EGreedyStrategy : IStrategy
    {
        public bool ExploratoryActionTaken { get; private set; }

        public double Epsilon => epsilon;

        private double epsilon;
        private double minEpsilon;
        private double decayPerStep;

        public EGreedyStrategy(double _epsilon = 0.1) : this(_epsilon, _epsilon, 0)
        {
        }

        /// <summary>
        /// Explores a lot early on and settles on a low rate later. A constant rate has to be
        /// a compromise between the two, and in a sparse reward setting that compromise tends
        /// to be too low to ever see the states that pay off.
        /// </summary>
        public EGreedyStrategy(double _startEpsilon, double _minEpsilon, long _decaySteps)
        {
            epsilon = _startEpsilon;
            minEpsilon = _minEpsilon;
            decayPerStep = _decaySteps > 0 ? (_startEpsilon - _minEpsilon) / _decaySteps : 0.0;
            ExploratoryActionTaken = false;
        }

        public int SelectAction(double[] _state, IFcq _model, Random _prng)
        {
            double[] prediction = _model.GetPrediction(_state);

            int argMaxAction = Commons.ArgMax(prediction);
            int action = argMaxAction;

            if (_prng.NextDouble() < epsilon)
                action = _prng.Next(prediction.Length);

            if (epsilon > minEpsilon)
                epsilon = Math.Max(minEpsilon, epsilon - decayPerStep);

            ExploratoryActionTaken = action != argMaxAction;
            return action;
        }
    }
}
