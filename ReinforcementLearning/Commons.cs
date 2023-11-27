using System;

namespace ReinforcementLearning
{
    public static class Commons
    {
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
    }
}
