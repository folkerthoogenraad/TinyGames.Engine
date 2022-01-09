using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;

namespace PinguinGame.Screens.UI
{
    public class CenterTextUI : GameUI
    {
        public Color Color { get; set; } = Color.White;

        private string _text;
        private Vector2 _scale = new Vector2(2, 2);

        public string Text
        {
            get => _text;
            set {
                if (value != _text)
                {
                    _text = value;
                    _scale = new Vector2(2, 2);
                }
            }
        }

        public CenterTextUI(GameUISkin skin, string text) :base(skin)
        {
            Text = text;
        }

        public override void Update(float delta, AABB bounds)
        {
            base.Update(delta, bounds);

            _scale = Vector2.Lerp(_scale, Vector2.One, delta * 5);

        }

        public override void Draw(Graphics2D graphics)
        {
            graphics.DrawString(Skin.FontOutline, Text, Bounds.Center, _scale, Color.Black, FontHAlign.Center, FontVAlign.Center);
            graphics.DrawString(Skin.Font, Text, Bounds.Center, _scale, Color, FontHAlign.Center, FontVAlign.Center);
        }
    }
}
