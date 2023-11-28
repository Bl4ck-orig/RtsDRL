using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReinforcementLearning;
using System.Collections.Generic;

namespace TestProject.Environment
{

    [TestClass]
    public class EnvironmentTest
    {
        [TestMethod]
        public void Action_Constructor_CheckProbabilitiesOrder()
        {
            List<(double, int)> transitionProbabilities = new List<(double, int)>()
            {
                (0.4f, 0),
                (0.5f, 1),
                (0.1f, 2),
            };

            Dictionary<int, double> transitionRewards = new Dictionary<int, double>()
            {
                { 0, 0f },
                { 1, 0f },
                { 2, 0f }
            };

            List<(double, int)> expected = new List<(double, int)>()
            {
                (0.1f, 2),
                (0.4f, 0),
                (0.5f, 1),
            };

            StepAction stepAction = new StepAction(transitionProbabilities, transitionRewards);
            PrivateObject privateStepAction = new PrivateObject(stepAction);

            List<(double, int)> result = (List<(double, int)>)privateStepAction.GetField("transitionProbabilities");

            for(int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Item1, result[i].Item1);
            }
        }

        [TestMethod]
        public void Action_GetNextState_CheckProbabilities()
        {
            var transitionProbabilities = new List<(double, int)>
        {
            (0.2, 1),  // 20% chance for state 1
            (0.3, 2),  // 30% chance for state 2
            (0.5, 3)   // 50% chance for state 3
        };

            PrivateType privateType = new PrivateType(typeof(StepAction));

            // Test different scenarios with different _random values

            // Act & Assert for _random = 0.1 (should fall in the first bucket, 20%)
            int result = (int)privateType.InvokeStatic("GetNextState", transitionProbabilities, 0.1);
            Assert.AreEqual(1, result);

            // Act & Assert for _random = 0.25 (should fall in the second bucket, 30%)
            result = (int)privateType.InvokeStatic("GetNextState", transitionProbabilities, 0.25);
            Assert.AreEqual(2, result);

            // Act & Assert for _random = 0.6 (should fall in the third bucket, 50%)
            result = (int)privateType.InvokeStatic("GetNextState", transitionProbabilities, 0.6);
            Assert.AreEqual(3, result);
        }
    }
}

