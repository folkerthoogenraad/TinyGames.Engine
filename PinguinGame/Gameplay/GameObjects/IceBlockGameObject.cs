using Microsoft.Xna.Framework;
using PinguinGame.Gameplay.Behaviours;
using PinguinGame.Gameplay.GameObjects.IceBlockStates;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Gameplay.GameObjects
{
    public class IceBlockGameObject : GameObject, IWalkable
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

        public IceBlockGameObject(Polygon polygon)
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

        public override void Update(float delta)
        {
            State = State.Update(delta);
        }

        public GroundInfo GetGroundInfo(Vector2 point)
        {
            if (LocalPolygon.Inside(point - Position))
            {
                return GroundInfo.Solid(GroundMaterial.Snow, Velocity, Height);
            }
            return GroundInfo.Empty();
        }
    }
}
