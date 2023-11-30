using System.Collections.Generic;

namespace ReinforcementLearning
{
    public readonly struct NfqResult
    {
        public readonly IFcq Model;
        public readonly string EndReason;
        public readonly List<double> EpisodeRewards;
        public readonly List<long> EpisodeTimeStep;
        public readonly List<long> EpisodeExploration;

        public NfqResult(IFcq model, 
            string endReason, 
            List<double> episodeRewards, 
            List<long> episodeTimeStep, 
            List<long> episodeExploration)
        {
            Model = model;
            EndReason = endReason;
            EpisodeRewards = episodeRewards;
            EpisodeTimeStep = episodeTimeStep;
            EpisodeExploration = episodeExploration;
        }
    }
}
