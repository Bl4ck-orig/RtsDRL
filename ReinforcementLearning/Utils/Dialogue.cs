using System;

namespace ReinforcementLearning
{
    public class Dialogue
    {
        private const int MAX_PROGRESS_LETTERS = 50;

        public static void PrintProgress(int _episodes, int _maxEpisodes, bool _firstCall)
        {
            if (!_firstCall && Console.CursorTop != 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }

            float finishedPercent = ((float)_episodes / _maxEpisodes);
            string progresString = "|";
            for (int i = 0; i < MAX_PROGRESS_LETTERS; i++)
            {
                progresString += ((float)i / MAX_PROGRESS_LETTERS) < finishedPercent ? "=" : " ";
            }
            progresString += "|";

            Console.WriteLine(progresString);
        }

        public static void PrintPolicy()
        {

        }

        internal static void PrintQLearningResult(QLearningResult _qLearningResult)
        {

            
        }
    }
}
