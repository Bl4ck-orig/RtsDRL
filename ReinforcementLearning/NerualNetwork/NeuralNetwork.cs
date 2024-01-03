using System;

namespace ReinforcementLearning
{
    public class NeuralNetwork : IFcq
    {
        private int inputSize;
        private int outputSize;
        private int batchSize;

        private int hiddenLayerNodesAmount;

        private InputLayer inputLayer;
        private ReluLayer[] hiddenLayer;
        private OutputLayer outputLayer;

        private double gradientClippingThreshold = 0f;

        public NeuralNetwork(int _inputSize, 
            int _hiddenLayerNodesAmount, 
            int _hiddenLayersAmount, 
            int _outputSize,
            int _batchSize,
            double _gradientClippingThreshold, 
            Random _prng)
        {
            inputSize = _inputSize;
            outputSize = _outputSize;
            batchSize = _batchSize;

            hiddenLayerNodesAmount = _hiddenLayerNodesAmount;

            inputLayer = new InputLayer(inputSize, _batchSize);
            
            hiddenLayer = new ReluLayer[_hiddenLayersAmount];
            hiddenLayer[0] = new ReluLayer(inputLayer, hiddenLayerNodesAmount, _batchSize);
            for (int i = 1; i < _hiddenLayersAmount; i++)
                hiddenLayer[i] = new ReluLayer(hiddenLayer[i - 1], hiddenLayerNodesAmount, _batchSize);

            outputLayer = new OutputLayer(hiddenLayer[hiddenLayer.Length - 1], outputSize, _batchSize);

            inputLayer.InitializeXavier(_prng);
            foreach (var hiddenLayer in hiddenLayer)
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
             hiddenLayer = _trainingResult.hiddenLayer;
             outputLayer = _trainingResult.outputLayer;
             gradientClippingThreshold = _trainingResult.gradientClippingThreshold;
        }

        #region Forward Propagation -----------------------------------------------------------------
        public void ApplyForwardPropagation()
        {
            foreach (var layer in hiddenLayer)
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

            hiddenLayer[hiddenLayer.Length - 1].BackwardPropagate(outputLayer);
            
            for(int i = hiddenLayer.Length - 2; i >= 0; i--)
                hiddenLayer[i].BackwardPropagate(hiddenLayer[i + 1]);
        }

        public double AdjustWeightsAndBiases(double _learningRate)
        {
            double originalMagnitude = GradientClipping();

            outputLayer.AdjustWeightsAndBiases(_learningRate);

            foreach (var hiddenLayer in hiddenLayer)
                hiddenLayer.AdjustWeightsAndBiases(_learningRate);

            return originalMagnitude;
        }

        private double GradientClipping()
        {
            int amountOfElements = outputLayer.ChangeInHiddenLayerWeightsLength();
            amountOfElements += outputLayer.ChangeInHiddenLayerBiasLength();

            foreach (var hiddenLayer in hiddenLayer)
            {
                amountOfElements += hiddenLayer.ChangeInHiddenLayerWeightsLength();
                amountOfElements += hiddenLayer.ChangeInHiddenLayerBiasLength();
            }

            double gradientMagnitude = outputLayer.EuclideanSumOfWeightsAndBiases();

            foreach (var hiddenLayer in hiddenLayer)
                gradientMagnitude += hiddenLayer.EuclideanSumOfWeightsAndBiases();

            if (Double.IsNaN(gradientMagnitude))
                throw new ArgumentOutOfRangeException();

            if (gradientMagnitude < gradientClippingThreshold)
                return gradientMagnitude;

            double changeOfEachElement = gradientClippingThreshold / gradientMagnitude;

            outputLayer.ScaleChangeInWeightsAndBias(changeOfEachElement);

            foreach (var hiddenLayer in hiddenLayer)
                hiddenLayer.ScaleChangeInWeightsAndBias(changeOfEachElement);

            return gradientMagnitude;
        }

        public NeuralNetworkValues GetNeuralNetworkValues()
        {
            return new NeuralNetworkValues(inputSize,
                outputSize,
                batchSize,
                hiddenLayerNodesAmount,
                inputLayer,
                hiddenLayer,
                outputLayer,
                gradientClippingThreshold);

        }
        #endregion -----------------------------------------------------------------
    }
}
