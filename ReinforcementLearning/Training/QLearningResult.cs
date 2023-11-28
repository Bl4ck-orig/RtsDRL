using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public override string ToString()
        {
            string qTableString = "QTable:\n";
            string stateValueString = "StateValueFunction:\n";
            string policyString = "Policy:\n";

            for (int x = 0; x < QTable.GetLength(0); x++)
            {
                for (int y = 0; y < QTable.GetLength(1); y++)
                {
                    qTableString += Math.Round(QTable[x, y], 3);

                    if (y == QTable.GetLength(1) - 1)
                        continue;

                    qTableString += "\t";
                }

                stateValueString += Math.Round(StateValueFunction[x], 3);
                policyString += Policy.Skip(x).First();

                qTableString += "\n";
                stateValueString += "\n";
                policyString += "\n";
            }

            return qTableString + "\n" + stateValueString + "\n" + policyString;
        }
    }
}
