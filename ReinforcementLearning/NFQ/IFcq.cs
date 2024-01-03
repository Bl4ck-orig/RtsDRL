namespace ReinforcementLearning
{
    public interface IFcq
    {
        double[,] GetOutputMatrix(double[,] _inputs);

        double[] GetHighestRewardVector(double[,] _inputs);

        double[] GetPrediction(double[] _inputs);

        void Backwards(double[,] _errorMatrix);

        double AdjustWeightsAndBiases(double _learningRate);

        NeuralNetworkValues GetNeuralNetworkValues();
    }
}
