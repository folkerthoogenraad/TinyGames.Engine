using Microsoft.Xna.Framework;
using PinguinGame.Screens.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;
using TinyGames.Engine.UI;

namespace PinguinGame.Screens.UI
{
    public class PreGameUIModel
    {
        public int[] Scores;
        public Color[] Colors;
    }

    public class UIPreGame : UIComponent
    {
        public InGameResources Resources { get; set; }
        private PreGameUIModel Model { get; set; }

        public UIPreGame(InGameResources resources, PreGameUIModel model)
        {
            Resources = resources;
            SetModel(model);
        }

        public void SetModel(PreGameUIModel model)
        {
            Model = model;
        }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            float widthPerSegment = 32;
            float width = (Model.Scores.Length - 1) * widthPerSegment;
            float offset = -width / 2;

            for (int i = 0; i < Model.Scores.Length; i++)
            {
                string score = "" + Model.Scores[i];
                Color color = Model.Colors[i];

                // TODO draw scores here.
                graphics.DrawString(Resources.Font, score, bounds.Center + new Vector2(offset, 2), new Vector2(2, 2), Color.Black, FontHAlign.Center, FontVAlign.Center);
                graphics.DrawString(Resources.Font, score, bounds.Center + new Vector2(offset, 0), new Vector2(2, 2), color, FontHAlign.Center, FontVAlign.Center);

                offset += widthPerSegment;
            }
        }
    }
}
