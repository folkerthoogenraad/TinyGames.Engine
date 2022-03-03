using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Collisions;
using TinyGames.Engine.Maths;

namespace PinguinGame.Gameplay.GameObjects
{
    public class WallGameObject : SolidGameObject
    {
        public WallGameObject(Vector2 position, Vector2 size)
        {
            Position = position;
            Collider = new BoxCollider(AABB.Create(0, 0, size.X, size.Y));
        }
    }
}
