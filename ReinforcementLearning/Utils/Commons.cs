using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace ReinforcementLearning
{
    public static class Commons
    {
        public static double[] FlattenMax(double[,] _array)
        {
            if (_array == null)
                throw new ArgumentNullException();

            double[] flattendMax = new double[_array.GetLength(0)];

            for(int i = 0; i < flattendMax.Length; i++)
            {
                flattendMax[i] = _array[i,ArgMax(_array, i)];
            }

            return flattendMax;
        }

        public static int ArgMax<T>(T[] vector) where T : IComparable
        {
            if (vector == null || vector.Length == 0)
            {
                throw new ArgumentException("Vector cannot be null or empty.");
            }

            int maxIndex = 0;
            for (int i = 1; i < vector.Length; i++)
            {
                if (vector[i].CompareTo(vector[maxIndex]) > 0)
                {
                    maxIndex = i;
                }
            }
            return maxIndex;
        }

        public static int ArgMax<T>(T[,] _array, int _row) where T : IComparable
        {
            int maxIndex = 0;
            T maxValue = _array[_row, 0];
            for (int i = 1; i < _array.GetLength(1); i++)
            {
                if (_array[_row, i].CompareTo(maxValue) > 0)
                {
                    maxValue = _array[_row, i];
                    maxIndex = i;
                }
            }
            return maxIndex;
        }

        public static double[] LogSpace(double _start, double _stop, int _num, double _base = 10.0, bool _endpoint = true)
        {
            if (_num < 2)
            {
                throw new ArgumentException("_num must be greater than or equal to 2.");
            }

            double[] result = new double[_num];
            double scale = _endpoint ? (_stop - _start) / (_num - 1) : (_stop - _start) / _num;

            for (int i = 0; i < _num; i++)
            {
                result[i] = Math.Pow(_base, _start + scale * i);
            }

            if (!_endpoint && _num > 1)
            {
                result[_num - 1] = Math.Pow(_base, _stop);
            }

            return result;
        }

        public static double[] PadArray(double[] _values, int _remSteps)
        {
            int newSize = _values.Length + _remSteps;
            double[] paddedArray = new double[newSize];

            for (int i = 0; i < _values.Length; i++)
            {
                paddedArray[i] = _values[i];
            }

            if (_values.Length == 0)
                return paddedArray;

            double edgeValue = _values[_values.Length - 1];
            for (int i = _values.Length; i < newSize; i++)
            {
                paddedArray[i] = edgeValue;
            }

            return paddedArray;
        }


        public static double[] SubtractScalar(double[] _array, double _scalar)
        {
            double[] copy = _array.Clone() as double[];
            for (int i = 0; i < copy.Length; i++)
                copy[i] -= _scalar;
            return copy;
        }

        public static double[] DivideScalar(double[] _array, double _scalar)
        {
            if (_scalar == 0)
                throw new DivideByZeroException();

            double[] copy = _array.Clone() as double[];
            for (int i = 0; i < copy.Length; i++)
                copy[i] /= _scalar;
            return copy;
        }

        public static double[] MultiplyScalar(double[] _array, double _scalar)
        {
            double[] copy = _array.Clone() as double[];
            for (int i = 0; i < copy.Length; i++)
                copy[i] *= _scalar;
            return copy;
        }

        public static double[] AddScalar(double[] _array, double _scalar)
        {
            double[] copy = _array.Clone() as double[];
            for (int i = 0; i < copy.Length; i++)
                copy[i] += _scalar;
            return copy;
        }

        public static double GetMaxValueOfRow(double[,] _array, int _row)
        {
            if (_array == null)
            {
                throw new ArgumentNullException(nameof(_array));
            }

            if (_row < 0 || _row >= _array.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(nameof(_row));
            }

            double maxValue = double.MinValue;
            int columns = _array.GetLength(1);

            for (int i = 0; i < columns; i++)
            {
                if (_array[_row, i] > maxValue)
                {
                    maxValue = _array[_row, i];
                }
            }

            return maxValue;
        }

        public static double GetMaxValueOfColumn(double[,] _array, int _column)
        {
            if (_array == null)
            {
                throw new ArgumentNullException(nameof(_array));
            }

            if (_column < 0 || _column >= _array.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(_column));
            }

            double maxValue = double.MinValue;
            int rows = _array.GetLength(0);

            for (int i = 0; i < rows; i++)
            {
                if (_array[i, _column] > maxValue)
                {
                    maxValue = _array[i, _column];
                }
            }

            return maxValue;
        }

        public static double[,] DotProduct(this double[,] _matrix1, double[,] _matrix2)
        {
            var matrix1Rows = _matrix1.GetLength(0);
            var matrix1Cols = _matrix1.GetLength(1);
            var matrix2Rows = _matrix2.GetLength(0);
            var matrix2Cols = _matrix2.GetLength(1);

            if (matrix1Cols != matrix2Rows)
                throw new InvalidOperationException
                  ("Product is undefined. n columns of first matrix must equal to n rows of second matrix");

            double[,] product = new double[matrix1Rows, matrix2Cols];

            for (int matrix1_row = 0; matrix1_row < matrix1Rows; matrix1_row++)
            {
                for (int matrix2_col = 0; matrix2_col < matrix2Cols; matrix2_col++)
                {
                    for (int matrix1_col = 0; matrix1_col < matrix1Cols; matrix1_col++)
                    {
                        product[matrix1_row, matrix2_col] +=
                          _matrix1[matrix1_row, matrix1_col] *
                          _matrix2[matrix1_col, matrix2_col];
                    }
                }
            }

            return product;
        }

        public static double[,] AddVector(this double[,] _matrix1, double[,] _matrix2)
        {
            var matrix1Rows = _matrix1.GetLength(0);
            var matrix1Cols = _matrix1.GetLength(1);
            var matrix2Rows = _matrix2.GetLength(0);

            if (matrix1Rows != matrix2Rows)
                throw new InvalidOperationException
                  ("Product is undefined. n columns of first matrix must equal to n rows of second matrix");

            double[,] product = new double[matrix1Rows, matrix1Cols];

            for (int x = 0; x < matrix1Rows; x++)
            {
                for (int y = 0; y < matrix1Cols; y++)
                {
                    product[x, y] = _matrix1[x, y] + _matrix2[x, 0];
                }
            }

            return product;
        }

        public static double[,] Add(this double[,] _matrix, double[,] _by)
        {
            int _matrixWidth = _matrix.GetLength(0);
            int _matrixHeight = _matrix.GetLength(1);

            int _byWidth = _by.GetLength(0);
            int _byHeight = _by.GetLength(1);

            if (_matrixWidth != _byWidth ||
                _matrixHeight != _byHeight)
                throw new ArgumentException("Matrix multiplication not possible");

            double[,] c = new double[_matrixWidth, _matrixHeight];
            for (int x = 0; x < _matrixWidth; x++)
            {
                for (int y = 0; y < _matrixHeight; y++)
                {
                    c[x, y] = _matrix[x, y] + _by[x, y];
                }
            }

            return c;
        }

        public static double[,] Subtract(this double[,] _matrix, double[,] _by)
        {
            int _matrixWidth = _matrix.GetLength(0);
            int _matrixHeight = _matrix.GetLength(1);

            int _byWidth = _by.GetLength(0);
            int _byHeight = _by.GetLength(1);

            if (_matrixWidth != _byWidth ||
                _matrixHeight != _byHeight)
                throw new ArgumentException("Matrix multiplication not possible");

            double[,] c = new double[_matrixWidth, _matrixHeight];
            for (int x = 0; x < _matrixWidth; x++)
            {
                for (int y = 0; y < _matrixHeight; y++)
                {
                    c[x, y] = _matrix[x, y] - _by[x, y];
                }
            }

            return c;
        }

        public static double[,] Multiply(this double[,] _matrix, double _scalar)
        {
            int _matrixWidth = _matrix.GetLength(0);
            int _matrixHeight = _matrix.GetLength(1);

            double[,] c = new double[_matrixWidth, _matrixHeight];
            for (int x = 0; x < _matrixWidth; x++)
            {
                for (int y = 0; y < _matrixHeight; y++)
                {
                    c[x, y] = _matrix[x, y] * _scalar;
                }
            }

            return c;
        }

        public static double[,] Multiply(this double[,] _matrix, double[,] _other)
        {
            int _matrixWidth = _matrix.GetLength(0);
            int _matrixHeight = _matrix.GetLength(1);

            int _byWidth = _other.GetLength(0);
            int _byHeight = _other.GetLength(1);

            if (_matrixWidth != _byWidth ||
                _matrixHeight != _byHeight)
                throw new ArgumentException("Matrix multiplication not possible");

            double[,] c = new double[_matrixWidth, _byHeight];
            for (int x = 0; x < _matrixWidth; x++)
            {
                for (int y = 0; y < _matrixHeight; y++)
                {
                    c[x, y] = _matrix[x, y] * _other[x, y];
                }
            }

            return c;
        }

        public static double[,] AddVectorToMatrix(double[,] matrix, double[] vector)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (vector.Length != cols)
            {
                throw new ArgumentException("Length of vector must match the number of columns in the matrix.");
            }

            double[,] result = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = matrix[i, j] + vector[j];
                }
            }

            return result;
        }

        public static double[,] AddVectorToMatrixByRows(double[,] matrix, double[] vector)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (vector.Length != rows)
            {
                throw new ArgumentException("Length of vector must match the number of rows in the matrix.");
            }

            double[,] result = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = matrix[i, j] + vector[i];
                }
            }

            return result;
        }


        public static double[,] AddMatrixBy(double[,] matrix, double value)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[,] result = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = matrix[i, j] + value;
                }
            }

            return result;
        }

        public static double[,] MultiplyMatrixByArray(double[,] matrix, double[] array)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (array.Length != rows)
            {
                throw new ArgumentException("Length of array must match the number of rows in the matrix.");
            }

            double[,] result = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = matrix[i, j] * array[i];
                }
            }

            return result;
        }


        public static T[,] Transpose<T>(this T[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            T[,] result = new T[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }

        public static T[,] ReverseY<T>(this T[,] _colorArray)
        {
            T[,] newArray = new T[_colorArray.GetLength(0), _colorArray.GetLength(1)];
            for (int x = 0; x < _colorArray.GetLength(0); x++)
            {
                for (int yIn = 0, yOut = _colorArray.GetLength(1) - 1; yIn < _colorArray.GetLength(1); yIn++, yOut--)
                {
                    newArray[x, yOut] = _colorArray[x, yIn];
                }
            }

            return newArray;
        }

        public static double[,] SoftMax(double[,] _matrix)
        {
            double[,] softmax = new double[_matrix.GetLength(0), _matrix.GetLength(1)];

            for (int y = 0; y < _matrix.GetLength(1); y++)
            {
                double expForBatch = (double)Enumerable.Range(0, _matrix.GetLength(0)).Select(i => Math.Exp(_matrix[i, y])).Sum();

                for (int x = 0; x < _matrix.GetLength(0); x++)
                {
                    softmax[x, y] = (double)Math.Exp(_matrix[x, y]) / expForBatch;
                }
            }

            return softmax;
        }

        public static T[,] Unsqueeze<T>(T[] array)
        {
            int length = array.Length;
            T[,] result = new T[length, 1];

            for (int i = 0; i < length; i++)
            {
                result[i, 0] = array[i];
            }

            return result;
        }

        public static List<double> SubtractFromValue(double value, List<double> inputList)
        {
            return inputList.Select(x => value - x).ToList();
        }

        public static double[,] ToMatrix(List<double[]> _value)
        {
            if (_value == null || _value.Count == 0 || _value[0].Length == 0)
            {
                return new double[0, 0];
            }

            int rows = _value.Count;
            int cols = _value[0].Length;
            double[,] matrix = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                if (_value[i].Length != cols)
                {
                    throw new ArgumentException("All arrays must have the same length");
                }

                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = _value[i][j];
                }
            }

            return matrix;
        }

        public static double[,] SubtractVectorFromMatrixColumns(double[] vector, double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (vector.Length != cols)
            {
                throw new ArgumentException("Length of the vector must match the number of columns in the matrix.");
            }

            double[,] result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = matrix[i, j] - vector[j];
                }
            }

            return result;
        }

        public static double[,] Pow(double[,] matrix, int _power)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[,] result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = Math.Pow(matrix[i, j], _power);
                }
            }

            return result;
        }

        public static double EuclideanSum(double[,] _vector)
        {
            double euclideanSum = 0f;

            for (int x = 0; x < _vector.GetLength(0); x++)
            {
                for (int y = 0; y < _vector.GetLength(1); y++)
                {
                    euclideanSum += Math.Pow(_vector[x, y], 2);
                }
            }

            return euclideanSum;
        }
    }
}
