namespace ReinforcementLearning
{
    [System.Serializable]
    public struct NeuralNetworkValues
    {
        public int inputSize;
        public int outputSize;
        public int batchSize;
        public double minZeroConvergeThreshold;
        public int hiddenLayerNodesAmount;
        public InputLayer inputLayer;
        public ReluLayer[] hiddenLayer;
        public OutputLayer outputLayer;
        public double gradientClippingThreshold;

        public NeuralNetworkValues(int inputSize,
            int outputSize,
            int batchSize,
            double minZeroConvergeThreshold,
            int hiddenLayerNodesAmount,
            InputLayer inputLayer,
            ReluLayer[] hiddenLayer,
            OutputLayer outputLayer,
            double gradientClippingThreshold)
        {
            this.inputSize = inputSize;
            this.outputSize = outputSize;
            this.batchSize = batchSize;
            this.minZeroConvergeThreshold = minZeroConvergeThreshold;
            this.hiddenLayerNodesAmount = hiddenLayerNodesAmount;
            this.inputLayer = inputLayer;
            this.hiddenLayer = hiddenLayer;
            this.outputLayer = outputLayer;
            this.gradientClippingThreshold = gradientClippingThreshold;
        }
    }
}
