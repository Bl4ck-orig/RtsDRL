using System.Linq;

namespace ReinforcementLearning
{
    public class Exploration
    {
        public static double[] DecaySchedule(double _initValue,
            double _minEpsilon,
            double _epsilonDecayRatio,
            int _maxSteps,
            int _logStart = -2,
            int _logBase = 10)
        {
            int decaySteps = (int)(_maxSteps * _epsilonDecayRatio);
            int remSteps = _maxSteps - decaySteps;
            double[] values = Commons.LogSpace(_logStart, 0, decaySteps, _logBase).Reverse().ToArray();
            double[] valuesMinusMin = Commons.SubtractScalar(values, values.Min());
            values = Commons.DivideScalar(valuesMinusMin, (values.Max() - values.Min()));
            values = Commons.MultiplyScalar(values, (_initValue - _minEpsilon));
            values = Commons.AddScalar(values, _minEpsilon);
            values = Commons.PadArray(values, remSteps);
            return values;
        }
    }
}
