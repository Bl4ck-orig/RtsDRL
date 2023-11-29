namespace ReinforcementLearning
{
    public interface IFcq
    {
        double[,] GetOutputMatrix(double[,] _inputs);

        double[] GetHighestRewardOutputIndex(double[,] _inputs);

        double[] GetPrediction(double[] _inputs);

        void Backwards(double[,] _errorMatrix, double _learningRate);

        void AdjustWeightsAndBiases();
    }
}
