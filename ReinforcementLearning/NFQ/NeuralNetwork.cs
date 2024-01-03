using System;
using System.Linq;

namespace ReinforcementLearning
{
    public class NeuralNetwork : IFcq
    {
        private int inputSize;
        private int outputSize;
        private int batchSize;

        private double[,] hiddenWeights;
        private double[,] hiddenBias;

        private double[,] outputWeights;
        private double[,] outputBias;

        private int hiddenLayerNodesAmount;
        private double learningRate = 0.0001f;

        private double[,] inputLayer;

        private double[,] hiddenLayer;
        private double[,] hiddenLayerZ;

        private double[,] outputLayer;
        private double[,] outputLayerZ;

        private double[,] errorMatrix;
        private double oneOverBatchSize;

        private double[,] changeInOutputLayerError;
        private double[,] changeInOutputWeights;
        private double[,] changeInOutputBias;

        private double[,] changeInHiddenLayerError;
        private double[,] changeInHiddenLayerWeights;
        private double[,] changeInHiddenBias;

        private double gradientClippingThreshold = 0f;

        public NeuralNetwork(int _inputSize, int _outputSize, int _hiddenLayerSize, int _batchSize, double _gradientClippingThreshold, Random _prng)
        {
            inputSize = _inputSize;
            outputSize = _outputSize;
            batchSize = _batchSize;
            oneOverBatchSize = (double)1 / batchSize;

            inputLayer = new double[inputSize, batchSize];

            hiddenLayerNodesAmount = _hiddenLayerSize;
            hiddenLayer = new double[hiddenLayerNodesAmount, batchSize];
            hiddenWeights = new double[hiddenLayerNodesAmount, inputSize];
            hiddenBias = new double[hiddenLayerNodesAmount, 1];

            outputLayer = new double[outputSize, batchSize];
            outputWeights = new double[outputSize, hiddenLayerNodesAmount];
            outputBias = new double[outputSize, 1];

            gradientClippingThreshold = _gradientClippingThreshold;

            SetUpNetworkXavier(_prng);
        }

        private void SetUpRandomNetwork(Random _prng)
        {
            for (int x = 0; x < hiddenWeights.GetLength(0); x++)
            {
                for (int y = 0; y < hiddenWeights.GetLength(1); y++)
                {
                    hiddenWeights[x, y] = _prng.NextDouble() - 0.5f;
                }
            }

            for (int x = 0; x < hiddenBias.GetLength(0); x++)
            {
                for (int y = 0; y < hiddenBias.GetLength(1); y++)
                {
                    hiddenBias[x, y] = _prng.NextDouble() - 0.5f;
                }
            }

            for (int x = 0; x < outputWeights.GetLength(0); x++)
            {
                for (int y = 0; y < outputWeights.GetLength(1); y++)
                {
                    outputWeights[x, y] = _prng.NextDouble() - 0.5f;
                }
            }

            for (int x = 0; x < outputBias.GetLength(0); x++)
            {
                for (int y = 0; y < outputBias.GetLength(1); y++)
                {
                    outputBias[x, y] = _prng.NextDouble() - 0.5f;
                }
            }
        }

        private void SetUpNetworkXavier(Random _prng)
        {
            for (int x = 0; x < hiddenWeights.GetLength(0); x++)
            {
                for (int y = 0; y < hiddenWeights.GetLength(1); y++)
                {
                    double distribution = Math.Sqrt((double)6 / (hiddenLayerNodesAmount + inputSize));
                    hiddenWeights[x, y] = _prng.NextDouble() * 2 * distribution - distribution;
                }
            }

            for (int x = 0; x < outputWeights.GetLength(0); x++)
            {
                for (int y = 0; y < outputWeights.GetLength(1); y++)
                {
                    double distribution = Math.Sqrt((double)6 / (hiddenLayerNodesAmount + outputSize));
                    outputWeights[x, y] = _prng.NextDouble() * 2 * distribution - distribution;
                }
            }
        }

        public NeuralNetwork(TrainingResult _trainingResult)
        {
            inputSize = _trainingResult.inputSize;
            outputSize= _trainingResult.outputSize;
            batchSize= _trainingResult.batchSize;
            hiddenWeights= _trainingResult.hiddenWeights;
            hiddenBias= _trainingResult.hiddenBias;
            outputWeights= _trainingResult.outputWeights;
            outputBias= _trainingResult.outputBias;
            hiddenLayerNodesAmount= _trainingResult.hiddenLayerNodesAmount;
            learningRate= _trainingResult.learningRate;
            inputLayer= _trainingResult.inputLayer;
            hiddenLayer= _trainingResult.hiddenLayer;
            hiddenLayerZ= _trainingResult.hiddenLayerZ;
            outputLayer= _trainingResult.outputLayer;
            outputLayerZ= _trainingResult.outputLayerZ;
            errorMatrix= _trainingResult.errorMatrix;
            oneOverBatchSize= _trainingResult.oneOverBatchSize;
            changeInOutputLayerError= _trainingResult.changeInOutputLayerError;
            changeInOutputWeights= _trainingResult.changeInOutputWeights;
            changeInOutputBias= _trainingResult.changeInOutputBias;
            changeInHiddenLayerError= _trainingResult.changeInHiddenLayerError;
            changeInHiddenLayerWeights= _trainingResult.changeInHiddenLayerWeights;
            changeInHiddenBias= _trainingResult.changeInHiddenBias;
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
            outputLayer = Commons.SoftMax(outputLayerZ);
            // Cloning for Linear regression
            //outputLayer = outputLayerZ.Clone() as double[,];
        }
        #endregion -----------------------------------------------------------------

