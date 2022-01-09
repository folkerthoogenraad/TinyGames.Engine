using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;

namespace PinguinGame.Screens.UI
{
    public class PreGameUIModel
    {
        public int[] Scores;
        public Color[] Colors;
    }

    public class PreGameUI : GameUI
    {
        private PreGameUIModel Model { get; set; }

        public PreGameUI(GameUISkin skin, PreGameUIModel model) :base(skin)
        {
            SetModel(model);
        }

        public void SetModel(PreGameUIModel model)
        {
            Model = model;
        }

        public override void Draw(Graphics2D graphics)
        {
            float widthPerSegment = 32;
            float width = (Model.Scores.Length - 1) * widthPerSegment;
            float offset = -width / 2;

            for (int i = 0; i < Model.Scores.Length; i++)
            {
                string score = "" + Model.Scores[i];
                Color color = Model.Colors[i];

                graphics.DrawString(Skin.FontOutline, score, Bounds.Center + new Vector2(offset, 0), color, FontHAlign.Center, FontVAlign.Center);
                graphics.DrawString(Skin.Font, score, Bounds.Center + new Vector2(offset, 0), Color.White, FontHAlign.Center, FontVAlign.Center);

                offset += widthPerSegment;
            }
        }
    }
}
