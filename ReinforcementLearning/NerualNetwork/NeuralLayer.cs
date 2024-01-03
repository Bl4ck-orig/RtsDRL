using System;
using System.Linq;
using System.Xml.Linq;

namespace ReinforcementLearning
{
    [System.Serializable]
    public abstract class NeuralLayer
    {
        protected int index = 0;
        protected int dimensionSize = 0;
        protected int layerBeforeSize = 0;
        protected int batchSize = 0;
        protected double oneOverBatchSize = 0f;

        protected NeuralLayer layerBefore;
        protected double[,] layerValues;
        protected double[,] layerValuesZ;
        protected double[,] weights;
        protected double[,] bias;

        protected double[,] changeInLayerError;
        protected double[,] changeInWeights;
        protected double[,] changeInBias;

        public NeuralLayer(NeuralLayer _layerBefore, int _dimensionSize, int _batchSize)
        {
            layerBefore = _layerBefore;
            dimensionSize = _dimensionSize;
            layerBeforeSize = _layerBefore == null ? 0 : layerBefore.dimensionSize;
            batchSize = _batchSize;
            oneOverBatchSize = (double)1 / _batchSize;

            layerValues = new double[dimensionSize, _batchSize];
            weights = new double[dimensionSize, layerBeforeSize];
            bias = new double[dimensionSize, 1];
        }

        public void InitializeXavier(Random _prng)
        {
            for (int x = 0; x < weights.GetLength(0); x++)
            {
                for (int y = 0; y < weights.GetLength(1); y++)
                {
                    double distribution = Math.Sqrt((double)6 / (dimensionSize + layerBeforeSize));
                    weights[x, y] = _prng.NextDouble() * 2 * distribution - distribution;
                }
            }
        }

        #region Forwads -----------------------------------------------------------------
        public void ForwardPropagate()
        {
            layerValuesZ = weights.DotProduct(layerBefore.layerValues).AddVector(bias);
            ApplyActivationFunction();
        }

        protected abstract void ApplyActivationFunction();
        #endregion -----------------------------------------------------------------

        #region Backwards -----------------------------------------------------------------
        protected void BackPropagateValuesFromLayerBefore()
        {
            BackPropagateLayerWeights();

            BackPropagateLayerBias();
        }

        private void BackPropagateLayerWeights()
        {
            changeInWeights = changeInLayerError.DotProduct(layerBefore.layerValues.Transpose());
            changeInWeights = changeInWeights.Multiply(oneOverBatchSize);
        }

        private void BackPropagateLayerBias()
        {
            changeInBias = new double[bias.GetLength(0), bias.GetLength(1)];
            for (int i = 0; i < bias.GetLength(0); i++)
                changeInBias[i, 0] = oneOverBatchSize *
                    Enumerable.Range(0, changeInLayerError.GetLength(0))
                        .Select(x => changeInLayerError[x, i]).Sum();
        }
        #endregion -----------------------------------------------------------------

        #region Getters -----------------------------------------------------------------
        public double[,] GetWeightsUnsafe() => weights;

        public double[,] GetChangeInLayerErrorUnsafe() => changeInLayerError;

        public int ChangeInHiddenLayerWeightsLength() => changeInWeights.Length;

        public int ChangeInHiddenLayerBiasLength() => changeInBias.Length;
        #endregion -----------------------------------------------------------------

        #region Weights and Bias Changing -----------------------------------------------------------------
        public double EuclideanSumOfWeightsAndBiases()
        {
            double euclideanSum = Commons.EuclideanSum(changeInWeights);
            euclideanSum += Commons.EuclideanSum(changeInBias);
            return euclideanSum;
        }

        public void ScaleChangeInWeightsAndBias(double _scale)
        {
            changeInWeights = changeInWeights.Multiply(_scale);
            changeInBias = changeInBias.Multiply(_scale);
        }

        public void AdjustWeightsAndBiases(double _learningRate)
        {
            weights = weights.Subtract(changeInWeights.Multiply(_learningRate));
            bias = bias.Subtract(changeInBias.Multiply(_learningRate));
        }

        public void ClipWeightsAndBiases(double _maxThreshold)
        {
            for (int x = 0; x < weights.GetLength(0); x++)
            {
                for(int y = 0; y < weights.GetLength(1); y++)
                {
                    double weightValue = weights[x, y];

                    if (weightValue > _maxThreshold)
                        weights[x, y] = _maxThreshold;
                    if(weightValue < -_maxThreshold)
                        weights[x, y] = -_maxThreshold;
                }
            }

            for (int x = 0; x < weights.GetLength(0); x++)
            {
                double biasValue = bias[x, 0];

                if (biasValue > _maxThreshold)
                    bias[x, 0] = _maxThreshold;
                if (biasValue < -_maxThreshold)
                    bias[x, 0] = -_maxThreshold;
            }
        }
        #endregion -----------------------------------------------------------------
    }
}
