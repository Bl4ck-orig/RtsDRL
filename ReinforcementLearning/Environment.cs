using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReinforcementLearning
{
    public abstract class Environment
    {
        public List<int> ObservationSpace = new List<int>();
        public List<int> ActionSpace = new List<int>();
    }
}
