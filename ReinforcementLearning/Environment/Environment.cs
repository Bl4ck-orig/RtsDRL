using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReinforcementLearning
{
    public abstract class Environment
    {
        public abstract List<int> ObservationSpace { get; protected set; }
        public abstract List<int> ActionSpace  { get; protected set; }
        public abstract int Space { get; }


    }
}
