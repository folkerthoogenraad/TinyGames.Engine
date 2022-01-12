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

    public class UITitleScreen : UIComponent
    {
        public UIBackground Background { get; set; }
        public UISprite Title { get; set; }
        public UISprite IceBlock { get; set; }
        public UISprite Penguin { get; set; }
        public UISprite InsertCoin { get; set; }

        private TitleResources _resources;

        public UITitleScreen(TitleResources resources)
        {
            _resources = resources;

            Background = new UIBackground() { 
                BackgroundColor = resources.BackgroundColor,
                BackgroundOverlayColor = resources.BackgroundOverlayColor,
                BackgroundOverlay= resources.BackgroundOverlay,
            };

            Title = new UISprite()
            {
                Sprite = resources.Title
            };
            IceBlock = new UISprite()
            {
                Sprite = resources.IceBlock
            };
            Penguin = new UISprite()
            {
                Sprite = resources.Penguin
            };
            InsertCoin = new UISprite()
            {
                Sprite = resources.InsertCoin
            };

            Title.SetAnimation(new UITransformAnimation(Vector2.Zero, new Vector2(2, 2)));
            Penguin.SetAnimation(new UITransformAnimation(new Vector2(64, 64), Vector2.One));
            IceBlock.SetAnimation(new UITransformAnimation(new Vector2(16, -16), Vector2.One));
            InsertCoin.SetAnimation(new UITransformAnimation(new Vector2(16, -16), new Vector2(1, 1)));

            AddComponent(Background);
            AddComponent(IceBlock);
            AddComponent(Penguin);
            AddComponent(InsertCoin);
            AddComponent(Title);
        }
        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Background.UpdateLayout(bounds);
            Title.UpdateLayout(AABB.Create(bounds.TopLeft + new Vector2(120, 64), Vector2.Zero));
            Penguin.UpdateLayout(AABB.Create(bounds.BottomRight - new Vector2(56, 56), Vector2.Zero));
            InsertCoin.UpdateLayout(AABB.Create(bounds.BottomLeft + new Vector2(72, -24), Vector2.Zero));
            IceBlock.UpdateLayout(AABB.Create(bounds.Center, Vector2.Zero));
        }

        public void FadeOut()
        {
            Title.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1, 1))
            {
                TargetPosition = new Vector2(0, -64),
            });
            InsertCoin.SetAnimation(new UITransformAnimation(Vector2.Zero, new Vector2(1.4f, 1.4f)));
            Penguin.SetAnimation(new UITransformAnimation(Vector2.Zero, Vector2.One)
            {
                TargetPosition = new Vector2(64, 64)
            });
        }

    }
}
