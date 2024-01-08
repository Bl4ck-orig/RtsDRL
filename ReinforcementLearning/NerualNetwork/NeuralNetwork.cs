using System;

namespace ReinforcementLearning
{
    public class NeuralNetwork : IFcq
    {
        private int inputSize;
        private int outputSize;
        private int batchSize;
        private double minZeroConvergeThreshold;
        private int hiddenLayerNodesAmount;

        private InputLayer inputLayer;
        private ReluLayer[] hiddenLayers;
        private OutputLayer outputLayer;

        private double gradientClippingThreshold;

        public NeuralNetwork(int _inputSize, 
            int _hiddenLayerNodesAmount, 
            int _hiddenLayersAmount, 
            int _outputSize,
            int _batchSize,
            double _gradientClippingThreshold, 
            double _minZeroConvergeThreshold,
            Random _prng)
        {
            inputSize = _inputSize;
            outputSize = _outputSize;
            batchSize = _batchSize;
            minZeroConvergeThreshold = _minZeroConvergeThreshold;

            hiddenLayerNodesAmount = _hiddenLayerNodesAmount;

            inputLayer = new InputLayer(inputSize, _batchSize);
            
            hiddenLayers = new ReluLayer[_hiddenLayersAmount];
            hiddenLayers[0] = new ReluLayer(inputLayer, hiddenLayerNodesAmount, _batchSize);
            for (int i = 1; i < _hiddenLayersAmount; i++)
                hiddenLayers[i] = new ReluLayer(hiddenLayers[i - 1], hiddenLayerNodesAmount, _batchSize);

            outputLayer = new OutputLayer(hiddenLayers[hiddenLayers.Length - 1], outputSize, _batchSize);

            inputLayer.InitializeXavier(_prng);
            foreach (var hiddenLayer in hiddenLayers)
                hiddenLayer.InitializeXavier(_prng);
            outputLayer.InitializeXavier(_prng);

            gradientClippingThreshold = _gradientClippingThreshold;
        }

        public NeuralNetwork(TrainingResult _trainingResult)
        {
             inputSize = _trainingResult.inputSize;
             outputSize = _trainingResult.outputSize;
             batchSize = _trainingResult.batchSize;
             hiddenLayerNodesAmount = _trainingResult.hiddenLayerNodesAmount;
             inputLayer = _trainingResult.inputLayer;
             hiddenLayers = _trainingResult.hiddenLayer;
             outputLayer = _trainingResult.outputLayer;
             gradientClippingThreshold = _trainingResult.gradientClippingThreshold;
        }

        #region Forward Propagation -----------------------------------------------------------------
        public void ApplyForwardPropagation()
        {
            foreach (var layer in hiddenLayers)
                layer.ForwardPropagate();
            outputLayer.ForwardPropagate();
        }
        #endregion -----------------------------------------------------------------

        #region Getting Prediction -----------------------------------------------------------------
        public double[,] GetOutputMatrix(double[,] _inputs)
        {
            inputLayer.SetInputs(_inputs);
            ApplyForwardPropagation();
            return outputLayer.GetOutputMatrixCopy();
        }

        public double[] GetHighestRewardVector(double[,] _inputs)
        {
            inputLayer.SetInputs(_inputs);
            ApplyForwardPropagation();
            return outputLayer.GetHighestRewardVector();
        }

        public double[] GetPrediction(double[] _inputs)
        {
            inputLayer.SetInputs(_inputs);
            ApplyForwardPropagation();
            return outputLayer.GetHighestRewardRow();
        }
        #endregion -----------------------------------------------------------------

        #region Training -----------------------------------------------------------------
        public void Backwards(double[,] _errorMatrix)
        {
            outputLayer.BackwardPropagate(_errorMatrix);

            hiddenLayers[hiddenLayers.Length - 1].BackwardPropagate(outputLayer);
            
            for(int i = hiddenLayers.Length - 2; i >= 0; i--)
                hiddenLayers[i].BackwardPropagate(hiddenLayers[i + 1]);
        }

        public double AdjustWeightsAndBiases(double _learningRate)
        {
            double originalMagnitude = GradientClipping();

            outputLayer.AdjustWeightsAndBiases(_learningRate);

            foreach (var hiddenLayer in hiddenLayers)
                hiddenLayer.AdjustWeightsAndBiases(_learningRate);

            return originalMagnitude;
        }

        private double GradientClipping()
        {
            HandleNan();

            ValueGradientClipping();

            double gradientMagnitude = GradientMagnitude();

            if (Double.IsNaN(gradientMagnitude))
                throw new ArgumentOutOfRangeException();

            if (gradientMagnitude < gradientClippingThreshold)
                return gradientMagnitude;

            NormalizedGradientClipping(gradientMagnitude);

            return gradientMagnitude;
        }

        private void HandleNan()
        {
            foreach (var hiddenLayer in hiddenLayers)
                hiddenLayer.HandleNan(minZeroConvergeThreshold, gradientClippingThreshold);

            outputLayer.HandleNan(minZeroConvergeThreshold, gradientClippingThreshold);
        }

        private void ValueGradientClipping()
        {
            foreach (var hiddenlayer in hiddenLayers)
                hiddenlayer.ClipWeightsAndBiases(Math.Sqrt(gradientClippingThreshold));

            outputLayer.ClipWeightsAndBiases(Math.Sqrt(gradientClippingThreshold));
        }

        private double GradientMagnitude()
        {
            int amountOfElements = outputLayer.ChangeInHiddenLayerWeightsLength();
            amountOfElements += outputLayer.ChangeInHiddenLayerBiasLength();

            foreach (var hiddenLayer in hiddenLayers)
            {
                amountOfElements += hiddenLayer.ChangeInHiddenLayerWeightsLength();
                amountOfElements += hiddenLayer.ChangeInHiddenLayerBiasLength();
            }

            double gradientMagnitude = outputLayer.EuclideanSumOfWeightsAndBiases();

            foreach (var hiddenLayer in hiddenLayers)
                gradientMagnitude += hiddenLayer.EuclideanSumOfWeightsAndBiases();

            return Math.Sqrt(gradientMagnitude);
        }

        private void NormalizedGradientClipping(double _gradientMagnitude)
        {
            double changeOfEachElement = gradientClippingThreshold / _gradientMagnitude;

            outputLayer.ScaleChangeInWeightsAndBias(changeOfEachElement);

            foreach (var hiddenLayer in hiddenLayers)
                hiddenLayer.ScaleChangeInWeightsAndBias(changeOfEachElement);
        }

        public NeuralNetworkValues GetNeuralNetworkValues()
        {
            return new NeuralNetworkValues(inputSize,
                outputSize,
                batchSize,
                minZeroConvergeThreshold,
                hiddenLayerNodesAmount,
                inputLayer,
                hiddenLayers,
                outputLayer,
                gradientClippingThreshold);

        }
        #endregion -----------------------------------------------------------------
    }
}
