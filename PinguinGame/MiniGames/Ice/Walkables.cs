using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Scenes;

namespace PinguinGame.MiniGames.Ice
{
    public struct GroundInfo
    {
        // TODO add velocity and whatnot here.
        public bool Solid;
        public float Height;
        public Vector2 Velocity;
    }

    public class Walkables : ISceneComponent
    {
        public IceLevel Level { get; set; }

        public Walkables(IceLevel level)
        {
            Level = level;
        }

        public GroundInfo GetGroundInfo(Vector2 position)
        {
            var block = Level.GetIceBlockForPosition(position, 0);

            if (block == null) return new GroundInfo() { Height = 0, Solid = false, Velocity = Vector2.Zero };

            return new GroundInfo() { Height = block.Height, Solid = true, Velocity = block.Velocity };
        }

        // ================================= //
        // Scene component parts
        // ================================= //
        public void Init(Scene scene)
        {
        }

        public void Destroy()
        {
        }
        public void AfterUpdate(float delta)
        {
        }

        public void BeforeUpdate(float delta)
        {
        }


    }
}
