using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReinforcementLearning.Training;
using System;
using System.Collections.Generic;

namespace TestProject.Training
{
    [TestClass]
    public class QLearningTest
    {
        [TestMethod]
        public void TrainFrozenLake_CreateQTable_ReturnsQTable()
        {
            int actionSpaceSize = 10;
            int observationSpaceSize = 20;

            var qTable = QLearning.CreateQTable(actionSpaceSize, observationSpaceSize);

            Assert.AreEqual(10, qTable.GetLength(0));
            Assert.AreEqual(20, qTable.GetLength(1));
        }

        [TestMethod]
        public void QLearning_SelectAction_ShouldChooseMaxValueAction_WhenEpsilonIsLow()
        {
            Random random = new Random();
            double epsilon = 0.01; // Low epsilon to favor exploitation
            double[,] Q = new double[,] { { 1, 2, 3 }, { 4, 5, 6 } }; // Example Q-table
            int state = 0; // Example state

            // Act
            int selectedAction = QLearning.SelectAction(random, state, Q, epsilon);

            // Assert
            Assert.AreEqual(2, selectedAction); // The max value action for state 0 is at index 2
        }

        [TestMethod]
        public void QLearning_SelectAction_ShouldChooseRandomAction_WhenEpsilonIsHigh()
        {
            Random random = new Random();
            double epsilon = 0.99; // High epsilon to favor exploration
            double[,] Q = new double[,] { { 1, 2, 3 }, { 4, 5, 6 } }; // Example Q-table
            int state = 0; // Example state
            int actionCount = Q.GetLength(1);
            bool[] actionsChosen = new bool[actionCount];

            // Act
            // Run multiple times to increase the chance of choosing different actions
            for (int i = 0; i < 1000; i++)
            {
                int selectedAction = QLearning.SelectAction(random, state, Q, epsilon);
                actionsChosen[selectedAction] = true;
            }

            // Assert
            // Verify that multiple different actions were chosen
            for (int i = 0; i < actionCount; i++)
            {
                Assert.IsTrue(actionsChosen[i], $"Action {i} was never chosen");
            }
        }


        [TestMethod]
        public void GetPolicy_SingleMaxValue_ReturnsCorrectPolicy()
        {
            PrivateType privateType = new PrivateType(typeof(QLearning));
            var Q = new double[,] { { 1, 0 }, { 0, 1 } };
            var expectedPolicy = new Dictionary<int, int> { { 0, 0 }, { 1, 1 } };

            var policy = (Dictionary<int, int>)privateType.InvokeStatic("GetPolicy", Q);

            CollectionAssert.AreEqual(expectedPolicy, policy);
        }

        [TestMethod]
        public void GetPolicy_MultipleMaxValues_ReturnsFirstMax()
        {
            PrivateType privateType = new PrivateType(typeof(QLearning));
            var Q = new double[,] { { 1, 1 }, { 0, 0 } };
            var expectedPolicy = new Dictionary<int, int> { { 0, 0 }, { 1, 0 } };

            var policy = (Dictionary<int, int>)privateType.InvokeStatic("GetPolicy", Q);

            CollectionAssert.AreEqual(expectedPolicy, policy);
        }

        [TestMethod]
        public void GetPolicy_AllZeros_ReturnsZeros()
        {
            PrivateType privateType = new PrivateType(typeof(QLearning));
            var Q = new double[,] { { 0, 0 }, { 0, 0 } };
            var expectedPolicy = new Dictionary<int, int> { { 0, 0 }, { 1, 0 } };

            var policy = (Dictionary<int, int>)privateType.InvokeStatic("GetPolicy", Q);

            CollectionAssert.AreEqual(expectedPolicy, policy);
        }
    }
}
