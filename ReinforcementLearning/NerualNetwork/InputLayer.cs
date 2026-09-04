using System;

namespace ReinforcementLearning
{
    [System.Serializable]
    public class InputLayer : NeuralLayer
    {
        // The observations are raw counts that run up to roughly MAX_GHOULS / MAX_WEAPONS.
        // Unscaled they make the gradient of the first layer one to two orders of magnitude
        // larger than that of the later layers.
#if NORMALISE_INPUTS
        private const double INPUT_SCALE = 1.0 / 40.0;
#else
        private const double INPUT_SCALE = 1.0;
#endif

        public InputLayer(int _dimensionSize, int _batchSize) : base(null, _dimensionSize, _batchSize)
        {

        }

        protected override void ApplyActivationFunction()
        {

        }

        public override double[] ForwardVector(double[] _input)
        {
            double[] scaled = new double[_input.Length];

            for (int x = 0; x < _input.Length; x++)
                scaled[x] = _input[x] * INPUT_SCALE;

            return scaled;
        }

        public void SetInputs(double[,] _inputs)
        {
            if (_inputs.GetLength(0) != layerValues.GetLength(0))
                throw new ArgumentException();

            if (_inputs.GetLength(1) != layerValues.GetLength(1))
                throw new ArgumentException();

            layerValues = new double[_inputs.GetLength(0), _inputs.GetLength(1)];

            for (int x = 0; x < _inputs.GetLength(0); x++)
                for (int y = 0; y < _inputs.GetLength(1); y++)
                    layerValues[x, y] = _inputs[x, y] * INPUT_SCALE;
        }

        public void SetInputs(double[] _inputs)
        {
            for (int x = 0; x < layerValues.GetLength(0); x++)
            {
                for (int y = 0; y < layerValues.GetLength(1); y++)
                {
                    layerValues[x, y] = _inputs[x] * INPUT_SCALE;
                }
            }
        }
    }
}
