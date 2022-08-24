using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Extensions
{
    public static class GameTimeExtension
    {
        public static float GetDeltaInSeconds(this GameTime time)
        {
            return (float)time.ElapsedGameTime.TotalSeconds;
        }
    }
}
