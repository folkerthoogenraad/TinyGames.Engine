using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class IceLevelGraphicsSettings
    {
        public Color SnowColor { get; set; } = new Color(255, 255, 255);
        public Color SnowColorHighlighted { get; set; } = new Color(255, 186, 186);
        public Color SideColor { get; set; } = new Color(213, 213, 213);
        public Color SideWaterColor { get; set; } = new Color(26, 84, 165);
        public Color WaterColor { get; set; } = new Color(22,55,100);
    }
}
