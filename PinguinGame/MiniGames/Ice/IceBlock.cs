using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice
{
    public class IceBlock
    {
        public string Behaviour { get; set; } = "None";
        public Polygon Polygon => LocalPolygon.Translated(Position);
        public Polygon LocalPolygon { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public bool Solid => State.Solid;
        public bool Highlighted => State.Highlighted;
        public float Height => State.Height;

        public float TimerTrigger { get; set; } = 0;
        public float TimerOffset { get; set; } = 0;
        public float TimerCycleDuration { get; set; } = 0;

        public bool IsIdle => State is IceBlockIdleState;
        public bool IsSinking => State is IceBlockSinkingState;
        public bool IsSunken => State is IceBlockSunkenState;
        public bool IsRaising => State is IceBlockRaisingState;

        public IceBlockState State { get; set; }
        public Vector2 DriftDirection = Vector2.Zero;

        public IceBlock(Polygon polygon)
        {
            LocalPolygon = polygon;

            State = new IceBlockIdleState(this);
        }

        public bool PointInside(Vector2 v)
        {
            return LocalPolygon.Inside(v - Position);
        }
        public float DistanceTo(Vector2 v)
        {
            return LocalPolygon.DistanceTo(v - Position);
        }

        public void Update(float delta)
        {
            State = State.Update(delta);
        }
    }
}
