using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Collisions;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Gameplay.GameObjects
{
    // This _should_ honestly be a component and not this. 
    // A nice component that gets registered to a nice service
    // with everything nice
    // but this is fine for now.
    public abstract class SolidGameObject : GameObject
    {
        public Vector2 Position;
        public Collider Collider { get; set; }

        public bool CanCollide => IsSolid && Collider != null;
        public bool IsSolid { get; set; } = true;
    }
}
