using System;

namespace ReinforcementLearning
{
    [System.Serializable]
    public class InputLayer : NeuralLayer
    {
        public InputLayer(int _dimensionSize, int _batchSize) : base(null, _dimensionSize, _batchSize)
        {

        }

        protected override void ApplyActivationFunction()
        {

        }

        public void SetInputs(double[,] _inputs)
        {
            if (_inputs.GetLength(0) != layerValues.GetLength(0))
                throw new ArgumentException();

            if (_inputs.GetLength(1) != layerValues.GetLength(1))
                throw new ArgumentException();

            layerValues = _inputs.Clone() as double[,];
        }

        public void SetInputs(double[] _inputs)
        {
            for (int x = 0; x < layerValues.GetLength(0); x++)
            {
                for (int y = 0; y < layerValues.GetLength(1); y++)
                {
                    layerValues[x, y] = _inputs[x];
                }
            }
        }
    }
}
