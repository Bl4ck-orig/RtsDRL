using System;

namespace ReinforcementLearning
{
    public class EGreedyStrategy : IStrategy
    {
        public bool ExploratoryActionTaken { get; private set; }

        private double epsilon;

        public EGreedyStrategy(double _epsilon = 0.1f)
        {
            epsilon = _epsilon;
            ExploratoryActionTaken = false;
        }

        public int SelectAction(double[] _state, IFcq _model, Random _prng)
        {
            double[] prediction = _model.GetPrediction(_state);

            int action = 0;
            int argMaxAction = Commons.ArgMax(prediction);

            if (_prng.NextDouble() < epsilon)
                action = _prng.Next(prediction.Length);

            ExploratoryActionTaken = action != argMaxAction;
            return action;
        }

    }
}
