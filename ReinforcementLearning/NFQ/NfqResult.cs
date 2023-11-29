namespace ReinforcementLearning
{
    public readonly struct NfqResult
    {
        public readonly IFcq Model;
        public readonly string EndReason;

        public NfqResult(IFcq model, string endReason)
        {
            Model = model;
            EndReason = endReason;
        }
    }
}
