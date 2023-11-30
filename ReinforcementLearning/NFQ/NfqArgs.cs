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
        public readonly int Seed;

        public NfqArgs(Environment<double[]> environment, 
            IStrategy explorationStrategy,
            IStrategy trainingStrategy,
            double learnRate = 0, 
            int batchSize = 1024, 
            int epochs = 40, 
            double gamma = 1.0f, 
            double maxMinutes = 600f, 
            long maxEpisodes = 10000,
            int _timeStepLimit = 200,
            int seed = -1)
        {
            LearnRate = learnRate;
            BatchSize = batchSize;
            Epochs = epochs;
            Environment = environment;
            Seed = seed;
            Gamma = gamma;
            MaxMinutes = maxMinutes;
            MaxEpisodes = maxEpisodes;
            ExplorationStrategy = explorationStrategy;
            TrainingStrategy = trainingStrategy;
            TimeStepLimit = _timeStepLimit;
            Seed = seed;
        }
    }
}
