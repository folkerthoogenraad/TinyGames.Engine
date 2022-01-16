using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace PinguinGame.Pinguins.Levels
{
    public class IceBlock
    {
        public bool Sinkable { get; set; } = true;
        public Polygon Polygon { get; set; }
        
        public bool Solid => State.Solid;
        public bool Highlighted => State.Highlighted;
        public float Height => State.Height;

        public IceBlockState State { get; set; }

        public IceBlock(Polygon polygon)
        {
            Polygon = polygon;

            State = new IceBlockIdleState(this);
        }

        public bool PointInside(Vector2 v)
        {
            return Polygon.Inside(v);
        }

        public void Update(float delta)
        {
            State = State.Update(delta);
        }
    }
}
