using ReinforcementLearning.Training;
using System;

namespace ReinforcementLearning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

        }

        private static void RunQLearning()
        {
            QLearningResult qLearningResult = QLearning.Learn(new QLearningArgs(new EnvironemntFrozenLake()));

            Console.WriteLine("\n" + qLearningResult.ToString());

            Console.ReadLine();
        }

        private static void RunNql()
        {
            //var nqlResult = QLearning.Learn(new QLearningArgs(new EnvironemntFrozenLake()));
            //
            //Console.WriteLine("\n" + qLearningResult.ToString());
            //
            //Console.ReadLine();
        }
    }
}
