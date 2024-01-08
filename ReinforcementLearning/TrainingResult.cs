using System;

namespace ReinforcementLearning
{
    [Serializable]
    public class TrainingResult
    {
        public int inputSize;
        public int outputSize;
        public int batchSize;
        public double minZeroConvergeThreshold;
        public int hiddenLayerNodesAmount;
        public double learnRate;
        public InputLayer inputLayer;
        public ReluLayer[] hiddenLayer;
        public OutputLayer outputLayer;
        public double gradientClippingThreshold;
        public string endReason;
        public double[] episodeRewards;
        public long[] episodeTimeStep;
        public long[] episodeExploration;
        public double[] gradientMagnitudes;

        public TrainingResult(NeuralNetworkValues neuralNetworkValues, 
            double learnRate,
            string endReason,
            double[] episodeRewards,
            long[] episodeTimeStep,
            long[] episodeExploration,
            double[] gradientMagnitudes)
        {
            inputSize = neuralNetworkValues.inputSize;
            outputSize = neuralNetworkValues.outputSize;
            batchSize = neuralNetworkValues.batchSize;
            minZeroConvergeThreshold = neuralNetworkValues.minZeroConvergeThreshold;
            hiddenLayerNodesAmount = neuralNetworkValues.hiddenLayerNodesAmount;
            inputLayer = neuralNetworkValues.inputLayer;
            hiddenLayer = neuralNetworkValues.hiddenLayer;
            outputLayer = neuralNetworkValues.outputLayer;
            gradientClippingThreshold = neuralNetworkValues.gradientClippingThreshold;

            this.learnRate = learnRate;
            this.endReason = endReason;
            this.episodeRewards = episodeRewards;
            this.episodeTimeStep = episodeTimeStep;
            this.episodeExploration = episodeExploration;
            this.gradientMagnitudes = gradientMagnitudes;
        }
    }

}
