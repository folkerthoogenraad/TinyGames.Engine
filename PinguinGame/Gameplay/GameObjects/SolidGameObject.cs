using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Collisions;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Gameplay.GameObjects
{
    public abstract class SolidGameObject : GameObject
    {
        public Vector2 Position;
        public Collider Collider { get; set; }
    }
}
