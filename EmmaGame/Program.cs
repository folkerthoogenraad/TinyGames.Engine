using System;

namespace EmmaGame
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new EmmaGameTest())
                game.Run();
        }
    }
}
