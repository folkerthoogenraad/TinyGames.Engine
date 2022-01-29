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
    public class UISelectPlayersModel
    {
        public enum PlayerState
        {
            UnJoined, Joined, Ready
        }

        public PlayerState[] PlayerStates { get; set; }
        public bool ShowContinueButton { get; set; }
        public bool FadeFoward { get; set; }
        public bool FadeBackwards { get; set; }
    }

    public class UISelectPlayers : UIComponent
    {
        public UIBackground Background { get; set; }
        public UISprite Title { get; set; }
        public UIIconButton[] Players { get; set; }
        public UIButton OK { get; set; }

        private PlayerSelectResources _resources;

        private UISelectPlayersModel _currentModel;

        public UISelectPlayers(PlayerSelectResources resources)
        {
            _resources = resources;

            Background = new UIBackground() { 
                BackgroundColor = resources.BackgroundColor,
                BackgroundOverlayColor = resources.BackgroundOverlayColor,
                BackgroundOverlay= resources.BackgroundOverlay,
            };

            Title = new UISprite()
            {
                Sprite = resources.TitlePlay
            };

            Title.SetAnimation(new UITransformAnimation(new Vector2(0, -16), new Vector2(1.2f, 1.2f)));

            OK = new UIButton()
            {
                Visible = false,
                Font = resources.Font,
                Text = "OK?",
                Background = resources.ButtonSelected,
            };

            Players = new UIIconButton[4];

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i] = new UIIconButton()
                {
                    Font = resources.Font,
                    Icon = resources.JoinIcon,
                    Text = "Join",
                    Background = resources.ButtonUnJoined,
                };

                Players[i].SetAnimation(new UITransformAnimation(GetDirectionVector(i) * 16, Vector2.One));
            }

            AddComponent(Background);
            AddComponent(Title);

            foreach (var player in Players)
            {
                AddComponent(player);
            }

            AddComponent(OK);

            _currentModel = new UISelectPlayersModel()
            {
                PlayerStates = new UISelectPlayersModel.PlayerState[] { 
                    UISelectPlayersModel.PlayerState.UnJoined,
                    UISelectPlayersModel.PlayerState.UnJoined,
                    UISelectPlayersModel.PlayerState.UnJoined,
                    UISelectPlayersModel.PlayerState.UnJoined
                },
                ShowContinueButton = false,
            };
        }

        public void SetModel(UISelectPlayersModel model)
        {
            foreach(var (index, state) in model.PlayerStates.WithIndex())
            {
                if(_currentModel.PlayerStates[index] == state)
                {
                    continue;
                }

                // TODO play animation on change.
                var ui = Players[index];

                ui.SetAnimation(new UITransformAnimation(Vector2.Zero, new Vector2(1.2f, 1.2f)));

                ui.Background = GetSpriteForState(state);

                if(state == UISelectPlayersModel.PlayerState.UnJoined)
                {
                    ui.Text = "Join";
                    ui.Icon = _resources.JoinIcon;
                    ui.IconOverlay = null;
                }
                else
                {
                    ui.Text = $"Player {index + 1}";
                    ui.Icon = null;
                    ui.IconOverlay = null;
                }
            }

            if(_currentModel.ShowContinueButton != model.ShowContinueButton)
            {
                OK.Visible = model.ShowContinueButton;
                OK.SetAnimation(new UIBounceAnimation());
            }

            if (_currentModel.FadeFoward != model.FadeFoward && model.FadeFoward)
            {
                foreach (var (index, state) in model.PlayerStates.WithIndex())
                {
                    if (state == UISelectPlayersModel.PlayerState.Ready)
                    {
                        Players[index].SetAnimation(new UITransformAnimation(Vector2.Zero, Vector2.One)
                        {
                            TargetPosition = GetDirectionVector(index) * 16
                        });
                    }
                    else
                    {
                        Players[index].SetAnimation(new UITransformAnimation(Vector2.Zero, Vector2.One)
                        {
                            TargetPosition = new Vector2(0, 64)
                        });
                    }

                }
            }

            if (_currentModel.FadeBackwards != model.FadeBackwards && model.FadeBackwards)
            {
                foreach (var (index, state) in model.PlayerStates.WithIndex())
                {
                    Players[index].SetAnimation(new UITransformAnimation(Vector2.Zero, Vector2.One)
                    {
                        TargetPosition = GetDirectionVector(index) * 8
                    });
                }
            }

            _currentModel = model;
        }

        private NineSideSprite GetSpriteForState(UISelectPlayersModel.PlayerState state)
        {
            switch (state)
            {
                case UISelectPlayersModel.PlayerState.Ready: return _resources.ButtonReady;
                case UISelectPlayersModel.PlayerState.Joined: return _resources.ButtonJoined;
                default: return _resources.ButtonUnJoined;
            }
        }

        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Background.UpdateLayout(bounds);
            Title.UpdateLayout(AABB.Create(bounds.TopCenter + new Vector2(0, 32), Vector2.Zero));

            Vector2 center = bounds.Center + new Vector2(0, 16);
            Vector2 size = new Vector2(80, 32);
            Vector2 offset = new Vector2(2, 2);

            OK.UpdateLayout(AABB.CreateCentered(center, new Vector2(64, 24)));

            Players[0].UpdateLayout(AABB.Create(center + new Vector2(-offset.X - size.X, -size.Y - offset.Y), size));
            Players[1].UpdateLayout(AABB.Create(center + new Vector2(offset.X, -size.Y - offset.Y), size));
            Players[2].UpdateLayout(AABB.Create(center + new Vector2(-offset.X - size.X, offset.Y), size));
            Players[3].UpdateLayout(AABB.Create(center + new Vector2(offset.X, offset.Y), size));
        }

        private Vector2 GetDirectionVector(int i)
        {
            var dir = new Vector2(1, 1);

            if (i == 0) dir = new Vector2(-1, -1);
            if (i == 1) dir = new Vector2(1, -1);
            if (i == 2) dir = new Vector2(-1, 1);
            if (i == 3) dir = new Vector2(1, 1);

            return dir.Normalized();
        }

    }
}
