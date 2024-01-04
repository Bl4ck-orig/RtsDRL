using System;
using System.Threading.Tasks;

namespace Utilities
{
    public static class InputManager
    {
        public static string Name { get; set; } = nameof(InputManager);

        public static bool Interrupt { get; set; } = false;

        public static bool Pause { get; set; } = false;

        private static ConsoleKey pauseKey = ConsoleKey.Spacebar;
        private static ConsoleKey interruptKey = ConsoleKey.Escape;

        public static async void ListenInputs()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey();

                    HandleInterrupt(key);
                }
            });
        }

        public static void HandleInterrupt(ConsoleKeyInfo _keyInfo)
        {
            if (_keyInfo.Key != interruptKey)
                return;

            Interrupt = true;
            Console.WriteLine("Interrupt");

        }
    }
}
