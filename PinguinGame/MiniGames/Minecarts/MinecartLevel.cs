using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Minecarts
{
    public class MinecartLevel
    {
        public float Width => Tilemap.PixelWidth;
        public float Height => Tilemap.PixelHeight;

        public AABB Bounds => AABB.Create(0, 0, Width, Height);

        public MinecartSpawn[] Spawns { get; set; }

        public Tilemap Tilemap { get; set; }
    }
}
