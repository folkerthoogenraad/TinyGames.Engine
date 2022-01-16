using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Maths.Algorithms;

namespace PinguinGame.Pinguins.Levels
{
    public class Level
    {
        public IceBlock[] Blocks { get; set; }

        public Level()
        {

        }

        public IceBlock GetIceBlockForPosition(Vector2 position)
        {
            return Blocks.Where(x => x.PointInside(position)).FirstOrDefault();
        }
    }
}
