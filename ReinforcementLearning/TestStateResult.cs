namespace ReinforcementLearning
{
    public readonly struct TestStateResult
    {
        public readonly double ModelCumulativeReward;
        public readonly double ModelLastReward;
        public readonly double NoOpCumulativeReward;
        public readonly double NoOpLastReward;
        public readonly double RandomCumulativeReward;
        public readonly double RandomLastReward;

        public TestStateResult(double modelCumulativeReward,
            double modelLastReward,
            double noOpCumulativeReward,
            double noOpLastReward, 
            double randomCumulativeReward, 
            double randomLastReward)
        {
            this.ModelCumulativeReward = modelCumulativeReward;
            this.ModelLastReward = modelLastReward;
            this.NoOpCumulativeReward = noOpCumulativeReward;
            this.NoOpLastReward = noOpLastReward;
            this.RandomCumulativeReward = randomCumulativeReward;
            this.RandomLastReward = randomLastReward;
            this.ModelCumulativeReward = modelCumulativeReward;
            this.ModelLastReward = modelLastReward;
            this.NoOpCumulativeReward = noOpCumulativeReward;
            this.NoOpLastReward = noOpLastReward;
            this.RandomCumulativeReward = randomCumulativeReward;
            this.RandomLastReward = randomLastReward;
        }
    }
}
