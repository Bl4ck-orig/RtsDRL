using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReinforcementLearning;
using ReinforcementLearning.Training;

namespace TestProject.Training
{
    [TestClass]
    public class TrainFrozenLakeTest
    {
        [TestMethod]
        public void TrainFrozenLake_CreateQTable_ReturnsQTable()
        {
            QLearningArgs qLearingArgs = new QLearningArgs(new EnvironemntFrozenLake());
            QLearning trainFrozenLake = new QLearning(qLearingArgs);
            int actionSpaceSize = 10;
            int observationSpaceSize = 20;

            var qTable = trainFrozenLake.CreateQTable(actionSpaceSize, observationSpaceSize);

            Assert.AreEqual(10, qTable.GetLength(0));
            Assert.AreEqual(20, qTable.GetLength(1));
        }

        [TestMethod]
        public void TrainFrozenLake_SelectAction_ShouldChooseMaxValueAction_WhenEpsilonIsLow()
        {
            QLearningArgs qLearingArgs = new QLearningArgs(new EnvironemntFrozenLake());
            QLearning trainFrozenLake = new QLearning(qLearingArgs);
            double epsilon = 0.01; // Low epsilon to favor exploitation
            double[,] Q = new double[,] { { 1, 2, 3 }, { 4, 5, 6 } }; // Example Q-table
            int state = 0; // Example state

            // Act
            int selectedAction = trainFrozenLake.SelectAction(state, Q, epsilon);

            // Assert
            Assert.AreEqual(2, selectedAction); // The max value action for state 0 is at index 2
        }

        [TestMethod]
        public void TrainFrozenLake_SelectAction_ShouldChooseRandomAction_WhenEpsilonIsHigh()
        {
            QLearningArgs qLearingArgs = new QLearningArgs(new EnvironemntFrozenLake());
            QLearning trainFrozenLake = new QLearning(qLearingArgs);
            double epsilon = 0.99; // High epsilon to favor exploration
            double[,] Q = new double[,] { { 1, 2, 3 }, { 4, 5, 6 } }; // Example Q-table
            int state = 0; // Example state
            int actionCount = Q.GetLength(1);
            bool[] actionsChosen = new bool[actionCount];

            // Act
            // Run multiple times to increase the chance of choosing different actions
            for (int i = 0; i < 1000; i++)
            {
                int selectedAction = trainFrozenLake.SelectAction(state, Q, epsilon);
                actionsChosen[selectedAction] = true;
            }

            // Assert
            // Verify that multiple different actions were chosen
            for (int i = 0; i < actionCount; i++)
            {
                Assert.IsTrue(actionsChosen[i], $"Action {i} was never chosen");
            }
        }
    }
}
