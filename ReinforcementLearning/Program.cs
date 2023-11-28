using ReinforcementLearning.Training;
using System;

namespace ReinforcementLearning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            QLearningResult qLearningResult = QLearning.Learn(new QLearningArgs(new EnvironemntFrozenLake()));

            Console.WriteLine("\n" + qLearningResult.ToString());

            Console.ReadLine();
        }
    }
}
