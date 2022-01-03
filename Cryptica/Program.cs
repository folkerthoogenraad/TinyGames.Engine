using System;

namespace Cryptica
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var game = new CrypticaGame())
                game.Run();
        }
    }
}
