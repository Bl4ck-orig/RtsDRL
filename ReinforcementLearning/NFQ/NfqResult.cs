using System.Collections.Generic;

namespace ReinforcementLearning
{
    public readonly struct NfqResult
    {
        public readonly IFcq Model;
        public readonly double LearnRate;
        public readonly string EndReason;
        public readonly List<double> EpisodeRewards;
        public readonly List<long> EpisodeTimeStep;
        public readonly List<long> EpisodeExploration;
        public readonly List<double> GradientMagnitudes;

        public NfqResult(IFcq model,
            double learnRate,
            string endReason, 
            List<double> episodeRewards, 
            List<long> episodeTimeStep, 
            List<long> episodeExploration,
            List<double> gradientMagnitudes)
        {
            Model = model;
            LearnRate = learnRate;
            EndReason = endReason;
            EpisodeRewards = episodeRewards;
            EpisodeTimeStep = episodeTimeStep;
            EpisodeExploration = episodeExploration;
            GradientMagnitudes = gradientMagnitudes;
        }

        public TrainingResult ToNeuralNetworkResults()
        {
            return new TrainingResult(Model.GetNeuralNetworkValues(),
                LearnRate,
                EndReason,
                EpisodeRewards.ToArray(),
                EpisodeTimeStep.ToArray(),
                EpisodeExploration.ToArray(),
                GradientMagnitudes.ToArray());
        }
    }
}
