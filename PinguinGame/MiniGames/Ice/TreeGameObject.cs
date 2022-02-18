using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.MiniGames.Ice
{
    public class TreeGameObject : IceDetailGameObject
    {
        public TreeGameObject(Vector2 position) : base(position)
        {
        }

        public override void LoadSprites(IceGameGraphics graphics)
        {
            Sprite = graphics.Tree;
            Shadow = graphics.TreeShadow;
        }
    }
}
