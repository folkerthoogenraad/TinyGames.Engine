using System;

namespace StudentBikeGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new StudentBikeGame())
                game.Run();
        }
    }
}
