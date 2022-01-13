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
    public class UIMapModel
    {
        public string Text = "";
        public bool Locked = false;
    }

    public class UIMapSelectModel
    {
        public UIMapModel[] Maps;
        public bool Ready = false;
        public int SelectedIndex = 0;
    }


    public class UIMapButton : UIComponent
    {
        public Sprite Outline { get; set; }
        public Sprite Sprite { get; set; }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            graphics.DrawSprite(Outline, bounds.Center);
            graphics.DrawSprite(Sprite, bounds.Center);
        }
    }

    public class UIMapSelectScreen : UIComponent
    {
        public UIBackground Background { get; set; }
        public UISprite Title { get; set; }

        public UIMapButton[] MapButtons { get; set; }

        public UIButton OkButton { get; set; }
        public UIButton TextBox { get; set; }


        private MapSelectResources _resources;

        private UIMapSelectModel _currentModel;

        public UIMapSelectScreen(MapSelectResources resources)
        {
            _resources = resources;

            Background = new UIBackground() { 
                BackgroundColor = resources.BackgroundColor,
                BackgroundOverlayColor = resources.BackgroundOverlayColor,
                BackgroundOverlay= resources.BackgroundOverlay,
            };

            Title = new UISprite()
            {
                Sprite = resources.TitleMap
            };

            TextBox = new UIButton()
            {
                Font = resources.Font,
                Text = "",
                Background = resources.ButtonUnselected
            };

            OkButton = new UIButton()
            {
                Visible = false,
                Font = resources.Font,
                Text = "OK?",
                Background = resources.ButtonSelected
            };

            MapButtons = new UIMapButton[4];

            for(int i = 0; i < MapButtons.Length; i++)
            {
                MapButtons[i] = new UIMapButton() { 
                    Sprite = resources.LevelIconLocked,
                    Outline = resources.LevelUnselected,
                };
            }

            foreach(var map in MapButtons)
            {
                map.SetAnimation(new UITransformAnimation(new Vector2(0, -64), new Vector2(1, 1)));
            }

            Title.SetAnimation(new UITransformAnimation(new Vector2(0, -64), new Vector2(1.5f, 1.5f)));
            TextBox.SetAnimation(new UITransformAnimation(new Vector2(0, -32), new Vector2(1, 1)));

            AddComponent(Background);
            AddComponent(Title);

            AddComponent(TextBox);
            AddComponent(OkButton);

            foreach (var map in MapButtons)
            {
                AddComponent(map);
            }
        }

        public void SetModel(UIMapSelectModel model, bool animate = true)
        {

            foreach (var (i, button) in MapButtons.WithIndex())
            {
                var map = model.Maps[i];

                button.Sprite = map.Locked ? _resources.LevelIconLocked : _resources.LevelIcon;
            }

            SetSelected(model.SelectedIndex, model.Maps[model.SelectedIndex].Text, animate);
            SetReady(model.Ready, model.SelectedIndex, animate);

            _currentModel = model;
        }

        private void SetSelected(int index, string text, bool animate = true)
        {
            if (_currentModel != null && index == _currentModel.SelectedIndex) return;

            foreach(var (i, button) in MapButtons.WithIndex())
            {
                if(index == i)
                {
                    button.Outline = _resources.LevelSelected;

                    if (animate)
                    {
                        button.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.1f, 1.1f)));
                    }
                }
                else
                {
                    button.Outline = _resources.LevelUnselected;
                }
            }

            SetText(text, animate);
        }

        private void SetText(string text, bool animate = true)
        {
            TextBox.Text = text;

            if (animate)
            {
                TextBox.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.1f, 1.1f)));
            }
        }

        private void SetReady(bool ready, int index, bool animate = true)
        {
            if (_currentModel != null && ready == _currentModel.Ready) return;

            MapButtons[index].Outline = ready ? _resources.LevelReady : _resources.LevelSelected;
            OkButton.Visible = ready;

            if (animate)
            {
                OkButton.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.4f, 1.4f)));
                MapButtons[index].SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.4f, 1.4f)));
            }
        }

        public void AcceptAnimation()
        {
            OkButton.Background = _resources.ButtonPressed;
            OkButton.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.1f, 1.1f)));
        }

        public void BackAnimation()
        {
            Title.SetAnimation(new UITransformAnimation(Vector2.Zero, new Vector2(1, 1))
            {
                TargetPosition = new Vector2(0, -64)
            });
            TextBox.SetAnimation(new UITransformAnimation(Vector2.Zero, new Vector2(1, 1))
            {
                TargetPosition = new Vector2(0, 32)
            });
        }

        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Background.UpdateLayout(bounds); 
            Title.UpdateLayout(AABB.Create(bounds.TopCenter + new Vector2(0, 32), Vector2.Zero));

            float widthPerItem = 32 + 8;
            float offset = -widthPerItem * (MapButtons.Length - 1) / 2;

            foreach(var map in MapButtons)
            {
                map.UpdateLayout(AABB.CreateCentered(bounds.Center + new Vector2(offset, -16), new Vector2(32, 32)));

                offset += widthPerItem;
            }

            var textBoxBounds = AABB.CreateCentered(bounds.Center + new Vector2(0, 32), new Vector2(160, 48));

            TextBox.UpdateLayout(textBoxBounds);
            OkButton.UpdateLayout(AABB.CreateCentered(textBoxBounds.BottomRight, new Vector2(64, 24)));
        }

    }
}
