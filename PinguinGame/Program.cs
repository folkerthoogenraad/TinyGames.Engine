using PinguinGame.IO.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TinyGames.Engine.Console;
using TinyGames.Engine.IO;
using TinyGames.Engine.Reflection;
using TinyGames.Engine.Scenes;

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
