using System;

using System.Linq;

namespace ReinforcementLearning
{
    [System.Serializable]
    public class OutputLayer : NeuralLayer
    {
        public OutputLayer(NeuralLayer _layerBefore, int _dimensionSize, int _batchSize) : base(_layerBefore, _dimensionSize, _batchSize)
        {

        }

        protected override void ApplyActivationFunction()
        {
            layerValues = layerValuesZ.Clone() as double[,];
        }

        public void BackwardPropagate(double[,] _errorMatrix)
        {
            changeInLayerError = layerValues.Subtract(_errorMatrix);

            BackPropagateValuesFromLayerBefore();
        }

        #region Output -----------------------------------------------------------------
        public double[,] GetOutputMatrixCopy() => layerValues.Clone() as double[,];

        public double[] GetHighestRewardVector()
        {
            double[] maxQValues = new double[batchSize];

            for (int i = 0; i < batchSize; i++)
            {
                double maxQValue = Double.MinValue;
                for (int j = 0; j < dimensionSize; j++)
                {
                    if (layerValues[j, i] > maxQValue)
                    {
                        maxQValue = layerValues[j, i];
                    }
                }
                maxQValues[i] = maxQValue;
            }

            return maxQValues;
        }

        public double[] GetHighestRewardRow()
        {
            double maxMean = double.MinValue;
            double[] maxRow = new double[dimensionSize];

            for (int y = 0; y < layerValues.GetLength(1); y++)
            {
                double[] row = new double[dimensionSize];

                for (int x = 0; x < dimensionSize; x++)
                {
                    row[x] = layerValues[x, y];
                }

                double mean = Mean(row);
                if (mean < maxMean)
                    continue;

                maxMean = mean;
                maxRow = row;
            }

            return maxRow;
        }

        private double Mean(double[] _vector) => _vector.Sum() / _vector.Length;
        #endregion -----------------------------------------------------------------
    }
}
