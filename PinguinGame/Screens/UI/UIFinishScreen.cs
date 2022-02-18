using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Player;
using PinguinGame.Screens.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Animations;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;
using TinyGames.Engine.UI;
using TinyGames.Engine.UI.Animations;
using TinyGames.Engine.Util;

namespace PinguinGame.Screens.UI
{
    public class UIFinishScreen: UIComponent
    {
        private InGameResources _resources;

        private UISprite Finish;

        public UIFinishScreen(InGameResources resources)
        {
            _resources = resources;

            Finish = new UISprite()
            {
                Sprite = resources.Finish
            };

            Finish.SetAnimation(new UITransformAnimation(new Vector2(0, 16), new Vector2(1.0f, 1.0f)));
            AddComponent(Finish);
        }

        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Finish.UpdateLayout(AABB.Create(bounds.Center, Vector2.Zero));
        }

    }
}
