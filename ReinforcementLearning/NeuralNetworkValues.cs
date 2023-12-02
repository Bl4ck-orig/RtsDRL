namespace ReinforcementLearning
{
    public struct NeuralNetworkValues
    {
        public int inputSize;
        public int outputSize;
        public int batchSize;

        public double[,] hiddenWeights;
        public double[,] hiddenBias;
        public double[,] outputWeights;
        public double[,] outputBias;

        public int hiddenLayerNodesAmount;
        public double learningRate;

        public double[,] inputLayer;

        public double[,] hiddenLayer;
        public double[,] hiddenLayerZ;

        public double[,] outputLayer;
        public double[,] outputLayerZ;

        public double[,] errorMatrix;
        public double oneOverBatchSize;

        public double[,] changeInOutputLayerError;
        public double[,] changeInOutputWeights;
        public double[,] changeInOutputBias;

        public double[,] changeInHiddenLayerError;
        public double[,] changeInHiddenLayerWeights;
        public double[,] changeInHiddenBias;

        public NeuralNetworkValues(int inputSize,
            int outputSize, int batchSize,
            double[,] hiddenWeights,
            double[,] hiddenBias,
            double[,] outputWeights,
            double[,] outputBias,
            int hiddenLayerNodesAmount,
            double learningRate,
            double[,] inputLayer,
            double[,] hiddenLayer,
            double[,] hiddenLayerZ,
            double[,] outputLayer,
            double[,] outputLayerZ,
            double[,] errorMatrix,
            double oneOverBatchSize,
            double[,] changeInOutputLayerError,
            double[,] changeInOutputWeights,
            double[,] changeInOutputBias,
            double[,] changeInHiddenLayerError,
            double[,] changeInHiddenLayerWeights,
            double[,] changeInHiddenBias)
        {
            this.inputSize = inputSize;
            this.outputSize = outputSize;
            this.batchSize = batchSize;
            this.hiddenWeights = hiddenWeights;
            this.hiddenBias = hiddenBias;
            this.outputWeights = outputWeights;
            this.outputBias = outputBias;
            this.hiddenLayerNodesAmount = hiddenLayerNodesAmount;
            this.learningRate = learningRate;
            this.inputLayer = inputLayer;
            this.hiddenLayer = hiddenLayer;
            this.hiddenLayerZ = hiddenLayerZ;
            this.outputLayer = outputLayer;
            this.outputLayerZ = outputLayerZ;
            this.errorMatrix = errorMatrix;
            this.oneOverBatchSize = oneOverBatchSize;
            this.changeInOutputLayerError = changeInOutputLayerError;
            this.changeInOutputWeights = changeInOutputWeights;
            this.changeInOutputBias = changeInOutputBias;
            this.changeInHiddenLayerError = changeInHiddenLayerError;
            this.changeInHiddenLayerWeights = changeInHiddenLayerWeights;
            this.changeInHiddenBias = changeInHiddenBias;
        }
    }
}
