using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ReinforcementLearning
{
    public class Nfq
    {
        private IFcq valueModelFn;
        private double learnRate;
        private int batchSize;
        private int epochs;
        private double gamma;
        private Environment<double[]> environment;
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

        public Nfq(NfqArgs _args)
        {
            valueModelFn = _args.ValueModelFn;
            learnRate = _args.LearnRate;
            batchSize = _args.BatchSize;
            epochs = _args.Epochs;
            environment = _args.Environment;
            seed = _args.Seed;
            maxMinutes = _args.MaxMinutes;
            maxEpisodes = _args.MaxEpisodes;
            explorationStrategy = _args.ExplorationStrategy;
            trainingStrategy = _args.TrainingStrategy;

            prng = seed == -1 ? new Random() : new Random(seed);
            nS = environment.ObservationSpaceSize;
            nA = environment.ActionSpaceSize;
            onlineModel = new NeuralNetwork(nS, nA, batchSize, prng);

            experiences = new List<Experience<double[]>>();
            episodeRewards = new List<double>();
            episodeTimeStep = new List<long>();
            episodeExploration = new List<long>();
        }

        private void OptimizeModel()
        {
            List<double[]> states = experiences.Select(x => x.State).ToList();
            List<int> actions = experiences.Select(x => x.Action).ToList();
            List<double> rewards = experiences.Select(x => x.Reward).ToList();
            List<double[]> nextStates = experiences.Select(x => x.NextState).ToList();
            List<double> isTerminals = experiences.Select(x => x.IsFailure).ToList();

            double[,] nextStateFeatureMatrix = Commons.ToMatrix(nextStates);
            double[,] maxAQSp = Commons.Unsqueeze(valueModelFn.GetHighestRewardOutputIndex(nextStateFeatureMatrix));
            double[] oneMinusTerminals = Commons.SubtractFromValue(1.0f, isTerminals).ToArray();
            double[,] targetQS_a = Commons.MultiplyMatrixByArray(maxAQSp, oneMinusTerminals);
            double[,] targetQS_b = Commons.AddMatrixBy(targetQS_a, gamma);
            double[,] targetQS = Commons.AddVectorToMatrix(targetQS_b, rewards.ToArray());

            double[,] stateFeatureMatrix = Commons.ToMatrix(states);
            double[,] statePredictionOutput = valueModelFn.GetOutputMatrix(stateFeatureMatrix);
            double[] qSa = new double[batchSize];
            for (int i = 0; i < batchSize; i++)
            {
                qSa[i] = statePredictionOutput[actions[i], i];
            }

            double[,] tdErrors = Commons.SubtractVectorFromMatrixColumns(qSa, stateFeatureMatrix);
            valueModelFn.Backwards(tdErrors, learnRate);
            valueModelFn.AdjustWeightsAndBiases();
        }

        private (double[] NextState, bool Done) InteractionStep(double[] _state, IFcq _model, Environment<double[]> _environment)
        {
            int action = trainingStrategy.SelectAction(_state, _model, prng);
            StepResult<double[]> stepResult = _environment.Step(action, prng);
            bool isFailure = stepResult.Done && !stepResult.IsTruncated;
            Experience<double[]> experience = new Experience<double[]>(_state, 
                action, 
                stepResult.Reward, 
                stepResult.NextState, 
                isFailure ? 0.0f : 1.0f);

            experiences.Add(experience);
            episodeRewards[episodeRewards.Count - 1] += stepResult.Reward;
            episodeTimeStep[episodeTimeStep.Count - 1] += 1;
            episodeExploration[episodeExploration.Count - 1] += trainingStrategy.ExploratoryActionTaken ? 1 : 0;
            return (stepResult.NextState, stepResult.Done);
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
                double[] state = environment.Reset(prng);
                bool isTerminal = false;
                episodeRewards.Add(0.0f);
                episodeTimeStep.Add(0);
                episodeExploration.Add(0);

                for(long step = 0; !isTerminal ; step++)
                {
                    (double[] NextState, bool Done) stepResult = InteractionStep(state, onlineModel, environment);
                    state = stepResult.NextState;
                    isTerminal = stepResult.Done;

                    if (experiences.Count < batchSize)
                        continue;

                    for (int i = 0; i < epochs; i++)
                        OptimizeModel();
                    experiences.Clear();
                }

                if (stopwatch.Elapsed >= maxTimeSpan)
                {
                    isTrainingFinished = true;
                    trainingFinishedReason = "Max training time reached.";
                }

                if(episode >= maxEpisodes)
                {
                    isTrainingFinished = true;
                    trainingFinishedReason = "Max episodes reached.";
                }
            }

            return new NfqResult(onlineModel, trainingFinishedReason);
        }
    }
}
