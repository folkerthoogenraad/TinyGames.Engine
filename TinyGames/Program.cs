using System;

namespace TinyGames
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new DepthTestingGame())
                game.Run();
        }
    }
}
