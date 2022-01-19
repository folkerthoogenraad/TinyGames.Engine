using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Minecarts
{
    public class MinecartSpawn
    {
        public int Index { get; }
        public Vector2 Position { get; }

        public MinecartSpawn(int index, Vector2 position)
        {
            Index = index;
            Position = position;
        }
    }
}
