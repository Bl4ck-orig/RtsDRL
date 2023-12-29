namespace ReinforcementLearning
{
    public readonly struct NfqArgs
    {
        public readonly double LearnRate;
        public readonly int BatchSize;
        public readonly int Epochs;
        public readonly Environment<double[]> Environment;
        public readonly double Gamma;
        public readonly double MaxMinutes;
        public readonly long MaxEpisodes;
        public readonly IStrategy ExplorationStrategy;
        public readonly IStrategy TrainingStrategy;
        public readonly int TimeStepLimit;
        public readonly int HiddenLayerSize;
        public readonly int Seed;

        public NfqArgs(Environment<double[]> _environment, 
            IStrategy _explorationStrategy,
            IStrategy _trainingStrategy,
            double _learnRate = 0.0001f, 
            int _batchSize = 1024, 
            int _epochs = 40, 
            double _gamma = 1.0f, 
            double _maxMinutes = 600f, 
            long _maxEpisodes = 10000,
            int _timeStepLimit = 200,
            int _hiddenLayerSize = 512,
            int _seed = -1)
        {
            LearnRate = _learnRate;
            BatchSize = _batchSize;
            Epochs = _epochs;
            Environment = _environment;
            Gamma = _gamma;
            MaxMinutes = _maxMinutes;
            MaxEpisodes = _maxEpisodes;
            ExplorationStrategy = _explorationStrategy;
            TrainingStrategy = _trainingStrategy;
            TimeStepLimit = _timeStepLimit;
            HiddenLayerSize = _hiddenLayerSize;
            Seed = _seed;
        }
    }
}
