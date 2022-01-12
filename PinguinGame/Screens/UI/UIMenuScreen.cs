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

    public class UIMenuScreen : UIComponent
    {
        public UIBackground Background { get; set; }
        public UISprite Title { get; set; }

        public UIIconButton PlayButton { get; set; }
        public UIIconButton SettingsButton { get; set; }
        public UIIconButton ExitButton { get; set; }

        private MenuResources _resources;

        private int SelectedIndex = 0;

        public UIMenuScreen(MenuResources resources)
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

            PlayButton = new UIIconButton()
            {
                Font = resources.Font,
                Background = resources.ButtonSelected,
                Text = "Play",
                Icon = resources.PlayIcon
            };
            SettingsButton = new UIIconButton()
            {
                Font = resources.Font,
                Background = resources.ButtonUnselected,
                Text = "Settings",
                Icon = resources.SettingsIcon
            };
            ExitButton = new UIIconButton()
            {
                Font = resources.Font,
                Background = resources.ButtonUnselected,
                Text = "Exit",
                Icon = resources.ExitIcon
            };

            Title.SetAnimation(new UITransformAnimation(new Vector2(0, -64), new Vector2(1.5f, 1.5f)));

            PlayButton.SetAnimation(new UITransformAnimation(new Vector2(0, 16), Vector2.One));
            SettingsButton.SetAnimation(new UITransformAnimation(new Vector2(0, 32), Vector2.One));
            ExitButton.SetAnimation(new UITransformAnimation(new Vector2(0, 48), Vector2.One));

            AddComponent(Background);

            AddComponent(PlayButton);
            AddComponent(SettingsButton);
            AddComponent(ExitButton);

            AddComponent(Title);
        }
        public void SetSelected(int index)
        {
            if (index == SelectedIndex) return;

            SelectedIndex = index;

            PlayButton.Background = _resources.ButtonUnselected;
            SettingsButton.Background = _resources.ButtonUnselected;
            ExitButton.Background = _resources.ButtonUnselected;

            UIIconButton icon = GetSelectedButton(index);

            if(icon != null)
            {
                icon.Background = _resources.ButtonSelected;
                icon.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.1f, 1.1f)));
            }
        }

        public void AcceptAnimation()
        {
            UIIconButton icon = GetSelectedButton(SelectedIndex);

            if (icon != null)
            {
                icon.Background = _resources.ButtonPressed;
                icon.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.2f, 1.2f))
                {
                    TargetPosition = new Vector2(-16, 0)
                });
            }

            var transform = new UITransformAnimation(new Vector2(0, 0), new Vector2(1, 1)) {
                TargetPosition = new Vector2(64, 0),
            };

            if (icon != PlayButton) PlayButton.SetAnimation(transform); 
            if (icon != SettingsButton) SettingsButton.SetAnimation(transform);
            if (icon != ExitButton) ExitButton.SetAnimation(transform);
        }

        public void BackAnimation()
        {
            Title.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1, 1))
            {
                TargetPosition = new Vector2(0, -64),
            });

            PlayButton.Background = _resources.ButtonUnselected;
            SettingsButton.Background = _resources.ButtonUnselected;
            ExitButton.Background = _resources.ButtonUnselected;

            PlayButton.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1, 1))
            {
                TargetPosition = new Vector2(128, 0),
            });
            SettingsButton.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1, 1))
            {
                TargetPosition = new Vector2(128, 0),
            });
            ExitButton.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1, 1))
            {
                TargetPosition = new Vector2(128, 0),
            });
        }

        private UIIconButton GetSelectedButton(int index)
        {
            if (index == 0) return PlayButton;
            if (index == 1) return SettingsButton;
            if (index == 2) return ExitButton;

            return null;
        }

        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Background.UpdateLayout(bounds);
            Title.UpdateLayout(AABB.Create(bounds.TopLeft + new Vector2(120, 50), Vector2.Zero));

            Vector2 offset = bounds.Center;

            PlayButton.UpdateLayout(AABB.Create(offset, new Vector2(120, 24)));
            SettingsButton.UpdateLayout(AABB.Create(offset + new Vector2(0, 28), new Vector2(120, 24)));
            ExitButton.UpdateLayout(AABB.Create(offset + new Vector2(0, 28 * 2), new Vector2(120, 24)));
        }

    }
}
