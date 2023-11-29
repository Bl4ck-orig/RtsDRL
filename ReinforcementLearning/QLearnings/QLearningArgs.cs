
namespace ReinforcementLearning.Training
{
    public readonly struct QLearningArgs
    {
        public readonly Environment<int> Environment;
        public readonly double Gamma;
        public readonly double InitAlpha;
        public readonly double MinAlpha;
        public readonly double AlphaDecayRatio;
        public readonly double InitEpsilon;
        public readonly double MinEpsilon;
        public readonly double EpsilonDecayRatio;
        public readonly int NEpisodes;

        public QLearningArgs(Environment<int> environment,
            double gamma = 1.0f,
            double initAlpha = 0.5f,
            double minAlpha = 0.01f,
            double alphaDecayRatio = 0.5f,
            double initEpsilon = 1.0f,
            double minEpsilon = 0.1f,
            double epsilonDecayRatio = 0.9f,
            int nEpisodes = 3000)
        {
            Environment = environment;
            Gamma = gamma;
            InitAlpha = initAlpha;
            MinAlpha = minAlpha;
            AlphaDecayRatio = alphaDecayRatio;
            InitEpsilon = initEpsilon;
            MinEpsilon = minEpsilon;
            EpsilonDecayRatio = epsilonDecayRatio;
            NEpisodes = nEpisodes;
        }
    }
}
