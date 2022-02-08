using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Player;
using PinguinGame.Screens.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Animations;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;
using TinyGames.Engine.UI;
using TinyGames.Engine.UI.Animations;
using TinyGames.Engine.Util;

namespace PinguinGame.Screens.UI
{

    public class UICharacterSelectModel
    {
        public Color[] Colors;
        public int[] SelectedIndices;
        public bool[] Ready;
        public Sprite[] CharacterIcons;

        public bool DisplayOk = false;
    }

    public class UICharacterSelect : UIComponent
    {
        public UIBackground Background { get; set; }
        public UISprite Title { get; set; }
        public UIButton OK { get; set; }
        public UIIconButton[] CharacterButtons { get; set; }

        private CharacterSelectResources _resources;
        private UICharacterSelectModel _currentModel;

        public UICharacterSelect(CharacterSelectResources resources, UICharacterSelectModel model)
        {
            _currentModel = model;
            _resources = resources;

            Background = new UIBackground() { 
                BackgroundColor = resources.BackgroundColor,
                BackgroundOverlayColor = resources.BackgroundOverlayColor,
                BackgroundOverlay= resources.BackgroundOverlay,
            };

            Title = new UISprite()
            {
                Sprite = resources.TitleCharacterSelect
            };

            Title.SetAnimation(new UITransformAnimation(new Vector2(0, -16), new Vector2(1.2f, 1.2f)));

            OK = new UIButton()
            {
                Visible = false,
                Font = resources.Font,
                Text = "OK?",
                Background = resources.ButtonSelected,
            };

            CharacterButtons = model.CharacterIcons.Select(x => {
                return new UIIconButton() { 
                    Icon = x,
                    Background = null,
                };
            }).ToArray();

            AddComponent(Background);
            AddComponent(Title);
            AddComponent(OK);

            foreach (var button in CharacterButtons) AddComponent(button);

            Title.SetAnimation(new UITransformAnimation(new Vector2(0, -64), new Vector2(1.5f, 1.5f)));

            SetModel(model);
        }
        public void FadeOut()
        {
            OK.Background = _resources.ButtonPressed;
            OK.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.1f, 1.1f)));
        }

        public void SetModel(UICharacterSelectModel model)
        {
            foreach (var button in CharacterButtons)
            {
                button.Background = null;
            }

            for(int i = 0; i < model.SelectedIndices.Length; i++)
            {
                int index = model.SelectedIndices[i];
                bool ready = model.Ready[i];
                Color color = model.Colors[i];

                CharacterButtons[index].Background = _resources.ButtonOutline;
                CharacterButtons[index].BackgroundColor = ready ? Color.White : color;

                if(ready == true && ready != _currentModel.Ready[i])
                {
                    CharacterButtons[index].SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.1f, 1.1f)));
                }
            }

            if(_currentModel.DisplayOk != model.DisplayOk)
            {
                OK.Visible = model.DisplayOk;
                OK.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.3f, 1.3f)));
            }

            _currentModel = model;
        }

        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Background.UpdateLayout(bounds);
            Title.UpdateLayout(AABB.Create(bounds.TopCenter + new Vector2(0, 32), Vector2.Zero));

            Vector2 center = bounds.Center + new Vector2(0, 0);
            
            float width = 24;
            float spacing = 8;

            float totalWidth = width * CharacterButtons.Length + spacing * (CharacterButtons.Length - 1);

            float offset = -totalWidth / 2;

            foreach(var button in CharacterButtons)
            {
                button.UpdateLayout(AABB.Create(center + new Vector2(offset, -width / 2), new Vector2(width, width)));

                offset += width + spacing;
            }

            OK.UpdateLayout(AABB.CreateCentered(center + new Vector2(0, 32), new Vector2(64, 24)));
        }

        public void AcceptAnimation()
        {
            OK.Background = _resources.ButtonPressed;
            OK.SetAnimation(new UITransformAnimation(new Vector2(0, 0), new Vector2(1.1f, 1.1f)));
        }

        public void BackAnimation()
        {
            Title.SetAnimation(new UITransformAnimation(Vector2.Zero, new Vector2(1, 1))
            {
                TargetPosition = new Vector2(0, -64)
            });
        }
    }
}
