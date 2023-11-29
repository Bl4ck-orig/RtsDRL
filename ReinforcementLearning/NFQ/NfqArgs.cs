namespace ReinforcementLearning
{
    public readonly struct NfqArgs
    {
        public readonly IFcq ValueModelFn;
        public readonly double LearnRate;
        public readonly int BatchSize;
        public readonly int Epochs;
        public readonly Environment<double[]> Environment;
        public readonly double Gamma;
        public readonly double MaxMinutes;
        public readonly long MaxEpisodes;
        public readonly IStrategy ExplorationStrategy;
        public readonly IStrategy TrainingStrategy;
        public readonly int Seed;

        public NfqArgs(IFcq valueModelFn, 
            double learnRate, 
            int batchSize, 
            int epochs, 
            Environment<double[]> environment, 
            double gamma, 
            double maxMinutes, 
            long maxEpisodes, 
            IStrategy explorationStrategy,
            IStrategy trainingStrategy,
            int seed = -1)
        {
            ValueModelFn = valueModelFn;
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
            Seed = seed;
        }
    }
}
