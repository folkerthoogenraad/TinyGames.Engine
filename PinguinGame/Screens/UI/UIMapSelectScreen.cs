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

    public class UIMapSelectScreen : UIComponent
    {
        public UIBackground Background { get; set; }
        public UISprite Title { get; set; }

        public UISprite[] MapOutlines { get; set; }

        public UIButton OkButton { get; set; }
        public UIButton TextBox { get; set; }


        private MapSelectResources _resources;

        private string[] _text = new string[] {
            "This is a nice map for the game",
            "Locked",
            "Locked",
            "Locked",
        };
        private bool _ready = false;
        private int _selectedIndex = -1;

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
                Text = _text[0],
                Background = resources.ButtonUnselected
            };

            OkButton = new UIButton()
            {
                Visible = false,
                Font = resources.Font,
                Text = "OK?",
                Background = resources.ButtonSelected
            };

            MapOutlines = new UISprite[4];

            for(int i = 0; i < MapOutlines.Length; i++)
            {
                MapOutlines[i] = new UISprite() { 
                    Sprite = resources.LevelUnselected,
                };
            }

            SetSelected(0);

            foreach(var map in MapOutlines)
            {
                map.SetAnimation(new UITransformAnimation(new Vector2(0, -64), new Vector2(1, 1)));
            }

            Title.SetAnimation(new UITransformAnimation(new Vector2(0, -64), new Vector2(1.5f, 1.5f)));
            TextBox.SetAnimation(new UITransformAnimation(new Vector2(0, -32), new Vector2(1, 1)));

            AddComponent(Background);
            AddComponent(Title);

            AddComponent(TextBox);
            AddComponent(OkButton);

            foreach (var map in MapOutlines)
            {
                AddComponent(map);
            }
        }

        public void SetSelected(int index)
        {
            if (index == _selectedIndex) return;

            _selectedIndex = index;

            foreach(var (i, outline) in MapOutlines.WithIndex())
            {
                if(_selectedIndex == i)
                {
                    outline.Sprite = _resources.LevelSelected;
                    outline.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.1f, 1.1f)));
                }
                else
                {
                    outline.Sprite = _resources.LevelUnselected;
                }
            }

            SetText(_text[_selectedIndex]);
        }

        private void SetText(string text)
        {
            TextBox.Text = text;
            TextBox.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.1f, 1.1f)));
        }

        public void SetReady(bool ready)
        {
            if (ready == _ready) return;

            _ready = ready;

            OkButton.Visible = _ready;
            OkButton.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.4f, 1.4f)));

            MapOutlines[_selectedIndex].SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.4f, 1.4f)));
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
            float offset = -widthPerItem * (MapOutlines.Length - 1) / 2;

            foreach(var map in MapOutlines)
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
