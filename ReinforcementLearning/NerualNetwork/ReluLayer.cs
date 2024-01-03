using System;

namespace ReinforcementLearning
{
    [System.Serializable]
    public class ReluLayer : NeuralLayer
    {
        public ReluLayer(NeuralLayer _layerBefore, int _dimensionSize, int _batchSize) : base(_layerBefore, _dimensionSize, _batchSize)
        {

        }

        protected override void ApplyActivationFunction()
        {
            for (int x = 0; x < layerValues.GetLength(0); x++)
            {
                for (int y = 0; y < layerValues.GetLength(1); y++)
                {
                    layerValues[x, y] = Math.Max(0, layerValuesZ[x, y]);
                }
            }
        }

        public void BackwardPropagate(NeuralLayer _layerAfter)
        {
            changeInLayerError = _layerAfter.GetWeightsUnsafe().Transpose().DotProduct(_layerAfter.GetChangeInLayerErrorUnsafe());

            changeInLayerError = changeInLayerError.Multiply(DerivedReluHiddenlayer());

            BackPropagateValuesFromLayerBefore();
        }

        private double[,] DerivedReluHiddenlayer()
        {
            int width = changeInLayerError.GetLength(0);
            int height = changeInLayerError.GetLength(1);

            double[,] derivedReluHiddenlayer = new double[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    derivedReluHiddenlayer[x, y] = ReluDerivative(layerValuesZ[x, y]);
                }
            }
            return derivedReluHiddenlayer;
        }

        private double ReluDerivative(double _value) => _value < 0 ? 0f : 1f;
    }
}
