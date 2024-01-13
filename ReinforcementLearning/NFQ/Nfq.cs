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
        private Environment<double[]> environment;
        private int timeStepLimit;
        private int seed;
        private double maxMinutes;
        private long maxEpisodes;
        private IStrategy explorationStrategy;
        private IStrategy trainingStrategy;

        private Random prng;
        private int nS;
        private int nA;
        private IFcq onlineModel;

        private List<Experience<double[]>> experiences;
        private List<double> episodeRewards;
        private List<long> episodeTimeStep;
        private List<long> episodeExploration;
        private List<double> gradientMagnitudes;

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

            prng = seed == -1 ? new Random() : new Random(seed);
            nS = environment.ObservationSpaceSize;
            nA = environment.ActionSpaceSize;
            onlineModel = _nn;

            experiences = new List<Experience<double[]>>();
            episodeRewards = new List<double>();
            episodeTimeStep = new List<long>();
            episodeExploration = new List<long>();
            gradientMagnitudes = new List<double>();
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

                    if (experiences.Count < batchSize)
                        continue;

                    for (int i = 0; i < epochs; i++)
                    {
                        OptimizeModel();

                        if (InputManager.Interrupt)
                            goto exit;
                    }

                    experiences.Clear();
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
                isFailure ? 1.0f : 0.0f);

            experiences.Add(experience);
            episodeRewards[episodeRewards.Count - 1] += stepResult.Reward;
            episodeTimeStep[episodeTimeStep.Count - 1] += 1;
            episodeExploration[episodeExploration.Count - 1] += trainingStrategy.ExploratoryActionTaken ? 1 : 0;
            return (stepResult.NextState, stepResult.Done || stepResult.IsTruncated);
        }

        private void OptimizeModel()
        {
            List<double[]> states = experiences.Select(x => x.State).ToList();
            List<int> actions = experiences.Select(x => x.Action).ToList();
            List<double> rewards = experiences.Select(x => x.Reward).ToList();
            List<double[]> nextStates = experiences.Select(x => x.NextState).ToList();
            List<double> isTerminals = experiences.Select(x => x.IsFailure).ToList();

            double[,] nextStateFeatureMatrix = Commons.ToMatrix(nextStates).Transpose(); 
            double[,] maxAQSp = onlineModel.GetOutputMatrix(nextStateFeatureMatrix);
            double[] oneMinusTerminals = Commons.SubtractFromValue(1.0f, isTerminals).ToArray();
            double[,] targetQS_a = Commons.MultiplyMatrixByArrayPerColumn(maxAQSp, oneMinusTerminals);
            double[,] targetQS_b = Commons.Multiply(targetQS_a, gamma);
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
