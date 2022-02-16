using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Maths.Algorithms;

namespace PinguinGame.MiniGames.Ice
{
    // I think this should be more of a template then the actual level
    public class IceLevel
    {
        // These blocks have a state, which is _very_ weird
        public IceBlock[] Blocks { get; set; }
        public Vector2[] Spawns { get; set; }
        public Vector2[] Geysers { get; set; }
        public Vector2[] Grass { get; set; }
        public Vector2[] Trees { get; set; }
        public (Vector2 Position, Vector2 Size)[] Bridges { get; set; }

        public IceLevel()
        {

        }

        public IceBlock GetIceBlockForPosition(Vector2 position, float margin = 4)
        {
            var block = Blocks.Where(x => x.PointInside(position)).FirstOrDefault();

            if(block != null)
            {
                return block;   
            }

            return Blocks.Where(x => 
                x.DistanceTo(position) < margin
                ).FirstOrDefault();
        }
    }
}
