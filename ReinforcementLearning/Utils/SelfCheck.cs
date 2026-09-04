using System;
using System.Globalization;

namespace ReinforcementLearning.Utils
{
    /// <summary>
    /// Numerical checks for the learning step that do not need a full training run: single
    /// state inference has to match the batch forward pass, and one optimisation step has to
    /// move the Q value of the chosen action towards its target while leaving the other
    /// actions untouched.
    /// </summary>
    public static class SelfCheck
    {
        private const int INPUT_SIZE = 24;
        private const int OUTPUT_SIZE = 14;
        private const int BATCH_SIZE = 32;
        private const int HIDDEN_NODES = 50;
        private const int HIDDEN_LAYERS = 3;
        private const int NETWORK_SEED = 7;

        public static void Run()
        {
            CheckInferenceMatchesBatch();
            CheckUpdateDirection();
            CheckGradientStep();
        }

        private static NeuralNetwork CreateNetwork()
        {
            return new NeuralNetwork(INPUT_SIZE,
                HIDDEN_NODES,
                HIDDEN_LAYERS,
                OUTPUT_SIZE,
                BATCH_SIZE,
                600.0,
                0.00001,
                false,
                false,
                new Random(NETWORK_SEED));
        }

        private static double[] CreateState(Random _prng)
        {
            double[] state = new double[INPUT_SIZE];

            for (int i = 0; i < INPUT_SIZE; i++)
                state[i] = _prng.Next(0, 20);

            return state;
        }

        private static double[,] Replicate(double[] _state)
        {
            double[,] matrix = new double[INPUT_SIZE, BATCH_SIZE];

            for (int x = 0; x < INPUT_SIZE; x++)
                for (int y = 0; y < BATCH_SIZE; y++)
                    matrix[x, y] = _state[x];

            return matrix;
        }

        /// <summary>
        /// GetPrediction no longer replicates the state across all batch columns, so it has to
        /// be shown to still return the values of the batch forward pass.
        /// </summary>
        private static void CheckInferenceMatchesBatch()
        {
            NeuralNetwork network = CreateNetwork();
            double[] state = CreateState(new Random(11));

            double[,] batchOutput = network.GetOutputMatrix(Replicate(state));
            double[] vectorOutput = network.GetPrediction(state);

            double maxDifference = 0.0;

            for (int a = 0; a < OUTPUT_SIZE; a++)
                maxDifference = Math.Max(maxDifference, Math.Abs(batchOutput[a, 0] - vectorOutput[a]));

            Report("single state inference equals batch forward pass",
                maxDifference < 0.000000001,
                "largest deviation " + Format(maxDifference));
        }

        /// <summary>
        /// Compares the value handed to the output layer by both variants. The layer computes
        /// dZ = prediction - argument, so a positive dZ lowers the Q value and a negative dZ
        /// raises it.
        /// </summary>
        private static void CheckUpdateDirection()
        {
            double[][] cases =
            {
                new[] { 0.50, 0.60 },
                new[] { 2.00, 3.00 },
                new[] { 0.80, 0.20 },
                new[] { -0.30, 0.40 },
            };

            Console.WriteLine("  prediction  target   dZ fixed   dZ legacy   same direction");

            bool allAgree = true;

            foreach (double[] testCase in cases)
            {
                double prediction = testCase[0];
                double target = testCase[1];

                double dzFixed = prediction - target;
                double dzLegacy = prediction - Math.Pow(target - prediction, 2);

                bool agrees = Math.Sign(dzFixed) == Math.Sign(dzLegacy);
                allAgree &= agrees;

                Console.WriteLine("  " + Format(prediction).PadRight(12)
                    + Format(target).PadRight(9)
                    + Format(dzFixed).PadRight(11)
                    + Format(dzLegacy).PadRight(12)
                    + (agrees ? "yes" : "NO"));
            }

            Report("legacy update points somewhere else than the target",
                !allAgree,
                "the squared error drops the direction of the temporal difference error");
        }

