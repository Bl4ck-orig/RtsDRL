using System;

namespace ReinforcementLearning
{
    [Serializable]
    public class TrainingResult
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
        public double[] episodeRewards;
        public long[] episodeTimeStep;
        public long[] episodeExploration;
        public string EndReason;

        public TrainingResult(NeuralNetworkValues neuralNetworkValues, 
            string endReason,
            double[] episodeRewards,
            long[] episodeTimeStep,
            long[] episodeExploration)
        {
            inputSize = neuralNetworkValues.inputSize;
            outputSize = neuralNetworkValues.outputSize;
            batchSize = neuralNetworkValues.batchSize;
            hiddenWeights = neuralNetworkValues.hiddenWeights;
            hiddenBias = neuralNetworkValues.hiddenBias;
            outputWeights = neuralNetworkValues.outputWeights;
            outputBias = neuralNetworkValues.outputBias;
            hiddenLayerNodesAmount = neuralNetworkValues.hiddenLayerNodesAmount;
            learningRate = neuralNetworkValues.learningRate;
            inputLayer = neuralNetworkValues.inputLayer;
            hiddenLayer = neuralNetworkValues.hiddenLayer;
            hiddenLayerZ = neuralNetworkValues.hiddenLayerZ;
            outputLayer = neuralNetworkValues.outputLayer;
            outputLayerZ = neuralNetworkValues.outputLayerZ;
            errorMatrix = neuralNetworkValues.errorMatrix;
            oneOverBatchSize = neuralNetworkValues.oneOverBatchSize;
            changeInOutputLayerError = neuralNetworkValues.changeInOutputLayerError;
            changeInOutputWeights = neuralNetworkValues.changeInOutputWeights;
            changeInOutputBias = neuralNetworkValues.changeInOutputBias;
            changeInHiddenLayerError = neuralNetworkValues.changeInHiddenLayerError;
            changeInHiddenLayerWeights = neuralNetworkValues.changeInHiddenLayerWeights;
            changeInHiddenBias = neuralNetworkValues.changeInHiddenBias;
            EndReason = endReason;
            this.episodeRewards = episodeRewards;
            this.episodeTimeStep = episodeTimeStep;
            this.episodeExploration = episodeExploration;
        }


    }

}
