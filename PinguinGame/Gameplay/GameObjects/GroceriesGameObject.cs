using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Collisions;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.Gameplay.GameObjects
{
    [RequireSceneBehaviour(typeof(ShoppingGraphics))]
    public class GroceriesGameObject : DetailGameObject
    {
        private string _spriteName;

        public GroceriesGameObject(Vector2 position, Vector2 size, string spriteName) : base(position)
        {
            _spriteName = spriteName;

            Collider = new BoxCollider(AABB.Create(-size.X / 2, -size.Y, size.X, size.Y));
            IsSolid = true;

            // Bit ugly hack but w/e
            Position = position + new Vector2(size.X / 2, size.Y);
        }

        public override void LoadSprites(IceGameGraphics graphics)
        {
            var g = Scene.GetBehaviour<ShoppingGraphics>();

            Sprite = g.GetSprite(_spriteName);
        }
    }
}
