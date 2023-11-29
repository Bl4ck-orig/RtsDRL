using System;
using System.Linq;

namespace ReinforcementLearning
{
    public class Fcq
    {
        private int inputSize;
        private int outputSize;
        private int batchSize;

        private double[,] hiddenWeights;
        private double[,] hiddenBias;
        private double[,] outputWeights;
        private double[,] outputBias;

        private int hiddenLayerNodesAmount;
        private double learningRate = 0.1f;

        private double[,] inputLayer;

        private double[,] hiddenLayer;
        private double[,] hiddenLayerZ;

        private double[,] outputLayer;
        private double[,] outputLayerZ;

        private double[,] featureMatrix;
        private double[,] rewardMatrix;
        private double oneOverBatchSize;

        private double[,] changeInOutputLayerError;
        private double[,] changeInOutputWeights;
        private double[,] changeInOutputBias;

        private double[,] changeInHiddenLayerError;
        private double[,] changeInHiddenLayerWeights;
        private double[,] changeInHiddenBias;

        private int yieldAt = 100;
        private int lastPredictionVectorIndex = 0;
        private Random prng;

        public Fcq(int _inputSize, int _outputSize, int _batchSize, int _seed = -1)
        {
            inputSize = _inputSize;
            outputSize = _outputSize;
            batchSize = _batchSize;
            oneOverBatchSize = (double)1 / batchSize;

            inputLayer = new double[inputSize, batchSize];

            hiddenLayerNodesAmount = outputSize;
            hiddenLayer = new double[hiddenLayerNodesAmount, batchSize];
            hiddenWeights = new double[hiddenLayerNodesAmount, inputSize];
            hiddenBias = new double[hiddenLayerNodesAmount, 1];

            outputLayer = new double[outputSize, batchSize];
            outputWeights = new double[outputSize, hiddenLayerNodesAmount];
            outputBias = new double[outputSize, 1];

            prng = _seed == -1 ? new Random() : new Random(_seed);

            SetUpRandomNetwork();
        }

        private void SetUpRandomNetwork()
        {
            for (int x = 0; x < hiddenWeights.GetLength(0); x++)
            {
                for (int y = 0; y < hiddenWeights.GetLength(1); y++)
                {
                    hiddenWeights[x, y] = prng.NextDouble() - 0.5f;
                }
            }

            for (int x = 0; x < hiddenBias.GetLength(0); x++)
            {
                for (int y = 0; y < hiddenBias.GetLength(1); y++)
                {
                    hiddenBias[x, y] = prng.NextDouble() - 0.5f;
                }
            }

            for (int x = 0; x < outputWeights.GetLength(0); x++)
            {
                for (int y = 0; y < outputWeights.GetLength(1); y++)
                {
                    outputWeights[x, y] = prng.NextDouble() - 0.5f;
                }
            }

            for (int x = 0; x < outputBias.GetLength(0); x++)
            {
                for (int y = 0; y < outputBias.GetLength(1); y++)
                {
                    outputBias[x, y] = prng.NextDouble() - 0.5f;
                }
            }
        }

        #region Forward Propagation -----------------------------------------------------------------
        public void ApplyForwardPropagation()
        {
            ForwardPropagateHiddenLayer();
            ForwardPropagateOutputLayer();
        }

        private void ForwardPropagateHiddenLayer()
        {
            hiddenLayerZ = hiddenWeights.DotProduct(inputLayer).AddVector(hiddenBias);

            for (int x = 0; x < hiddenLayer.GetLength(0); x++)
            {
                for (int y = 0; y < hiddenLayer.GetLength(1); y++)
                {
                    hiddenLayer[x, y] = Relu(hiddenLayerZ[x, y]);
                }
            }
        }

        private double Relu(double _value) => Math.Max(0, _value);

        private void ForwardPropagateOutputLayer()
        {
            outputLayerZ = outputWeights.DotProduct(hiddenLayer).AddVector(outputBias);
            // Cloning for Linear regression
            outputLayer = outputLayerZ.Clone() as double[,];
        }
        #endregion -----------------------------------------------------------------

        #region Getting Prediction -----------------------------------------------------------------
        public int GetPrediction(double[,] _inputs)
        {
            inputLayer = _inputs.Clone() as double[,];

            ApplyForwardPropagation();
            return GetHighestRewardOutputIndex();
        }

        public int GetPrediction(double[] _inputs)
        {
            for (int x = 0; x < inputLayer.GetLength(0); x++)
            {
                for (int y = 0; y < inputLayer.GetLength(1); y++)
                {
                    inputLayer[x, y] = _inputs[x];
                }
            }

            ApplyForwardPropagation();
            return GetHighestRewardOutputIndex();
        }


        private int GetHighestRewardOutputIndex()
        {
            double max = double.MinValue;
            int index = 0;

            for (int x = 0; x < outputLayer.GetLength(0); x++)
            {
                for (int y = 0; y < outputLayer.GetLength(1); y++)
                {
                    if (outputLayer[x, y] < max)
                        continue;

                    max = outputLayer[x, y];
                    index = x;
                    lastPredictionVectorIndex = y;
                }
            }

            return index;
        }

        public double[] GetRewardsFromLastPrediction()
        {
            double[] rewardVector = new double[outputLayer.GetLength(0)];

            for (int x = 0; x < outputLayer.GetLength(0); x++)
            {
                rewardVector[x] = outputLayer[x, lastPredictionVectorIndex];
            }

            return rewardVector;
        }
        #endregion -----------------------------------------------------------------

