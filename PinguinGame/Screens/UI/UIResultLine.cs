using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;
using TinyGames.Engine.UI;

namespace PinguinGame.Screens.UI
{
    public class UIResultLineModel
    {
        public string PlayerName { get; set; }
        public string WinningLabel { get; set; }
        public bool IsWinning { get; set; }

        public int Score { get; set; }
        public string ScoreLabel { get; set; }

        public string FullScoreLabel => Score + " " + ScoreLabel;

        public Sprite Icon { get; set; }
        public Color Color { get; set; } = Color.White;
    }

    public class UIResultLine : UIComponent
    {
        public Color TextColor { get; set; } = Color.White;
        public Color TextWinningColor { get; set; } = new Color(240, 163, 35);
        public Color TextGrayColor { get; set; } = new Color(153, 153, 153);

        public Font Font { get; set; }
        public NineSideSprite ResultLine { get; private set; }
        public NineSideSprite ResultOutline { get; private set; }

        public UIResultLineModel Model { get; set; }

        public UIResultLine(UIResultLineModel model, Font font, NineSideSprite resultLine, NineSideSprite resultOutline)
        {
            Font = font;
            ResultLine = resultLine;
            ResultOutline = resultOutline;

            Model = model;
        }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            graphics.DrawNineSideSprite(ResultLine, bounds.Position, bounds.Size, Model.Color);

            if (Model.IsWinning)
            {
                graphics.DrawNineSideSprite(ResultOutline, bounds.Position, bounds.Size);
            }

            graphics.DrawString(Font, Model.PlayerName, bounds.LeftCenter + new Vector2(10, 0), TextColor, FontHAlign.Left, FontVAlign.Center);

            if (Model.IsWinning)
            {
                graphics.DrawString(Font, Model.WinningLabel, bounds.LeftCenter + new Vector2(64, 0), TextWinningColor, FontHAlign.Left, FontVAlign.Center);
            }

            graphics.DrawString(Font, Model.FullScoreLabel, bounds.RightCenter - new Vector2(34, 0), Model.IsWinning ? TextGrayColor : TextColor, FontHAlign.Right, FontVAlign.Center);

            graphics.DrawSprite(Model.Icon, bounds.RightCenter + new Vector2(-14, -5));
        }
    }
}
