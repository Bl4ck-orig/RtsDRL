using System;
using System.IO.Compression;

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

    }
}
