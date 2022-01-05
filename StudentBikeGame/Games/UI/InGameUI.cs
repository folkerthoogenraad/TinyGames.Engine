using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;

namespace StudentBikeGame.Games.UI
{
    public class InGameUI
    { 
        public Font Font { get; set; }
        public Font FontOutline { get; set; }



        public Color _scoreColor { get; set; }
        public Vector2 _scoreScale { get; set; }
        private float _score { get; set; }
        private int _targetScore { get; set; }

        public InGameUI(Font font, Font fontOutline)
        {
            Font = font;
            FontOutline = fontOutline;
        }

        public void Update(int score, float delta)
        {
            if(_targetScore != score)
            {
                _scoreScale = new Vector2(1.5f, 1.5f);
                _scoreColor = new Color(233, 186, 96);
                _targetScore = score;
            }

            _scoreColor = Color.Lerp(_scoreColor, Color.White, delta * 5);
            _score = Tools.Lerp(_score, _targetScore, delta * 5);
            _scoreScale = Vector2.Lerp(_scoreScale, Vector2.One, delta * 5);
        }

        public void Draw(Graphics2D graphics, AABB bounds)
        {
            var v = bounds.TopCenter + new Vector2(0, 32);

            graphics.DrawString(Font, "" + (int)MathF.Ceiling(_score), v, _scoreScale * 2, _scoreColor, FontHAlign.Center, FontVAlign.Center);
            graphics.DrawString(FontOutline, "" + (int)MathF.Ceiling(_score), v, _scoreScale * 2, Color.Black, FontHAlign.Center, FontVAlign.Center);
        }
    }
}
