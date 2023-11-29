using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReinforcementLearning
{
    public class Nfq
    {
        //         value_optimizer_fn, = Gradient Descent Methode
        //         training_strategy_fn, = EGreedyStrategy(epsilon=0.5f)
        //         evaluation_strategy_fn, = GreedyStrategy()

        private Func<int, int, IFcq> valueModelFn;
        private double learnRate;
        private int batchSize;
        private int epochs;
        private EGreedyStrategy trainingStrategyFn;

        public Nfq(NfQArgs _args)
        {
            valueModelFn = _args.ValueModelFn;
            learnRate = _args.LearnRate;
            batchSize = _args.BatchSize;
            epochs = _args.Epochs;
        }

        private void OptimizeModel()
        {

        }

        private void InteractionStep(double[] _state, Environment<double[]> _environment)
        {
            int action = trainingStrategyFn.SelectAction(_state, _environment);
            
        }
    }

    public class EGreedyStrategy
    {
        private double epsilon;
        private bool exploratoryActionTaken;

        public EGreedyStrategy(double _epsilon = 0.1f)
        {
            epsilon = _epsilon;
            exploratoryActionTaken = false;
        }

        public int SelectAction(double[] _state, Environment<double[]> _environment)
        {
            throw new NotImplementedException();
        }
    }

    public readonly struct NfQArgs
    {
        public readonly Func<int, int, IFcq> ValueModelFn;
        public readonly double LearnRate;
        public readonly int BatchSize;
        public readonly int Epochs;

        public NfQArgs(Func<int, int, IFcq> valueModelFn, double learnRate = 0.0005f, int batchSize = 1024, int epochs = 40)
        {
            ValueModelFn = valueModelFn;
            LearnRate = learnRate;
            BatchSize = batchSize;
            Epochs = epochs;
        }

    }


}
