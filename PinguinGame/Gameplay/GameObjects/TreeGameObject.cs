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
    public class TreeGameObject : DetailGameObject
    {
        public TreeGameObject(Vector2 position) : base(position)
        {
            Collider = new CircleCollider(new Circle(4));
            IsSolid = true;
        }

        public override void LoadSprites(IceGameGraphics graphics)
        {
            Sprite = graphics.Tree;
            Shadow = graphics.TreeShadow;
        }
    }
}
