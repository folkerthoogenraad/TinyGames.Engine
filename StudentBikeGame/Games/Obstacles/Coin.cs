using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace StudentBikeGame.Games.Obstacles
{
    public class Coin
    {
        public Vector2 Position { get; set; }
        public AABB Bounds => AABB.CreateCentered(Position, new Vector2(16, 16));

        public Coin(Vector2 position)
        {
            Position = position;
        }
    }
}