        /// <summary>
        /// Runs a single optimisation step against a target that lies slightly above the
        /// current Q value of one action and checks where that Q value actually moves. A
        /// positive Q value together with a small temporal difference error is the regime
        /// that dominates training once the rewards are sparse, so the action with the
        /// highest Q value is used.
        /// </summary>
        private static void CheckGradientStep()
        {
            const double LEARN_RATE = 0.0002;
            const double TD_ERROR = 0.1;

            double[] state = CreateState(new Random(11));
            double[,] inputs = Replicate(state);

            NeuralNetwork fixedNetwork = CreateNetwork();
            double[,] before = fixedNetwork.GetOutputMatrix(inputs);

            int ACTION = 0;
            for (int a = 1; a < OUTPUT_SIZE; a++)
                if (before[a, 0] > before[ACTION, 0])
                    ACTION = a;

            double wanted = before[ACTION, 0] + TD_ERROR;

            double[,] target = before.Clone() as double[,];
            for (int i = 0; i < BATCH_SIZE; i++)
                target[ACTION, i] = wanted;

            fixedNetwork.Backwards(target);
            fixedNetwork.AdjustWeightsAndBiases(LEARN_RATE);
            double[,] afterFixed = fixedNetwork.GetOutputMatrix(inputs);

            NeuralNetwork legacyNetwork = CreateNetwork();
            double[,] legacyBefore = legacyNetwork.GetOutputMatrix(inputs);

            double[,] squaredErrors = new double[OUTPUT_SIZE, BATCH_SIZE];
            for (int a = 0; a < OUTPUT_SIZE; a++)
                for (int i = 0; i < BATCH_SIZE; i++)
                    squaredErrors[a, i] = Math.Pow(target[a, i] - legacyBefore[a, i], 2);

            legacyNetwork.Backwards(squaredErrors);
            legacyNetwork.AdjustWeightsAndBiases(LEARN_RATE);
            double[,] afterLegacy = legacyNetwork.GetOutputMatrix(inputs);

            Console.WriteLine("  Q of action " + ACTION + " before " + Format(before[ACTION, 0])
                + ", target " + Format(wanted));
            Console.WriteLine("    fixed  -> " + Format(afterFixed[ACTION, 0]));
            Console.WriteLine("    legacy -> " + Format(afterLegacy[ACTION, 0]));

            Report("fixed step raises the chosen action towards its target",
                afterFixed[ACTION, 0] > before[ACTION, 0],
                "moved by " + Format(afterFixed[ACTION, 0] - before[ACTION, 0]));

            Report("legacy step lowers it instead, towards zero",
                afterLegacy[ACTION, 0] < legacyBefore[ACTION, 0],
                "moved by " + Format(afterLegacy[ACTION, 0] - legacyBefore[ACTION, 0]));

            double largestOtherChange = 0.0;
            double largestOtherChangeLegacy = 0.0;

            for (int a = 0; a < OUTPUT_SIZE; a++)
            {
                if (a == ACTION)
                    continue;

                largestOtherChange = Math.Max(largestOtherChange,
                    Math.Abs(afterFixed[a, 0] - before[a, 0]));
                largestOtherChangeLegacy = Math.Max(largestOtherChangeLegacy,
                    Math.Abs(afterLegacy[a, 0] - legacyBefore[a, 0]));
            }

            Report("fixed step changes the actions that were not taken far less",
                largestOtherChange < Math.Abs(afterFixed[ACTION, 0] - before[ACTION, 0]),
                "largest change elsewhere " + Format(largestOtherChange)
                    + ", legacy " + Format(largestOtherChangeLegacy));
        }

        private static void Report(string _name, bool _passed, string _detail)
        {
            Console.WriteLine((_passed ? "  [ok]   " : "  [FAIL] ") + _name + " (" + _detail + ")");
        }

        private static string Format(double _value) => _value.ToString("G6", CultureInfo.InvariantCulture);
    }
}
