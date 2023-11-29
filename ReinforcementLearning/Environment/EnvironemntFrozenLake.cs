using System.Collections.Generic;
using System.Linq;

namespace ReinforcementLearning
{
    public class EnvironemntFrozenLake : Environment<int>
    {
        //public override List<int> ObservationSpace { get; protected set; } = Enumerable.Range(0, 16).ToList();

        //public override List<int> ActionSpace { get; protected set; } = Enumerable.Range(0, 4).ToList();

        //public override int State { protected set; }
        public override int ObservationSpaceSize { get => ObservationSpace.Count; }

        public override int ActionSpaceSize { get => ActionSpace.Count; }

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
        protected override Dictionary<StateAction<int>, StepAction<int>> P { get; set; } = new Dictionary<StateAction<int>, StepAction<int>>()
        {
            // State 0 actions
            { new StateAction<int>(0, 0), new StepAction<int>(new List<(double, int)>() { (0.666f, 0), (0.333f, 4) }, new Dictionary<int, double>() { { 0, 0 }, { 4, 0 } }) },
            { new StateAction<int>(0, 1), new StepAction<int>(new List<(double, int)>() { (0.666f, 0), (0.333f, 1) }, new Dictionary<int, double>() { { 0, 0 }, { 1, 0 } }) },
            { new StateAction<int>(0, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 1), (0.333f, 4), (0.333f, 0) }, new Dictionary<int, double>() { { 1, 0 }, { 4, 0 }, { 0, 0 } }) },
            { new StateAction<int>(0, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 1), (0.333f, 4), (0.333f, 0) }, new Dictionary<int, double>() { { 1, 0 }, { 4, 0 }, { 0, 0 } }) },
    
            // State 1 actions
            { new StateAction<int>(1, 0), new StepAction<int>(new List<(double, int)>() { (0.333f, 1), (0.333f, 0), (0.333f, 5) }, new Dictionary<int, double>() { { 1, 0.0f }, { 0, 0.0f }, { 5, 0.0f } }) },
            { new StateAction<int>(1, 1), new StepAction<int>(new List<(double, int)>() { (0.333f, 0), (0.333f, 1), (0.333f, 2) }, new Dictionary<int, double>() { { 0, 0.0f }, { 1, 0.0f }, { 2, 0.0f } }) },
            { new StateAction<int>(1, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 2), (0.333f, 1), (0.333f, 5) }, new Dictionary<int, double>() { { 2, 0.0f }, { 1, 0.0f }, { 5, 0.0f } }) },
            { new StateAction<int>(1, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 0), (0.333f, 5), (0.333f, 2) }, new Dictionary<int, double>() { { 0, 0.0f }, { 5, 0.0f }, { 2, 0.0f } }) },

            // State 2 actions
            { new StateAction<int>(2, 0), new StepAction<int>(new List<(double, int)>() { (0.333f, 2), (0.333f, 1), (0.333f, 6) }, new Dictionary<int, double>() { { 2, 0.0f }, { 1, 0.0f }, { 6, 0.0f } }) },
            { new StateAction<int>(2, 1), new StepAction<int>(new List<(double, int)>() { (0.333f, 1), (0.333f, 2), (0.333f, 3) }, new Dictionary<int, double>() { { 1, 0.0f }, { 2, 0.0f }, { 3, 0.0f } }) },
            { new StateAction<int>(2, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 2), (0.333f, 3), (0.333f, 6) }, new Dictionary<int, double>() { { 2, 0.0f }, { 3, 0.0f }, { 6, 0.0f } }) },
            { new StateAction<int>(2, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 1), (0.333f, 3), (0.333f, 6) }, new Dictionary<int, double>() { { 1, 0.0f }, { 3, 0.0f }, { 6, 0.0f } }) },

            // State 3 actions
            { new StateAction<int>(3, 0), new StepAction<int>(new List<(double, int)>() { (0.333f, 3), (0.333f, 2), (0.333f, 7) }, new Dictionary<int, double>() { { 3, 0.0f }, { 2, 0.0f }, { 7, 0.0f } }) },
            { new StateAction<int>(3, 1), new StepAction<int>(new List<(double, int)>() { (0.333f, 2), (0.333f, 3), (0.333f, 3) }, new Dictionary<int, double>() { { 2, 0.0f }, { 3, 0.0f } }) },
            { new StateAction<int>(3, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 3), (0.333f, 3), (0.333f, 7) }, new Dictionary<int, double>() { { 3, 0.0f }, { 7, 0.0f } }) },
            { new StateAction<int>(3, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 3), (0.333f, 7), (0.333f, 2) }, new Dictionary<int, double>() { { 3, 0.0f }, { 7, 0.0f }, { 2, 0.0f } }) },
    
            // State 4 actions
            { new StateAction<int>(4, 0), new StepAction<int>(new List<(double, int)>() { (0.333f, 4), (0.333f, 0), (0.333f, 8) }, new Dictionary<int, double>() { { 4, 0.0f }, { 0, 0.0f }, { 8, 0.0f } }) },
            { new StateAction<int>(4, 1), new StepAction<int>(new List<(double, int)>() { (0.333f, 4), (0.333f, 0), (0.333f, 5) }, new Dictionary<int, double>() { { 4, 0.0f }, { 0, 0.0f }, { 5, 0.0f } }) },
            { new StateAction<int>(4, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 0), (0.333f, 5), (0.333f, 8) }, new Dictionary<int, double>() { { 0, 0.0f }, { 5, 0.0f }, { 8, 0.0f } }) },
            { new StateAction<int>(4, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 0), (0.333f, 8), (0.333f, 5) }, new Dictionary<int, double>() { { 0, 0.0f }, { 8, 0.0f }, { 5, 0.0f } }) },

            // State 5 actions
            { new StateAction<int>(5, 0), new StepAction<int>(new List<(double, int)>() { (1.0f, 5) }, new Dictionary<int, double>() { { 5, 0.0f } }) },
            { new StateAction<int>(5, 1), new StepAction<int>(new List<(double, int)>() { (1.0f, 5) }, new Dictionary<int, double>() { { 5, 0.0f } }) },
            { new StateAction<int>(5, 2), new StepAction<int>(new List<(double, int)>() { (1.0f, 5) }, new Dictionary<int, double>() { { 5, 0.0f } }) },
            { new StateAction<int>(5, 3), new StepAction<int>(new List<(double, int)>() { (1.0f, 5) }, new Dictionary<int, double>() { { 5, 0.0f } }) },

            // State 6 actions
            { new StateAction<int>(6, 0), new StepAction<int>(new List<(double, int)>() { (0.333f, 2), (0.333f, 5), (0.333f, 10) }, new Dictionary<int, double>() { { 2, 0.0f }, { 5, 0.0f }, { 10, 0.0f } }) },
            { new StateAction<int>(6, 1), new StepAction<int>(new List<(double, int)>() { (0.333f, 5), (0.333f, 2), (0.333f, 7) }, new Dictionary<int, double>() { { 5, 0.0f }, { 2, 0.0f }, { 7, 0.0f } }) },
            { new StateAction<int>(6, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 2), (0.333f, 7), (0.333f, 10) }, new Dictionary<int, double>() { { 2, 0.0f }, { 7, 0.0f }, { 10, 0.0f } }) },
            { new StateAction<int>(6, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 5), (0.333f, 10), (0.333f, 7) }, new Dictionary<int, double>() { { 5, 0.0f }, { 10, 0.0f }, { 7, 0.0f } }) },

            // State 7 actions
            { new StateAction<int>(7, 0), new StepAction<int>(new List<(double, int)>() { (1.0f, 7) }, new Dictionary<int, double>() { { 7, 0.0f } }) },
            { new StateAction<int>(7, 1), new StepAction<int>(new List<(double, int)>() { (1.0f, 7) }, new Dictionary<int, double>() { { 7, 0.0f } }) },
            { new StateAction<int>(7, 2), new StepAction<int>(new List<(double, int)>() { (1.0f, 7) }, new Dictionary<int, double>() { { 7, 0.0f } }) },
            { new StateAction<int>(7, 3), new StepAction<int>(new List<(double, int)>() { (1.0f, 7) }, new Dictionary<int, double>() { { 7, 0.0f } }) },

            // State 8 actions
            { new StateAction<int>(8, 0), new StepAction<int>(new List<(double, int)>() { (0.333f, 4), (0.333f, 8), (0.333f, 12) }, new Dictionary<int, double>() { { 4, 0.0f }, { 8, 0.0f }, { 12, 0.0f } }) },
            { new StateAction<int>(8, 1), new StepAction<int>(new List<(double, int)>() { (0.333f, 8), (0.333f, 4), (0.333f, 9) }, new Dictionary<int, double>() { { 8, 0.0f }, { 4, 0.0f }, { 9, 0.0f } }) },
            { new StateAction<int>(8, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 4), (0.333f, 9), (0.333f, 12) }, new Dictionary<int, double>() { { 4, 0.0f }, { 9, 0.0f }, { 12, 0.0f } }) },
            { new StateAction<int>(8, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 9), (0.333f, 12), (0.333f, 8) }, new Dictionary<int, double>() { { 9, 0.0f }, { 12, 0.0f }, { 8, 0.0f } }) },

            // State 9 actions
            { new StateAction<int>(9, 0), new StepAction<int>(new List<(double, int)>() { (0.333f, 5), (0.333f, 8), (0.333f, 13) }, new Dictionary<int, double>() { { 5, 0.0f }, { 8, 0.0f }, { 13, 0.0f } }) },
            { new StateAction<int>(9, 1), new StepAction<int>(new List<(double, int)>() { (0.333f, 8), (0.333f, 5), (0.333f, 10) }, new Dictionary<int, double>() { { 8, 0.0f }, { 5, 0.0f }, { 10, 0.0f } }) },
            { new StateAction<int>(9, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 5), (0.333f, 10), (0.333f, 13) }, new Dictionary<int, double>() { { 5, 0.0f }, { 10, 0.0f }, { 13, 0.0f } }) },
            { new StateAction<int>(9, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 8), (0.333f, 13), (0.333f, 10) }, new Dictionary<int, double>() { { 8, 0.0f }, { 13, 0.0f }, { 10, 0.0f } }) },

            // State 10 actions
            { new StateAction<int>(10, 0), new StepAction<int>(new List<(double, int)>() { (0.333f, 6), (0.333f, 9), (0.333f, 14) }, new Dictionary<int, double>() { { 6, 0.0f }, { 9, 0.0f }, { 14, 0.0f } }) },
            { new StateAction<int>(10, 1), new StepAction<int>(new List<(double, int)>() { (0.333f, 9), (0.333f, 6), (0.333f, 11) }, new Dictionary<int, double>() { { 9, 0.0f }, { 6, 0.0f }, { 11, 0.0f } }) },
            { new StateAction<int>(10, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 6), (0.333f, 11), (0.333f, 14) }, new Dictionary<int, double>() { { 6, 0.0f }, { 11, 0.0f }, { 14, 0.0f } }) },
            { new StateAction<int>(10, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 9), (0.333f, 14), (0.333f, 11) }, new Dictionary<int, double>() { { 9, 0.0f }, { 14, 0.0f }, { 11, 0.0f } }) },

            // State 11 actions
            { new StateAction<int>(11, 0), new StepAction<int>(new List<(double, int)>() { (1.0f, 11) }, new Dictionary<int, double>() { { 11, 0.0f } }) },
            { new StateAction<int>(11, 1), new StepAction<int>(new List<(double, int)>() { (1.0f, 11) }, new Dictionary<int, double>() { { 11, 0.0f } }) },
            { new StateAction<int>(11, 2), new StepAction<int>(new List<(double, int)>() { (1.0f, 11) }, new Dictionary<int, double>() { { 11, 0.0f } }) },
            { new StateAction<int>(11, 3), new StepAction<int>(new List<(double, int)>() { (1.0f, 11) }, new Dictionary<int, double>() { { 11, 0.0f } }) },

            // State 12 actions
            { new StateAction<int>(12, 0), new StepAction<int>(new List<(double, int)>() { (1.0f, 12) }, new Dictionary<int, double>() { { 12, 0.0f } }) },
            { new StateAction<int>(12, 1), new StepAction<int>(new List<(double, int)>() { (1.0f, 12) }, new Dictionary<int, double>() { { 12, 0.0f } }) },
            { new StateAction<int>(12, 2), new StepAction<int>(new List<(double, int)>() { (1.0f, 12) }, new Dictionary<int, double>() { { 12, 0.0f } }) },
            { new StateAction<int>(12, 3), new StepAction<int>(new List<(double, int)>() { (1.0f, 12) }, new Dictionary<int, double>() { { 12, 0.0f } }) },

            // State 13 actions
            { new StateAction<int>(13, 0), new StepAction<int>(new List<(double, int)>() { (0.333f, 12), (0.333f, 13), (0.333f, 9) }, new Dictionary<int, double>() { { 12, 0.0f }, { 13, 0.0f }, { 9, 0.0f } }) },
            { new StateAction<int>(13, 1), new StepAction<int>(new List<(double, int)>() { (0.333f, 12), (0.333f, 9), (0.333f, 14) }, new Dictionary<int, double>() { { 12, 0.0f }, { 9, 0.0f }, { 14, 0.0f } }) },
            { new StateAction<int>(13, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 9), (0.333f, 14), (0.333f, 13) }, new Dictionary<int, double>() { { 9, 0.0f }, { 14, 0.0f }, { 13, 0.0f } }) },
            { new StateAction<int>(13, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 12), (0.333f, 13), (0.333f, 14) }, new Dictionary<int, double>() { { 12, 0.0f }, { 13, 0.0f }, { 14, 0.0f } }) },

            // State 14 actions
            { new StateAction<int>(14, 0), new StepAction<int>(new List<(double, int)>() { (0.333f, 14), (0.333f, 13), (0.333f, 10) }, new Dictionary<int, double>() { { 14, 0.0f }, { 13, 0.0f }, { 10, 0.0f } }) },
            { new StateAction<int>(14, 1), new StepAction<int>(new List<(double, int)>() { (0.333f, 13), (0.333f, 10), (0.333f, 15) }, new Dictionary<int, double>() { { 13, 0.0f }, { 10, 0.0f }, { 15, 1.0f } }) },
            { new StateAction<int>(14, 2), new StepAction<int>(new List<(double, int)>() { (0.333f, 10), (0.333f, 15), (0.333f, 14) }, new Dictionary<int, double>() { { 10, 0.0f }, { 15, 1.0f }, { 14, 0.0f } }) },
            { new StateAction<int>(14, 3), new StepAction<int>(new List<(double, int)>() { (0.333f, 13), (0.333f, 14), (0.333f, 15) }, new Dictionary<int, double>() { { 13, 0.0f }, { 14, 0.0f }, { 15, 1.0f } }) },

            // State 15 actions
            { new StateAction<int>(15, 0), new StepAction<int>(new List<(double, int)>() { (1.0f, 15) }, new Dictionary<int, double>() { { 15, 0.0f } }) },
            { new StateAction<int>(15, 1), new StepAction<int>(new List<(double, int)>() { (1.0f, 15) }, new Dictionary<int, double>() { { 15, 0.0f } }) },
            { new StateAction<int>(15, 2), new StepAction<int>(new List<(double, int)>() { (1.0f, 15) }, new Dictionary<int, double>() { { 15, 0.0f } }) },
            { new StateAction<int>(15, 3), new StepAction<int>(new List<(double, int)>() { (1.0f, 15) }, new Dictionary<int, double>() { { 15, 0.0f } }) },
        };

        protected override int TimeStepLimit => int.MaxValue;

    }
}
