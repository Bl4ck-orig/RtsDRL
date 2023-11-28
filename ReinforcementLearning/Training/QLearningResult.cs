using System;
using System.Collections.Generic;

namespace ReinforcementLearning
{
    public readonly struct QLearningResult
    {
        public readonly double[,] QTable;
        public readonly double[] StateValueFunction;
        public readonly Dictionary<int, int> Policy;

        public QLearningResult(double[,] qTable, double[] stateValueFunction, Dictionary<int, int> policy)
        {
            QTable = qTable;
            StateValueFunction = stateValueFunction;
            Policy = policy;
        }
    }
}
