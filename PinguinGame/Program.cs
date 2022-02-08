using PinguinGame.IO.Characters;
using System;
using TinyGames.Engine.IO;

namespace TinyGames
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new PinguinGame.PenguinGame())
                game.Run();
        }
    }
}
