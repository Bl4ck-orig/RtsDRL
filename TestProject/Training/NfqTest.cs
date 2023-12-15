using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReinforcementLearning;
using System.Reflection;

namespace TestProject.Training
{
    [TestClass]
    public class NfqTest
    {
        private FieldInfo GetPrivateField<T>(object obj, string fieldName)
        {
            var type = typeof(T);
            return type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private T GetPrivateFieldValue<T>(object obj, string fieldName)
        {
            FieldInfo field = GetPrivateField<NeuralNetwork>(obj, fieldName);
            return (T)field.GetValue(obj);
        }

        [TestMethod]
        public void Fcq_Constructor_InitializesMatricesWithCorrectDimensions()
        {
            int inputSize = 10;
            int outputSize = 5;
            int batchSize = 3;

            var fcq = new NeuralNetwork(inputSize, outputSize, outputSize, batchSize, new System.Random());

            double[,] inputLayer = GetPrivateFieldValue<double[,]>(fcq, "inputLayer");
            double[,] hiddenWeights = GetPrivateFieldValue<double[,]>(fcq, "hiddenWeights");
            double[,] hiddenBias = GetPrivateFieldValue<double[,]>(fcq, "hiddenBias");
            double[,] outputLayer = GetPrivateFieldValue<double[,]>(fcq, "outputLayer");
            double[,] outputWeights = GetPrivateFieldValue<double[,]>(fcq, "outputWeights");
            double[,] outputBias = GetPrivateFieldValue<double[,]>(fcq, "outputBias");

            Assert.AreEqual(inputLayer.GetLength(0), inputSize);
            Assert.AreEqual(inputLayer.GetLength(1), batchSize);

            Assert.AreEqual(hiddenWeights.GetLength(0), outputSize);
            Assert.AreEqual(hiddenWeights.GetLength(1), inputSize);
            Assert.AreEqual(hiddenBias.GetLength(0), outputSize);
            Assert.AreEqual(hiddenBias.GetLength(1), 1);

            Assert.AreEqual(outputLayer.GetLength(0), outputSize);
            Assert.AreEqual(outputLayer.GetLength(1), batchSize);
            Assert.AreEqual(outputWeights.GetLength(0), outputSize);
            Assert.AreEqual(outputWeights.GetLength(1), outputSize);
            Assert.AreEqual(outputBias.GetLength(0), outputSize);
            Assert.AreEqual(outputBias.GetLength(1), 1);
        }
    }
}
