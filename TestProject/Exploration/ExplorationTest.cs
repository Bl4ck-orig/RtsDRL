using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReinforcementLearning;
using System;

namespace TestProject.Training
{
    [TestClass]
    public class ExplorationTest
    {
        [TestMethod]
        public void Exploration_DecaySchedule_TestFirstHundredValues()
        {
            double[] result = Exploration.DecaySchedule(1.0, 0.1, 1.0, 100);

            double[] expected_1_Point1_1_100 = new double[]
        {
             1.0,        0.95868042, 0.91923887, 0.88159,    0.84565233, 0.81134809,
             0.77860303, 0.74734627, 0.71751019, 0.6890302,  0.66184468, 0.63589477,
             0.61112433, 0.58747975, 0.56490984, 0.54336578 ,0.52280092, 0.50317077,
             0.48443284, 0.46654658, 0.44947328, 0.43317598 ,0.41761942, 0.40276994,
             0.38859538, 0.37506508, 0.36214975, 0.34982144 ,0.33805348, 0.32682038,
             0.31609785, 0.30586267, 0.2960927 , 0.28676679 ,0.27786476, 0.26936733,
             0.26125613, 0.25351359, 0.24612297, 0.23906826 ,0.23233419, 0.22590621,
             0.21977038, 0.21391343, 0.2083227 , 0.20298607 ,0.197892  , 0.19302946,
             0.18838793, 0.18395737, 0.17972818, 0.17569121 ,0.17183774, 0.1681594,
             0.16464826, 0.1612967 , 0.15809747, 0.15504366 ,0.15212864, 0.14934612,
             0.14669007, 0.14415473, 0.14173464, 0.13942454 ,0.13721944, 0.13511456,
             0.13310535, 0.13118747, 0.12935675, 0.12760925 ,0.12594117, 0.12434891,
             0.12282902, 0.12137821, 0.11999334, 0.11867141 ,0.11740957, 0.11620509,
             0.11505534, 0.11395786, 0.11291026, 0.11191027 ,0.11095573, 0.11004458,
             0.10917485, 0.10834464, 0.10755217, 0.10679571 ,0.10607364, 0.10538439,
             0.10472646, 0.10409844, 0.10349897, 0.10292674 ,0.10238052, 0.10185912,
             0.10136143, 0.10088635, 0.10043287, 0.1
        };

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i], expected_1_Point1_1_100[i], 1e-5);
            }
        }

        [TestMethod]
        public void Exploration_DecaySchedule_TestLengthOfOutput()
        {
            double[] result = Exploration.DecaySchedule(1.0, 0.1, 0.9, 100);
            Assert.AreEqual(100, result.Length);
        }

        [TestMethod]
        public void Exploration_DecaySchedule_TestDecayPattern()
        {
            double[] result = Exploration.DecaySchedule(1.0, 0.1, 0.9, 100);
            for (int i = 1; i < result.Length; i++)
            {
                Assert.IsTrue(result[i] <= result[i - 1]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Exploration_DecaySchedule_TestNegativeMaxSteps()
        {
            Exploration.DecaySchedule(1.0, 0.1, 0.9, -1);
        }
    }
}