        #region Training -----------------------------------------------------------------
        public void TrainModel(double[,] _featureMatrix, 
            double[,] _rewardMatrix,
            int _iterations, 
            float _learningRate)
        {
            featureMatrix = _featureMatrix;
            inputLayer = _featureMatrix.Clone() as double[,];
            rewardMatrix = _rewardMatrix;
            learningRate = _learningRate;

            Console.WriteLine("Starting training of " + _iterations + " iterations");

            for (int i = 0; i < _iterations; i++)
            {
                ApplyForwardPropagation();
                ApplyBackPropagation();
                AdjustWeightsAndBiases();

                if (i % yieldAt == 0)
                    Dialogue.PrintProgress(i, _iterations, i == 0);
            }

            Console.WriteLine("Finished with accuracy of = " + GetAccuracy());
        }

        private void ApplyBackPropagation()
        {
            BackPropagateOutputLayer();
            BackPropagateHiddenLayer();
        }

        private void BackPropagateOutputLayer()
        {
            BackPropagateOutputLayerError();

            BackPropagateOutputLayerWeights();

            BackPropagateOutputLayerBias();
        }

        private void BackPropagateOutputLayerError()
        {
            changeInOutputLayerError = outputLayer.Subtract(rewardMatrix);
        }

        private void BackPropagateOutputLayerWeights()
        {
            changeInOutputWeights = changeInOutputLayerError.DotProduct(hiddenLayer.Transpose());
            changeInOutputWeights = changeInOutputWeights.Multiply(oneOverBatchSize);
        }

        private void BackPropagateOutputLayerBias()
        {
            changeInOutputBias = new double[outputBias.GetLength(0), outputBias.GetLength(1)];
            for (int i = 0; i < outputBias.GetLength(0); i++)
                changeInOutputBias[i, 0] = oneOverBatchSize *
                    Enumerable.Range(0, changeInOutputLayerError.GetLength(0))
                        .Select(x => changeInOutputLayerError[x, i]).Sum();
        }

        private void BackPropagateHiddenLayer()
        {
            BackPropagateHiddenLayerError();

            BackPropagateHiddenLayerWeights();

            BackPropagateHiddenLayerBias();
        }

        private void BackPropagateHiddenLayerError()
        {
            changeInHiddenLayerError = outputWeights.Transpose().DotProduct(changeInOutputLayerError);

            double[,] derivedReluHiddenlayer = GetDerivedReluHiddenlayer();

            changeInHiddenLayerError = changeInHiddenLayerError.Multiply(derivedReluHiddenlayer);
        }

        private double[,] GetDerivedReluHiddenlayer()
        {
            int width = changeInHiddenLayerError.GetLength(0);
            int height = changeInHiddenLayerError.GetLength(1);

            double[,] derivedReluHiddenlayer = new double[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    derivedReluHiddenlayer[x, y] = ReluDerivative(hiddenLayerZ[x, y]);
                }
            }
            return derivedReluHiddenlayer;
        }

        private void BackPropagateHiddenLayerWeights()
        {
            changeInHiddenLayerWeights = changeInHiddenLayerError.DotProduct(featureMatrix.Transpose());
            changeInHiddenLayerWeights = changeInHiddenLayerWeights.Multiply(oneOverBatchSize);
        }

        private void BackPropagateHiddenLayerBias()
        {
            changeInHiddenBias = new double[hiddenBias.GetLength(0), hiddenBias.GetLength(1)];
            for (int i = 0; i < hiddenBias.GetLength(0); i++)
            {
                changeInHiddenBias[i, 0] = oneOverBatchSize *
                    Enumerable.Range(0, changeInHiddenLayerError.GetLength(0))
                        .Select(x => changeInHiddenLayerError[x, i]).Sum();
            }
        }

        private double ReluDerivative(double _value) => (_value < 0) ? 0f : 1f;

        private void AdjustWeightsAndBiases()
        {
            hiddenWeights = hiddenWeights.Subtract(changeInHiddenLayerWeights.Multiply(learningRate));
            hiddenBias = hiddenBias.Subtract(changeInHiddenBias.Multiply(learningRate));
            outputWeights = outputWeights.Subtract(changeInOutputWeights.Multiply(learningRate));
            outputBias = outputBias.Subtract(changeInOutputBias.Multiply(learningRate));
        }

        private double GetAccuracy()
        {
            int correctPredictions = 0;

            for (int y = 0; y < outputLayer.GetLength(1); y++)
            {
                int correctIndex = 0;
                double rewardMatrixMax = double.MinValue;
                int predictionIndex = 0;
                double predictionMatrixMax = double.MinValue;

                for (int x = 0; x < outputLayer.GetLength(0); x++)
                {
                    if (outputLayer[x, y] > predictionMatrixMax)
                    {
                        predictionMatrixMax = outputLayer[x, y];
                        predictionIndex = x;
                    }

                    if (rewardMatrix[x, y] > rewardMatrixMax)
                    {
                        rewardMatrixMax = rewardMatrix[x, y];
                        correctIndex = x;
                    }
                }

                if (correctIndex == predictionIndex)
                    correctPredictions++;
            }
            return (double)correctPredictions / batchSize;
        }
        #endregion -----------------------------------------------------------------
    }
}
