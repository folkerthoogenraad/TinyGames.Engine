using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Graphics
{
    public static class NineSideSpriteExtension
    {
        public static void DrawNineSideSprite(this Graphics2D graphics, NineSideSprite sprite, Vector2 position, Vector2 size)
        {
            sprite.Draw(graphics, position, size);
        }
    }
}
