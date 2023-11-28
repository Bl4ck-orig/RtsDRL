using System.Collections.Generic;
using System.Linq;

namespace ReinforcementLearning
{
    public class EnvironemntFrozenLake : Environment
    {
        //public override List<int> ObservationSpace { get; protected set; } = Enumerable.Range(0, 16).ToList();

        //public override List<int> ActionSpace { get; protected set; } = Enumerable.Range(0, 4).ToList();

        //public override int State { protected set; }
        public override int State { get; protected set; }
        protected override List<int> ObservationSpace { get; set; } = Enumerable.Range(0, 16).ToList();
        protected override List<int> ActionSpace { get; set; } = new List<int>()
        {
            0, // Left
            1, // Up
            2, // Right
            3, // Down
        };
        protected override List<int> InitialStates { get; set; } = new List<int>() { 0 };
        protected override List<int> TerminalStates { get; set; } = new List<int>()
        {
            5,
            7,
            11,
            12,
            15,
        };
        protected override Dictionary<StateAction, StepAction> P { get; set; } = new Dictionary<StateAction, StepAction>()
        {
            // State 0 actions
            { new StateAction(0, 0), new StepAction(new List<(double, int)>() { (0.666f, 0), (0.333f, 4) }, new Dictionary<int, double>() { { 0, 0 }, { 4, 0 } }) },
            { new StateAction(0, 1), new StepAction(new List<(double, int)>() { (0.666f, 0), (0.333f, 1) }, new Dictionary<int, double>() { { 0, 0 }, { 1, 0 } }) },
            { new StateAction(0, 2), new StepAction(new List<(double, int)>() { (0.333f, 1), (0.333f, 4), (0.333f, 0) }, new Dictionary<int, double>() { { 1, 0 }, { 4, 0 }, { 0, 0 } }) },
            { new StateAction(0, 3), new StepAction(new List<(double, int)>() { (0.333f, 1), (0.333f, 4), (0.333f, 0) }, new Dictionary<int, double>() { { 1, 0 }, { 4, 0 }, { 0, 0 } }) },
    
            // State 1 actions
            { new StateAction(1, 0), new StepAction(new List<(double, int)>() { (0.333f, 1), (0.333f, 0), (0.333f, 5) }, new Dictionary<int, double>() { { 1, 0.0f }, { 0, 0.0f }, { 5, 0.0f } }) },
            { new StateAction(1, 1), new StepAction(new List<(double, int)>() { (0.333f, 0), (0.333f, 1), (0.333f, 2) }, new Dictionary<int, double>() { { 0, 0.0f }, { 1, 0.0f }, { 2, 0.0f } }) },
            { new StateAction(1, 2), new StepAction(new List<(double, int)>() { (0.333f, 2), (0.333f, 1), (0.333f, 5) }, new Dictionary<int, double>() { { 2, 0.0f }, { 1, 0.0f }, { 5, 0.0f } }) },
            { new StateAction(1, 3), new StepAction(new List<(double, int)>() { (0.333f, 0), (0.333f, 5), (0.333f, 2) }, new Dictionary<int, double>() { { 0, 0.0f }, { 5, 0.0f }, { 2, 0.0f } }) },

            // State 2 actions
            { new StateAction(2, 0), new StepAction(new List<(double, int)>() { (0.333f, 2), (0.333f, 1), (0.333f, 6) }, new Dictionary<int, double>() { { 2, 0.0f }, { 1, 0.0f }, { 6, 0.0f } }) },
            { new StateAction(2, 1), new StepAction(new List<(double, int)>() { (0.333f, 1), (0.333f, 2), (0.333f, 3) }, new Dictionary<int, double>() { { 1, 0.0f }, { 2, 0.0f }, { 3, 0.0f } }) },
            { new StateAction(2, 2), new StepAction(new List<(double, int)>() { (0.333f, 2), (0.333f, 3), (0.333f, 6) }, new Dictionary<int, double>() { { 2, 0.0f }, { 3, 0.0f }, { 6, 0.0f } }) },
            { new StateAction(2, 3), new StepAction(new List<(double, int)>() { (0.333f, 1), (0.333f, 3), (0.333f, 6) }, new Dictionary<int, double>() { { 1, 0.0f }, { 3, 0.0f }, { 6, 0.0f } }) },

            // State 3 actions
            { new StateAction(3, 0), new StepAction(new List<(double, int)>() { (0.333f, 3), (0.333f, 2), (0.333f, 7) }, new Dictionary<int, double>() { { 3, 0.0f }, { 2, 0.0f }, { 7, 0.0f } }) },
            { new StateAction(3, 1), new StepAction(new List<(double, int)>() { (0.333f, 2), (0.333f, 3), (0.333f, 3) }, new Dictionary<int, double>() { { 2, 0.0f }, { 3, 0.0f } }) },
            { new StateAction(3, 2), new StepAction(new List<(double, int)>() { (0.333f, 3), (0.333f, 3), (0.333f, 7) }, new Dictionary<int, double>() { { 3, 0.0f }, { 7, 0.0f } }) },
            { new StateAction(3, 3), new StepAction(new List<(double, int)>() { (0.333f, 3), (0.333f, 7), (0.333f, 2) }, new Dictionary<int, double>() { { 3, 0.0f }, { 7, 0.0f }, { 2, 0.0f } }) },
    
            // State 4 actions
            { new StateAction(4, 0), new StepAction(new List<(double, int)>() { (0.333f, 4), (0.333f, 0), (0.333f, 8) }, new Dictionary<int, double>() { { 4, 0.0f }, { 0, 0.0f }, { 8, 0.0f } }) },
            { new StateAction(4, 1), new StepAction(new List<(double, int)>() { (0.333f, 4), (0.333f, 0), (0.333f, 5) }, new Dictionary<int, double>() { { 4, 0.0f }, { 0, 0.0f }, { 5, 0.0f } }) },
            { new StateAction(4, 2), new StepAction(new List<(double, int)>() { (0.333f, 0), (0.333f, 5), (0.333f, 8) }, new Dictionary<int, double>() { { 0, 0.0f }, { 5, 0.0f }, { 8, 0.0f } }) },
            { new StateAction(4, 3), new StepAction(new List<(double, int)>() { (0.333f, 0), (0.333f, 8), (0.333f, 5) }, new Dictionary<int, double>() { { 0, 0.0f }, { 8, 0.0f }, { 5, 0.0f } }) },

            // State 5 actions
            { new StateAction(5, 0), new StepAction(new List<(double, int)>() { (1.0f, 5) }, new Dictionary<int, double>() { { 5, 0.0f } }) },
            { new StateAction(5, 1), new StepAction(new List<(double, int)>() { (1.0f, 5) }, new Dictionary<int, double>() { { 5, 0.0f } }) },
            { new StateAction(5, 2), new StepAction(new List<(double, int)>() { (1.0f, 5) }, new Dictionary<int, double>() { { 5, 0.0f } }) },
            { new StateAction(5, 3), new StepAction(new List<(double, int)>() { (1.0f, 5) }, new Dictionary<int, double>() { { 5, 0.0f } }) },

            // State 6 actions
            { new StateAction(6, 0), new StepAction(new List<(double, int)>() { (0.333f, 2), (0.333f, 5), (0.333f, 10) }, new Dictionary<int, double>() { { 2, 0.0f }, { 5, 0.0f }, { 10, 0.0f } }) },
            { new StateAction(6, 1), new StepAction(new List<(double, int)>() { (0.333f, 5), (0.333f, 2), (0.333f, 7) }, new Dictionary<int, double>() { { 5, 0.0f }, { 2, 0.0f }, { 7, 0.0f } }) },
            { new StateAction(6, 2), new StepAction(new List<(double, int)>() { (0.333f, 2), (0.333f, 7), (0.333f, 10) }, new Dictionary<int, double>() { { 2, 0.0f }, { 7, 0.0f }, { 10, 0.0f } }) },
            { new StateAction(6, 3), new StepAction(new List<(double, int)>() { (0.333f, 5), (0.333f, 10), (0.333f, 7) }, new Dictionary<int, double>() { { 5, 0.0f }, { 10, 0.0f }, { 7, 0.0f } }) },

            // State 7 actions
            { new StateAction(7, 0), new StepAction(new List<(double, int)>() { (1.0f, 7) }, new Dictionary<int, double>() { { 7, 0.0f } }) },
            { new StateAction(7, 1), new StepAction(new List<(double, int)>() { (1.0f, 7) }, new Dictionary<int, double>() { { 7, 0.0f } }) },
            { new StateAction(7, 2), new StepAction(new List<(double, int)>() { (1.0f, 7) }, new Dictionary<int, double>() { { 7, 0.0f } }) },
            { new StateAction(7, 3), new StepAction(new List<(double, int)>() { (1.0f, 7) }, new Dictionary<int, double>() { { 7, 0.0f } }) },

            // State 8 actions
            { new StateAction(8, 0), new StepAction(new List<(double, int)>() { (0.333f, 4), (0.333f, 8), (0.333f, 12) }, new Dictionary<int, double>() { { 4, 0.0f }, { 8, 0.0f }, { 12, 0.0f } }) },
            { new StateAction(8, 1), new StepAction(new List<(double, int)>() { (0.333f, 8), (0.333f, 4), (0.333f, 9) }, new Dictionary<int, double>() { { 8, 0.0f }, { 4, 0.0f }, { 9, 0.0f } }) },
            { new StateAction(8, 2), new StepAction(new List<(double, int)>() { (0.333f, 4), (0.333f, 9), (0.333f, 12) }, new Dictionary<int, double>() { { 4, 0.0f }, { 9, 0.0f }, { 12, 0.0f } }) },
            { new StateAction(8, 3), new StepAction(new List<(double, int)>() { (0.333f, 9), (0.333f, 12), (0.333f, 8) }, new Dictionary<int, double>() { { 9, 0.0f }, { 12, 0.0f }, { 8, 0.0f } }) },

            // State 9 actions
            { new StateAction(9, 0), new StepAction(new List<(double, int)>() { (0.333f, 5), (0.333f, 8), (0.333f, 13) }, new Dictionary<int, double>() { { 5, 0.0f }, { 8, 0.0f }, { 13, 0.0f } }) },
            { new StateAction(9, 1), new StepAction(new List<(double, int)>() { (0.333f, 8), (0.333f, 5), (0.333f, 10) }, new Dictionary<int, double>() { { 8, 0.0f }, { 5, 0.0f }, { 10, 0.0f } }) },
            { new StateAction(9, 2), new StepAction(new List<(double, int)>() { (0.333f, 5), (0.333f, 10), (0.333f, 13) }, new Dictionary<int, double>() { { 5, 0.0f }, { 10, 0.0f }, { 13, 0.0f } }) },
            { new StateAction(9, 3), new StepAction(new List<(double, int)>() { (0.333f, 8), (0.333f, 13), (0.333f, 10) }, new Dictionary<int, double>() { { 8, 0.0f }, { 13, 0.0f }, { 10, 0.0f } }) },

            // State 10 actions
            { new StateAction(10, 0), new StepAction(new List<(double, int)>() { (0.333f, 6), (0.333f, 9), (0.333f, 14) }, new Dictionary<int, double>() { { 6, 0.0f }, { 9, 0.0f }, { 14, 0.0f } }) },
            { new StateAction(10, 1), new StepAction(new List<(double, int)>() { (0.333f, 9), (0.333f, 6), (0.333f, 11) }, new Dictionary<int, double>() { { 9, 0.0f }, { 6, 0.0f }, { 11, 0.0f } }) },
            { new StateAction(10, 2), new StepAction(new List<(double, int)>() { (0.333f, 6), (0.333f, 11), (0.333f, 14) }, new Dictionary<int, double>() { { 6, 0.0f }, { 11, 0.0f }, { 14, 0.0f } }) },
            { new StateAction(10, 3), new StepAction(new List<(double, int)>() { (0.333f, 9), (0.333f, 14), (0.333f, 11) }, new Dictionary<int, double>() { { 9, 0.0f }, { 14, 0.0f }, { 11, 0.0f } }) },

            // State 11 actions
            { new StateAction(11, 0), new StepAction(new List<(double, int)>() { (1.0f, 11) }, new Dictionary<int, double>() { { 11, 0.0f } }) },
            { new StateAction(11, 1), new StepAction(new List<(double, int)>() { (1.0f, 11) }, new Dictionary<int, double>() { { 11, 0.0f } }) },
            { new StateAction(11, 2), new StepAction(new List<(double, int)>() { (1.0f, 11) }, new Dictionary<int, double>() { { 11, 0.0f } }) },
            { new StateAction(11, 3), new StepAction(new List<(double, int)>() { (1.0f, 11) }, new Dictionary<int, double>() { { 11, 0.0f } }) },

            // State 12 actions
            { new StateAction(12, 0), new StepAction(new List<(double, int)>() { (1.0f, 12) }, new Dictionary<int, double>() { { 12, 0.0f } }) },
            { new StateAction(12, 1), new StepAction(new List<(double, int)>() { (1.0f, 12) }, new Dictionary<int, double>() { { 12, 0.0f } }) },
            { new StateAction(12, 2), new StepAction(new List<(double, int)>() { (1.0f, 12) }, new Dictionary<int, double>() { { 12, 0.0f } }) },
            { new StateAction(12, 3), new StepAction(new List<(double, int)>() { (1.0f, 12) }, new Dictionary<int, double>() { { 12, 0.0f } }) },

            // State 13 actions
            { new StateAction(13, 0), new StepAction(new List<(double, int)>() { (0.333f, 12), (0.333f, 13), (0.333f, 9) }, new Dictionary<int, double>() { { 12, 0.0f }, { 13, 0.0f }, { 9, 0.0f } }) },
            { new StateAction(13, 1), new StepAction(new List<(double, int)>() { (0.333f, 12), (0.333f, 9), (0.333f, 14) }, new Dictionary<int, double>() { { 12, 0.0f }, { 9, 0.0f }, { 14, 0.0f } }) },
            { new StateAction(13, 2), new StepAction(new List<(double, int)>() { (0.333f, 9), (0.333f, 14), (0.333f, 13) }, new Dictionary<int, double>() { { 9, 0.0f }, { 14, 0.0f }, { 13, 0.0f } }) },
            { new StateAction(13, 3), new StepAction(new List<(double, int)>() { (0.333f, 12), (0.333f, 13), (0.333f, 14) }, new Dictionary<int, double>() { { 12, 0.0f }, { 13, 0.0f }, { 14, 0.0f } }) },

            // State 14 actions
            { new StateAction(14, 0), new StepAction(new List<(double, int)>() { (0.333f, 14), (0.333f, 13), (0.333f, 10) }, new Dictionary<int, double>() { { 14, 0.0f }, { 13, 0.0f }, { 10, 0.0f } }) },
            { new StateAction(14, 1), new StepAction(new List<(double, int)>() { (0.333f, 13), (0.333f, 10), (0.333f, 15) }, new Dictionary<int, double>() { { 13, 0.0f }, { 10, 0.0f }, { 15, 1.0f } }) },
            { new StateAction(14, 2), new StepAction(new List<(double, int)>() { (0.333f, 10), (0.333f, 15), (0.333f, 14) }, new Dictionary<int, double>() { { 10, 0.0f }, { 15, 1.0f }, { 14, 0.0f } }) },
            { new StateAction(14, 3), new StepAction(new List<(double, int)>() { (0.333f, 13), (0.333f, 14), (0.333f, 15) }, new Dictionary<int, double>() { { 13, 0.0f }, { 14, 0.0f }, { 15, 1.0f } }) },

            // State 15 actions
            { new StateAction(15, 0), new StepAction(new List<(double, int)>() { (1.0f, 15) }, new Dictionary<int, double>() { { 15, 0.0f } }) },
            { new StateAction(15, 1), new StepAction(new List<(double, int)>() { (1.0f, 15) }, new Dictionary<int, double>() { { 15, 0.0f } }) },
            { new StateAction(15, 2), new StepAction(new List<(double, int)>() { (1.0f, 15) }, new Dictionary<int, double>() { { 15, 0.0f } }) },
            { new StateAction(15, 3), new StepAction(new List<(double, int)>() { (1.0f, 15) }, new Dictionary<int, double>() { { 15, 0.0f } }) },
        };
    }
}
