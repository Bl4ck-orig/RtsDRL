using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReinforcementLearning;
using System;
using System.Collections.Generic;

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
        public void Commons_ArgMax_ValidVector_ReturnsCorrectMaxIndex()
        {
            int[] vector = { 3, 8, 2, 5, 9, 1 };
            int expectedMaxIndex = 4; // The maximum element (9) is at index 4

            int actualMaxIndex = Commons.ArgMax(vector);

            Assert.AreEqual(expectedMaxIndex, actualMaxIndex);
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
        public void Commons_GetMaxValueOfColumn_ValidColumn_ReturnsMaxValue()
        {
            double[,] array = { { 1.5, 2.3, 3.7 }, { 4.2, 5.1, 6.6 }, { 7.0, 8.8, 9.9 } };
            var result = Commons.GetMaxValueOfColumn(array, 1);
            Assert.AreEqual(8.8, result);
        }


        [TestMethod]
        public void Commons_GetMaxValueOfColumn_FirstColumn_ReturnsMaxValue()
        {
            double[,] array = { { 1.5, 2.3, 3.7 }, { 4.2, 5.1, 6.6 }, { 7.0, 8.8, 9.9 } };
            var result = Commons.GetMaxValueOfColumn(array, 0);
            Assert.AreEqual(7.0, result);
        }

        [TestMethod]
        public void Commons_GetMaxValueOfColumn_LastColumn_ReturnsMaxValue()
        {
            double[,] array = { { 1.5, 2.3, 3.7 }, { 4.2, 5.1, 6.6 }, { 7.0, 8.8, 9.9 } };
            var result = Commons.GetMaxValueOfColumn(array, 2);
            Assert.AreEqual(9.9, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Commons_GetMaxValueOfColumn_NullArray_ThrowsException()
        {
            Commons.GetMaxValueOfColumn(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Commons_GetMaxValueOfColumn_ColumnOutOfRange_ThrowsException()
        {
            double[,] array = { { 1.5, 2.3, 3.7 } };
            Commons.GetMaxValueOfColumn(array, 3);
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

        [TestMethod]
        public void Commons_DotProduct_ValidMatrices_ReturnsCorrectProduct()
        {
            double[,] matrix1 = { { 1, 2 }, { 3, 4 } };
            double[,] matrix2 = { { 2, 0 }, { 1, 2 } };
            double[,] expectedProduct = { { 4, 4 }, { 10, 8 } };

            double[,] actualProduct = Commons.DotProduct(matrix1, matrix2);

            CollectionAssert.AreEqual(expectedProduct, actualProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Commons_DotProduct_InvalidDimensions_ThrowsException()
        {
            double[,] matrix1 = { { 1, 2 } }; // 1x2 matrix
            double[,] matrix2 = { { 2 }, { 1 }, { 3 } }; // 3x1 matrix

            var result = Commons.DotProduct(matrix1, matrix2);
        }

        [TestMethod]
        public void Commons_AddVector_ValidMatrices_ReturnsCorrectSum()
        {
            double[,] matrix1 = { { 1, 2 }, { 3, 4 } };
            double[,] vector = { { 1 }, { 2 } };
            double[,] expectedSum = { { 2, 3 }, { 5, 6 } };

            double[,] actualSum = Commons.AddVector(matrix1, vector);

            CollectionAssert.AreEqual(expectedSum, actualSum);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Commons_AddVector_InvalidDimensions_ThrowsException()
        {
            double[,] matrix1 = { { 1, 2 }, { 3, 4 } };
            double[,] vector = { { 1, 2 } };

            var result = Commons.AddVector(matrix1, vector);
        }

        [TestMethod]
        public void Commons_Add_ValidMatrices_ReturnsCorrectSum()
        {
            double[,] matrix1 = { { 1, 2 }, { 3, 4 } };
            double[,] matrix2 = { { 2, 0 }, { 1, 2 } };
            double[,] expectedSum = { { 3, 2 }, { 4, 6 } };

            double[,] actualSum = Commons.Add(matrix1, matrix2);

            CollectionAssert.AreEqual(expectedSum, actualSum);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Commons_Add_InvalidDimensions_ThrowsException()
        {
            double[,] matrix1 = { { 1, 2 } };
            double[,] matrix2 = { { 2, 0, 3 }, { 1, 2, 4 } };

            var result = Commons.Add(matrix1, matrix2);
        }

        [TestMethod]
        public void Commons_Subtract_ValidMatrices_ReturnsCorrectDifference()
        {
            double[,] matrix1 = { { 3, 5 }, { 8, 10 } };
            double[,] matrix2 = { { 1, 2 }, { 3, 4 } };
            double[,] expectedDifference = { { 2, 3 }, { 5, 6 } };

            double[,] actualDifference = Commons.Subtract(matrix1, matrix2);

            CollectionAssert.AreEqual(expectedDifference, actualDifference);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Commons_Subtract_InvalidDimensions_ThrowsException()
        {
            double[,] matrix1 = { { 1, 2 } };
            double[,] matrix2 = { { 1 }, { 2 } };

            var result = Commons.Subtract(matrix1, matrix2);
        }

        [TestMethod]
        public void Commons_Multiply_MatrixWithScalar_ReturnsCorrectProduct()
        {
            double[,] matrix = { { 1, 2 }, { 3, 4 } };
            double scalar = 2;
            double[,] expectedProduct = { { 2, 4 }, { 6, 8 } };

            double[,] actualProduct = Commons.Multiply(matrix, scalar);

            CollectionAssert.AreEqual(expectedProduct, actualProduct);
        }

        [TestMethod]
        public void Commons_Multiply_ElementWise_ValidMatrices_ReturnsCorrectProduct()
        {
            double[,] matrix1 = { { 1, 2 }, { 3, 4 } };
            double[,] matrix2 = { { 5, 6 }, { 7, 8 } };
            double[,] expectedProduct = { { 5, 12 }, { 21, 32 } };

            double[,] actualProduct = Commons.Multiply(matrix1, matrix2);

            CollectionAssert.AreEqual(expectedProduct, actualProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Commons_Multiply_ElementWise_InvalidDimensions_ThrowsException()
        {
            double[,] matrix1 = { { 1, 2 } };
            double[,] matrix2 = { { 1, 2, 3 }, { 4, 5, 6 } };

            var result = Commons.Multiply(matrix1, matrix2);
        }

        [TestMethod]
        public void Commons_Transpose_ValidMatrix_ReturnsCorrectTranspose()
        {
            double[,] matrix = { { 1, 2 }, { 3, 4 } };
            double[,] expectedTranspose = { { 1, 3 }, { 2, 4 } };

            double[,] actualTranspose = Commons.Transpose(matrix);

            CollectionAssert.AreEqual(expectedTranspose, actualTranspose);
        }

        [TestMethod]
        public void Commons_ReverseY_ValidMatrix_ReturnsCorrectlyReversedMatrix()
        {
            int[,] matrix = { { 1, 2 }, { 3, 4 } };
            int[,] expectedReversed = { { 2, 1 }, { 4, 3 } };

            int[,] actualReversed = Commons.ReverseY(matrix);

            CollectionAssert.AreEqual(expectedReversed, actualReversed);
        }

        [TestMethod]
        public void Commons_Unsqueeze_EmptyDoubleArray_ReturnsEmpty2DArray()
        {
            double[] emptyArray = new double[0];
            double[,] expected = new double[0, 1];

            double[,] actual = Commons.Unsqueeze(emptyArray);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Commons_Unsqueeze_SingleElementDoubleArray_ReturnsSingleRow2DArray()
        {
            double[] singleElementArray = { 42.0 };
            double[,] expected = { { 42.0 } };

            double[,] actual = Commons.Unsqueeze(singleElementArray);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Commons_Unsqueeze_MultiElementDoubleArray_ReturnsCorrect2DArray()
        {
            double[] multiElementArray = { 1.0, 2.0, 3.0 };
            double[,] expected = { { 1.0 }, { 2.0 }, { 3.0 } };

            double[,] actual = Commons.Unsqueeze(multiElementArray);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Commons_Unsqueeze_MultiElementIntArray_ReturnsCorrect2DArray()
        {
            int[] multiElementArray = { 1, 2, 3 };
            int[,] expected = { { 1 }, { 2 }, { 3 } };

            int[,] actual = Commons.Unsqueeze(multiElementArray);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Commons_Unsqueeze_StringArray_ReturnsCorrect2DArray()
        {
            string[] stringArray = { "Hello", "World" };
            string[,] expected = { { "Hello" }, { "World" } };

            string[,] actual = Commons.Unsqueeze(stringArray);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Commons_SubtractFromValue_ValidList_ReturnsCorrectlySubtractedValues()
        {
            // Arrange
            double subtractValue = 1.0;
            List<double> inputList = new List<double> { 0.2, 0.5, 0.8 };
            List<double> expected = new List<double> { 0.8, 0.5, 0.2 };

            // Act
            List<double> result = Commons.SubtractFromValue(subtractValue, inputList);

            // Assert
            for(int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i], result[i], 1e-6);
            }
        }

        [TestMethod]
        public void Commons_MultiplyMatrixByArray_ValidInput_ReturnsCorrectlyMultipliedMatrix()
        {
            // Arrange
            double[,] matrix = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            double[] array = { 0.5, 1.0 };
            double[,] expected = { { 0.5, 1.0 }, { 3.0, 4.0 } };

            // Act
            double[,] result = Commons.MultiplyMatrixByArray(matrix, array);

            // Assert
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Assert.AreEqual(expected[i, j], result[i, j], 1e-6, $"Mismatch at position [{i}, {j}]");
                }
            }
        }

        [TestMethod]
        public void Commons_AddMatrixBy_ValidInput_ReturnsCorrectlyAddedMatrix()
        {
            // Arrange
            double[,] matrix = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            double addValue = 1.5;
            double[,] expected = { { 2.5, 3.5 }, { 4.5, 5.5 } };

            // Act
            double[,] result = Commons.AddMatrixBy(matrix, addValue);

            // Assert
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Assert.AreEqual(expected[i, j], result[i, j], 1e-6, $"Mismatch at position [{i}, {j}]");
                }
            }
        }

        [TestMethod]
        public void Commons_AddVectorToMatrix_ValidInput_ReturnsCorrectlyAddedMatrix()
        {
            // Arrange
            double[,] matrix = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            double[] vector = { 1.5, 2.0 };
            double[,] expected = { { 2.5, 4.0 }, { 4.5, 6.0 } };

            // Act
            double[,] result = Commons.AddVectorToMatrix(matrix, vector);

            // Assert
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Assert.AreEqual(expected[i, j], result[i, j], 1e-6, $"Mismatch at position [{i}, {j}]");
                }
            }
        }

        [TestMethod]
        public void Commons_AddVectorToMatrixByRows_ValidInput_ReturnsCorrectlyAddedMatrix()
        {
            // Arrange
            double[,] matrix = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            double[] vector = { 1.5, 2.0 };
            double[,] expected = { { 2.5, 3.5 }, { 5.0, 6.0 } };

            // Act
            double[,] result = Commons.AddVectorToMatrixByRows(matrix, vector);

            // Assert
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Assert.AreEqual(expected[i, j], result[i, j], 1e-6, $"Mismatch at position [{i}, {j}]");
                }
            }
        }

        [TestMethod]
        public void Commons_ToMatrix_ValidList_ReturnsCorrectMatrix()
        {
            List<double[]> list = new List<double[]>
        {
            new double[] { 1.0, 2.0 },
            new double[] { 3.0, 4.0 }
        };
            double[,] expectedMatrix = { { 1.0, 2.0 }, { 3.0, 4.0 } };

            double[,] actualMatrix = Commons.ToMatrix(list);

            CollectionAssert.AreEqual(expectedMatrix, actualMatrix);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Commons_ToMatrix_InvalidList_ThrowsArgumentException()
        {
            List<double[]> list = new List<double[]>
        {
            new double[] { 1.0, 2.0 },
            new double[] { 3.0 }
        };

            double[,] actualMatrix = Commons.ToMatrix(list);
        }

        [TestMethod]
        public void Commons_SubtractVectorFromMatrixColumns_ValidInput_ReturnsCorrectResult()
        {
            // Setup
            double[] vector = { 1, 2 };
            double[,] matrix = { { 3, 5 }, { 4, 6 } };
            double[,] expected = { { 2, 3 }, { 3, 4 } };

            // Act
            double[,] actual = Commons.SubtractVectorFromMatrixColumns(vector, matrix);

            // Assert
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Assert.AreEqual(expected[i, j], actual[i, j], 0.0001, $"Mismatch at element [{i}, {j}]");
                }
            }
        }

        [TestMethod]
        public void Commons_Pow_ValidMatrixAndPower_ReturnsCorrectlyPoweredMatrix()
        {
            // Setup
            double[,] matrix = { { 2, 3 }, { 4, 5 } };
            int power = 2;
            double[,] expected = { { 4, 9 }, { 16, 25 } };

            // Act
            double[,] actual = Commons.Pow(matrix, power);

            // Assert
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Assert.AreEqual(expected[i, j], actual[i, j], 0.0001, $"Mismatch at element [{i}, {j}]");
                }
            }
        }
    }
}
