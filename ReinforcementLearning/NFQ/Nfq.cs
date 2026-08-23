using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Utilities;

namespace ReinforcementLearning
{
    public class Nfq
    {
        private double learnRate;
        private int batchSize;
        private int epochs;
        private double gamma;
        private double currentGamma;
        private Environment<double[]> environment;
        private int timeStepLimit;
        private int seed;
        private double maxMinutes;
        private long maxEpisodes;
        private IStrategy explorationStrategy;
        private IStrategy trainingStrategy;
        private bool legacyTarget;

        private Random prng;
        private int nS;
        private int nA;
        private IFcq onlineModel;

        private List<Experience<double[]>> experiences;
        private List<double> episodeRewards;
        private List<long> episodeTimeStep;
        private List<long> episodeExploration;
        private List<double> gradientMagnitudes;
        private List<Experience<double[]>> fitBatch;
        private int stepsSinceFit;

        // Experience replay: the batch is drawn at random from a buffer of recent transitions
        // instead of being the last batchSize steps, which nearly all come from the same few
        // episodes and from the same policy.
        private const int REPLAY_CAPACITY_IN_BATCHES = 20;

        public Nfq(NfqArgs _args)
        {
            learnRate = _args.LearnRate;
            gamma = _args.Gamma;
            batchSize = _args.BatchSize;
            epochs = _args.Epochs;
            environment = _args.Environment;
            seed = _args.Seed;
            maxMinutes = _args.MaxMinutes;
            maxEpisodes = _args.MaxEpisodes;
            explorationStrategy = _args.ExplorationStrategy;
            trainingStrategy = _args.TrainingStrategy;
            timeStepLimit = _args.TimeStepLimit;
            legacyTarget = _args.LegacyTarget;

            prng = seed == -1 ? new Random() : new Random(seed);
            nS = environment.ObservationSpaceSize;
            nA = environment.ActionSpaceSize;

            onlineModel = new NeuralNetwork(nS,
                _args.HiddenLayerNodesAmount,
                _args.HiddenLayersAmount,
                nA,
                batchSize,
                _args.GradientClippingThreshold,
                _args.MinZeroConvergeThreshold,
                _args.FixNan,
                _args.ClipValuesFirst,
                prng);

            experiences = new List<Experience<double[]>>();
            episodeRewards = new List<double>();
            episodeTimeStep = new List<long>();
            episodeExploration = new List<long>();
            gradientMagnitudes = new List<double>();
            fitBatch = new List<Experience<double[]>>();
        }

        public Nfq(NfqArgs _args, NeuralNetwork _nn)
        {
            learnRate = _args.LearnRate;
            gamma = _args.Gamma;
            batchSize = _args.BatchSize;
            epochs = _args.Epochs;
            environment = _args.Environment;
            seed = _args.Seed;
            maxMinutes = _args.MaxMinutes;
            maxEpisodes = _args.MaxEpisodes;
            explorationStrategy = _args.ExplorationStrategy;
            trainingStrategy = _args.TrainingStrategy;
            timeStepLimit = _args.TimeStepLimit;
            legacyTarget = _args.LegacyTarget;

            prng = seed == -1 ? new Random() : new Random(seed);
            nS = environment.ObservationSpaceSize;
            nA = environment.ActionSpaceSize;
            onlineModel = _nn;

            experiences = new List<Experience<double[]>>();
            episodeRewards = new List<double>();
            episodeTimeStep = new List<long>();
            episodeExploration = new List<long>();
            gradientMagnitudes = new List<double>();
            fitBatch = new List<Experience<double[]>>();
        }

        public NfqResult Train()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            TimeSpan maxTimeSpan = TimeSpan.FromMinutes(maxMinutes);

            bool isTrainingFinished = false;
            string trainingFinishedReason = "";

