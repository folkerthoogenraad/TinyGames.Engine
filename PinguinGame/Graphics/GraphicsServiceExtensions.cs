using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.Graphics
{
    public static class GraphicsServiceExtensions
    {
        public static int GetPreferredScalingFactor(this GraphicsService service, float minimumHeight)
        {
            return GetPreferredScalingFactor(service.ScreenHeight, minimumHeight);
        }

        public static int GetPreferredScalingFactor(float height, float minimumHeight)
        {
            int scaling = 1;

            while (height / (scaling + 1) >= minimumHeight) scaling++;

            return scaling;
        }
    }
}
