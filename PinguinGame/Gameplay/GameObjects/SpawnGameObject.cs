using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Gameplay.GameObjects
{
    public class SpawnGameObject : GameObject
    {
        public Vector2 Position;

        public SpawnGameObject(Vector2 v)
        {
            Position = v;
        }
    }
}