            for (int episode = 1; !isTrainingFinished; episode++)
            {
                double episodesFinishedPercent = (float)episode / maxEpisodes;
                double timeFinishedPercent = stopwatch.Elapsed.TotalMilliseconds / maxTimeSpan.TotalMilliseconds;

                Dialogue.PrintProgress((float)Math.Max(episodesFinishedPercent, timeFinishedPercent), episode == 1);

                double[] state = environment.Reset(timeStepLimit != 0, true, prng, timeStepLimit);
                bool isTerminal = false;
                bool nanOccured = false;
                currentGamma = gamma;
                episodeRewards.Add(0.0f);
                episodeTimeStep.Add(0);
                episodeExploration.Add(0);

                for(long step = 0; !isTerminal ; step++)
                {
                    (double[] NextState, bool Done) stepResult = InteractionStep(state, onlineModel, environment);
                    state = stepResult.NextState;
                    isTerminal = stepResult.Done;

                    if (InputManager.Interrupt)
                        goto exit;

                    stepsSinceFit++;

                    if (experiences.Count < batchSize || stepsSinceFit < batchSize)
                        continue;

                    stepsSinceFit = 0;
                    PrepareFitBatch();

                    try
                    {
                        double[] targets = legacyTarget ? null : ComputeTdTargets();

                        for (int i = 0; i < epochs; i++)
                        {
                            OptimizeModel(targets);

                            if (InputManager.Interrupt)
                                goto exit;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        nanOccured = true;
                        goto exit;
                    }

                }

            exit:
                if (stopwatch.Elapsed >= maxTimeSpan)
                {
                    isTrainingFinished = true;
                    trainingFinishedReason = "Max training time reached after " + maxTimeSpan.ToString() + ".";
                }

                if (nanOccured)
                {
                    isTrainingFinished = true;
                    trainingFinishedReason = "Nan occured by learning rate of " + learnRate + " after " + stopwatch.Elapsed.ToString() + ".";
                }

                if (InputManager.Interrupt)
                {
                    isTrainingFinished = true;
                    trainingFinishedReason = "Interrupt after " + stopwatch.Elapsed.ToString() + ".";
                }

                //if(episode >= maxEpisodes)
                //{
                //    isTrainingFinished = true;
                //    trainingFinishedReason = "Max episodes reached after " + stopwatch.Elapsed.ToString() + ".";
                //}
            }

            return new NfqResult(onlineModel,
                learnRate,
                trainingFinishedReason, 
                episodeRewards,
                episodeTimeStep, 
                episodeExploration, 
                gradientMagnitudes);
        }

        private (double[] NextState, bool Done) InteractionStep(double[] _state, IFcq _model, Environment<double[]> _environment)
        {
            int action = trainingStrategy.SelectAction(_state, _model, prng);
            StepResult<double[]> stepResult = _environment.Step(action);
            bool isFailure = stepResult.Done && !stepResult.IsTruncated;
            Experience<double[]> experience = new Experience<double[]>(_state,
                action,
                stepResult.Reward,
                stepResult.NextState,
                isFailure ? 1.0f : 0.0f,
                currentGamma);

            // Squaring compounds to gamma^(2^t) instead of gamma^t, and Q-learning applies
            // the discount once per bootstrap step anyway - kept only for the legacy variant.
            if (legacyTarget)
                currentGamma *= currentGamma;

            experiences.Add(experience);
            episodeRewards[episodeRewards.Count - 1] += stepResult.Reward;
            episodeTimeStep[episodeTimeStep.Count - 1] += 1;
            episodeExploration[episodeExploration.Count - 1] += trainingStrategy.ExploratoryActionTaken ? 1 : 0;
            return (stepResult.NextState, stepResult.Done || stepResult.IsTruncated);
        }

        /// <summary>
        /// Selects the transitions the next fitting round is trained on. The legacy variant
        /// keeps the original behaviour of taking the last batchSize steps and discarding
        /// them afterwards; the fixed variant samples from a buffer of recent transitions so
        /// consecutive batches are not all drawn from the same handful of episodes.
        /// </summary>
        private void PrepareFitBatch()
        {
            fitBatch.Clear();

            if (legacyTarget)
            {
                fitBatch.AddRange(experiences);
                experiences.Clear();
                return;
            }

            int capacity = REPLAY_CAPACITY_IN_BATCHES * batchSize;

            if (experiences.Count > capacity)
                experiences.RemoveRange(0, experiences.Count - capacity);

            for (int i = 0; i < batchSize; i++)
                fitBatch.Add(experiences[prng.Next(experiences.Count)]);
        }

