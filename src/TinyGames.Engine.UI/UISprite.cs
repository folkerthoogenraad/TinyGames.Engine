using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.UI
{
    public class UISprite : UIComponent
    {
        public Sprite Sprite { get; set; }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            graphics.DrawSprite(Sprite, bounds.Center);
        }
    }

}