        #region Getting Prediction -----------------------------------------------------------------
        public double[,] GetOutputMatrix(double[,] _inputs)
        {
            SetInputLayerByMatrix(_inputs);
            ApplyForwardPropagation();
            return outputLayer.Clone() as double[,];
        }

        public double[] GetHighestRewardVector(double[,] _inputs)
        {
            SetInputLayerByMatrix(_inputs);
            ApplyForwardPropagation();
            return GetHighestRewardVector();
        }

        private void SetInputLayerByMatrix(double[,] _inputs)
        {
            if (_inputs.GetLength(0) != inputLayer.GetLength(0))
                throw new ArgumentException("Wrong dimension of input matrix");

            if (_inputs.GetLength(1) != inputLayer.GetLength(1))
                throw new ArgumentException("Wrong dimension of input matrix");

            inputLayer = _inputs.Clone() as double[,];
        }

        public double[] GetPrediction(double[] _inputs)
        {
            for (int x = 0; x < inputLayer.GetLength(0); x++)
            {
                for (int y = 0; y < inputLayer.GetLength(1); y++)
                {
                    inputLayer[x, y] = _inputs[x];
                }
            }

            ApplyForwardPropagation();
            return GetHighestRewardRow();
        }

        private double[] GetHighestRewardVector()
        {
            double[] maxQValues = new double[batchSize];

            for (int i = 0; i < batchSize; i++)
            {
                double maxQValue = Double.MinValue;
                for (int j = 0; j < outputSize; j++)
                {
                    if (outputLayer[j, i] > maxQValue)
                    {
                        maxQValue = outputLayer[j, i];
                    }
                }
                maxQValues[i] = maxQValue;
            }

            return maxQValues;
        }


        private double[] GetHighestRewardRow()
        {
            double maxMean = double.MinValue;
            double[] maxRow = new double[outputSize];

            for (int y = 0; y < outputLayer.GetLength(1); y++)
            {
                double[] row = new double[outputSize];

                for (int x = 0; x < outputSize; x++)
                {
                    row[x] = outputLayer[x, y];
                }

                double mean = Mean(row);
                if (mean < maxMean)
                    continue;

                maxMean = mean;
                maxRow = row;
            }

            return maxRow;
        }

        private double Mean(double[] _vector) => _vector.Sum() / _vector.Length;

        #endregion -----------------------------------------------------------------

        #region Training -----------------------------------------------------------------
        public void Backwards(double[,] _errorMatrix,
            double _learningRate)
        {
            errorMatrix = _errorMatrix;
            learningRate = _learningRate;

            ApplyBackPropagation();
        }

        public void ApplyBackPropagation()
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
            changeInOutputLayerError = outputLayer.Subtract(errorMatrix);
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
            changeInHiddenLayerWeights = changeInHiddenLayerError.DotProduct(inputLayer.Transpose());
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

        public void AdjustWeightsAndBiases()
        {
            GradientClipping();

            hiddenWeights = hiddenWeights.Subtract(changeInHiddenLayerWeights.Multiply(learningRate));
            hiddenBias = hiddenBias.Subtract(changeInHiddenBias.Multiply(learningRate));
            outputWeights = outputWeights.Subtract(changeInOutputWeights.Multiply(learningRate));
            outputBias = outputBias.Subtract(changeInOutputBias.Multiply(learningRate));
        }

        private void GradientClipping()
        {
            int amountOfElements = changeInHiddenLayerWeights.Length + changeInHiddenBias.Length +
                changeInOutputWeights.Length + changeInOutputBias.Length;

            double gradientMagnitude = 0f;

            gradientMagnitude += Commons.EuclideanSum(changeInHiddenLayerWeights);
            gradientMagnitude += Commons.EuclideanSum(changeInHiddenBias);
            gradientMagnitude += Commons.EuclideanSum(changeInOutputWeights);
            gradientMagnitude += Commons.EuclideanSum(changeInOutputBias);

            gradientMagnitude = Math.Sqrt(gradientMagnitude);

            if (Double.IsNaN(gradientMagnitude))
                throw new ArgumentOutOfRangeException();

            if (gradientMagnitude < gradientClippingThreshold)
                return;

            double changeOfEachElement = gradientClippingThreshold / gradientMagnitude;

            changeInHiddenLayerWeights.Multiply(changeOfEachElement);
            changeInHiddenBias.Multiply(changeOfEachElement);
            changeInOutputWeights.Multiply(changeOfEachElement);
            changeInOutputBias.Multiply(changeOfEachElement);
        }





        public bool HasNan()
        {
            return Double.IsNaN(hiddenWeights[0, 0]);
        }

        public NeuralNetworkValues GetNeuralNetworkValues()
        {
            return new NeuralNetworkValues(inputSize,
                outputSize,
                batchSize,
                hiddenWeights,
                hiddenBias,
                outputWeights,
                outputBias,
                hiddenLayerNodesAmount,
                learningRate,
                inputLayer,
                hiddenLayer,
                hiddenLayerZ,
                outputLayer,
                outputLayerZ,
                errorMatrix,
                oneOverBatchSize,
                changeInOutputLayerError,
                changeInOutputWeights,
                changeInOutputBias,
                changeInHiddenLayerError,
                changeInHiddenLayerWeights,
                changeInHiddenBias);
        }
        #endregion -----------------------------------------------------------------
    }
}