        /// <summary>
        /// Computes the temporal difference targets once per fitting round. They stay fixed
        /// while the network is fitted to them, which is what makes this fitted Q iteration.
        /// Recomputing the bootstrap value after every gradient step turns it into a moving
        /// target that chases itself and lets the Q values diverge.
        /// </summary>
        private double[] ComputeTdTargets()
        {
            List<double[]> nextStates = fitBatch.Select(x => x.NextState).ToList();

            double[,] nextStateFeatureMatrix = Commons.ToMatrix(nextStates).Transpose();
            double[,] nextStateQValues = onlineModel.GetOutputMatrixDetached(nextStateFeatureMatrix);

            double[] targets = new double[fitBatch.Count];

            for (int i = 0; i < fitBatch.Count; i++)
            {
                double maxNextQValue = Commons.GetMaxValueOfColumn(nextStateQValues, i);

                targets[i] = fitBatch[i].Reward
                    + gamma * maxNextQValue * (1.0 - fitBatch[i].IsFailure);
            }

            return targets;
        }

        private void OptimizeModel(double[] _targets)
        {
            if (legacyTarget)
            {
                OptimizeModelLegacy();
                return;
            }

            List<double[]> states = fitBatch.Select(x => x.State).ToList();

            double[,] stateFeatureMatrix = Commons.ToMatrix(states).Transpose();
            double[,] statePredictionOutput = onlineModel.GetOutputMatrix(stateFeatureMatrix);

            // The output layer computes dZ = prediction - target, so what is handed to
            // Backwards has to be the target itself. Starting from the current prediction
            // and only overwriting the row of the action that was actually taken leaves the
            // error of every other action at exactly 0, so this step does not touch them.
            double[,] targetQS = statePredictionOutput.Clone() as double[,];

            for (int i = 0; i < fitBatch.Count; i++)
                targetQS[fitBatch[i].Action, i] = _targets[i];

            onlineModel.Backwards(targetQS);
            double gradientMagnitude = onlineModel.AdjustWeightsAndBiases(learnRate);

            gradientMagnitudes.Add(gradientMagnitude);
        }

        /// <summary>
        /// Original target computation, kept so both variants can be compared under identical
        /// conditions. It squares the temporal difference error and feeds that to Backwards,
        /// which expects a target - so the network is fitted to the squared error instead of
        /// to the Q value. It also lacks the max over the next actions and updates all
        /// actions instead of only the one that was taken.
        /// </summary>
        private void OptimizeModelLegacy()
        {
            List<double[]> states = fitBatch.Select(x => x.State).ToList();
            List<int> actions = fitBatch.Select(x => x.Action).ToList();
            List<double> rewards = fitBatch.Select(x => x.Reward).ToList();
            List<double[]> nextStates = fitBatch.Select(x => x.NextState).ToList();
            List<double> isTerminals = fitBatch.Select(x => x.IsFailure).ToList();
            double[] gammas = fitBatch.Select(x => x.Gamma).ToArray();

            double[,] nextStateFeatureMatrix = Commons.ToMatrix(nextStates).Transpose();

            double[,] maxAQSp = onlineModel.GetOutputMatrixDetached(nextStateFeatureMatrix);
            double[] oneMinusTerminals = Commons.SubtractFromValue(1.0f, isTerminals).ToArray();
            double[,] targetQS_a = Commons.MultiplyMatrixByArrayPerColumn(maxAQSp, oneMinusTerminals);
            double[,] targetQS_b = Commons.MultiplyMatrixByArrayPerColumn(targetQS_a, gammas);
            double[,] targetQS = Commons.AddVectorToMatrix(targetQS_b, rewards.ToArray());

            double[,] stateFeatureMatrix = Commons.ToMatrix(states).Transpose();
            double[,] statePredictionOutput = onlineModel.GetOutputMatrix(stateFeatureMatrix);
            double[,] tdErrors = Commons.Subtract(targetQS, statePredictionOutput);

            double[,] errorMatrix = new double[tdErrors.GetLength(0), tdErrors.GetLength(1)];
            for (int i = 0; i < tdErrors.GetLength(0); i++)
            {
                for (int j = 0; j < tdErrors.GetLength(1); j++)
                {
                    errorMatrix[i, j] = Math.Pow(tdErrors[i, j], 2);
                }
            }

            onlineModel.Backwards(errorMatrix);
            double gradientMagnitude = onlineModel.AdjustWeightsAndBiases(learnRate);

            try
            {
                gradientMagnitudes.Add(gradientMagnitude);
            }
            catch (Exception) { }
        }
    }
}
