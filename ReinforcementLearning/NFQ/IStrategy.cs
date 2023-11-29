using System;

namespace ReinforcementLearning
{
    public interface IStrategy
    {
        bool ExploratoryActionTaken { get; }

        int SelectAction(double[] _state, IFcq _model, Random _prng);
    }
}
