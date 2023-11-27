using System.Collections.Generic;
using System.Linq;

namespace ReinforcementLearning
{
    public class EnvironemntFrozenLake : Environment
    {
        public EnvironemntFrozenLake()
        {
        }

        public override List<int> ObservationSpace { get; protected set; } = Enumerable.Range(0, 16).ToList();

        public override List<int> ActionSpace { get; protected set; } = Enumerable.Range(0, 4).ToList();

        public override int Space => 0;
    }
}
