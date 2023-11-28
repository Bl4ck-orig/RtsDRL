using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReinforcementLearning;
using System;

namespace TestProject.Training
{
    [TestClass]
    public class CommonsTest
    {
        [TestMethod]
        public void Commons_ArgMax_ReturnsIndex()
        {
            float[,] test = new float[,]
            {
                { 1, 2, 0 },
                { 2, -1, 0 },
                { 3, 4, 5 }
            };

            int argMaxRow0 = Commons.ArgMax(test, 0);
            int argMaxRow1 = Commons.ArgMax(test, 1);
            int argMaxRow2 = Commons.ArgMax(test, 2);

            Assert.AreEqual(argMaxRow0, 1);
            Assert.AreEqual(argMaxRow1, 0);
            Assert.AreEqual(argMaxRow2, 2);
        }

        [TestMethod]
        public void Commons_LogSpace_ShouldGenerateCorrectNumberOfElements()
        {
            // Arrange
            double start = 1;
            double stop = 3;
            int num = 10;

            // Act
            var result = Commons.LogSpace(start, stop, num);

            // Assert
            Assert.AreEqual(num, result.Length);
        }

        [TestMethod]
        public void Commons_LogSpace_ShouldHaveCorrectStartAndStopValues()
        {
            // Arrange
            double start = 1;
            double stop = 3;
            int num = 10;
            double @base = 10;

            // Act
            var result = Commons.LogSpace(start, stop, num, @base);

            // Assert
            Assert.AreEqual(Math.Pow(@base, start), result[0], 1e-10);
            Assert.AreEqual(Math.Pow(@base, stop), result[num - 1], 1e-10);
        }

        [TestMethod]
        public void Commons_LogSpace_ShouldGenerateLogarithmicallySpacedValues()
        {
            // Arrange
            double start = 1;
            double stop = 3;
            int num = 5;
            double @base = 10;

            // Act
            var result = Commons.LogSpace(start, stop, num, @base);

            // Assert
            for (int i = 0; i < num - 1; i++)
            {
                // Checking the ratio of consecutive elements is constant
                double ratio = result[i + 1] / result[i];
                Assert.AreEqual(Math.Pow(@base, (stop - start) / (num - 1)), ratio, 1e-10);
            }
        }

        [TestMethod]
        public void Commons_PadArray_ShouldPadWithLastElement()
        {
            // Arrange
            double[] values = { 1.0, 2.0, 3.0 };
            int rem_steps = 2;
            double[] expected = { 1.0, 2.0, 3.0, 3.0, 3.0 };

            // Act
            double[] result = Commons.PadArray(values, rem_steps);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Commons_PadArray_ShouldReturnOriginalArray_WhenRemStepsIsZero()
        {
            // Arrange
            double[] values = { 1.0, 2.0, 3.0 };
            int rem_steps = 0;
            double[] expected = { 1.0, 2.0, 3.0 };

            // Act
            double[] result = Commons.PadArray(values, rem_steps);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Commons_PadArray_ShouldHandleEmptyArray()
        {
            // Arrange
            double[] values = { };
            int rem_steps = 2;
            double[] expected = new double[rem_steps];

            // Act
            double[] result = Commons.PadArray(values, rem_steps);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Commons_TestSubtractScalar()
        {
            double[] array = { 1.0, 2.0, 3.0 };
            double[] result = Commons.SubtractScalar(array, 1.0);

            double[] expected = new double[] { 0.0, 1.0, 2.0 };

            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(expected[i], result[i], 1e-6);
            }
        }

        [TestMethod]
        public void Commons_TestDivideScalar()
        {
            double[] array = { 10.0, 20.0, 30.0 };
            var expected = new double[] { 1.0, 2.0, 3.0 };

            double[] result = Commons.DivideScalar(array, 10.0);

            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(expected[i], result[i], 1e-6);
            }
        }

        [TestMethod]
        public void Commons_TestMultiplyScalar()
        {
            double[] array = { 1.0, 2.0, 3.0 };
            double[] result = Commons.MultiplyScalar(array, 2.0);
            var expected = new double[] { 2.0, 4.0, 6.0 };

            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(expected[i], result[i], 1e-6);
            }
        }

        [TestMethod]
        public void Commons_TestAddScalar()
        {
            double[] array = { 1.0, 2.0, 3.0 };
            double[] result = Commons.AddScalar(array, 1.0);
            var expected = new double[] { 2.0, 3.0, 4.0 };

            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(expected[i], result[i], 1e-6);
            }
        }

        [TestMethod]
        public void Commons_TestImmutabilityOfOriginalArray()
        {
            double[] original = { 1.0, 2.0, 3.0 };
            double[] copy = original.Clone() as double[];
            Commons.AddScalar(copy, 1.0);

            var expected = new double[] { 1.0, 2.0, 3.0 };

            for (int i = 0; i < original.Length; i++)
            {
                Assert.AreEqual(expected[i], original[i], 1e-6);
            }
        }

        [TestMethod]
        public void Commons_GetMaxValueOfRow_ValidRow_ReturnsMaxValue()
        {
            double[,] array = { { 1.5, 2.3, 3.7 }, { 4.2, 5.1, 6.6 }, { 7.0, 8.8, 9.9 } };
            var result = Commons.GetMaxValueOfRow(array, 1);
            Assert.AreEqual(6.6, result);
        }

        [TestMethod]
        public void Commons_GetMaxValueOfRow_FirstRow_ReturnsMaxValue()
        {
            double[,] array = { { 1.5, 2.3, 3.7 }, { 4.2, 5.1, 6.6 }, { 7.0, 8.8, 9.9 } };
            var result = Commons.GetMaxValueOfRow(array, 0);
            Assert.AreEqual(3.7, result);
        }

        [TestMethod]
        public void Commons_GetMaxValueOfRow_LastRow_ReturnsMaxValue()
        {
            double[,] array = { { 1.5, 2.3, 3.7 }, { 4.2, 5.1, 6.6 }, { 7.0, 8.8, 9.9 } };
            var result = Commons.GetMaxValueOfRow(array, 2);
            Assert.AreEqual(9.9, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Commons_GetMaxValueOfRow_NullArray_ThrowsException()
        {
            Commons.GetMaxValueOfRow(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Commons_GetMaxValueOfRow_RowOutOfRange_ThrowsException()
        {
            double[,] array = { { 1.5, 2.3, 3.7 } };
            Commons.GetMaxValueOfRow(array, 3);
        }

        [TestMethod]
        public void Commons_FlattenMax_WithValidInput_ReturnsCorrectMaxValues()
        {
            // Arrange
            double[,] array = {
            { 1.0, 2.0, 3.0 },
            { 6.0, 5.0, 4.0 },
            { 7.0, 8.0, 9.0 }
        };
            double[] expected = { 3.0, 6.0, 9.0 };

            // Act
            double[] actual = Commons.FlattenMax(array);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Commons_FlattenMax_WithEmptyArray_ReturnsEmptyArray()
        {
            // Arrange
            double[,] array = new double[0, 0];
            double[] expected = new double[0];

            // Act
            double[] actual = Commons.FlattenMax(array);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Commons_FlattenMax_WithNullArray_ThrowsArgumentNullException()
        {
            // Act
            Commons.FlattenMax(null);
        }

    }
}
