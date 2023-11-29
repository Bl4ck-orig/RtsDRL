using System;

namespace ReinforcementLearning
{
    public class GreedyStrategy : IStrategy
    {
        public bool ExploratoryActionTaken { get; } = false;

        public GreedyStrategy()
        {
        }

        public int SelectAction(double[] _state, IFcq _model, Random _prng)
        {
            double[] prediction = _model.GetPrediction(_state);
            return Commons.ArgMax(prediction);
        }

    }
}
