using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Graphics
{
    public class GraphicsUtils
    {
        public static Color ParseColor(string input)
        {
            if (input == null) throw new ArgumentException("Input is null");
            if(!input.StartsWith('#')) throw new ArgumentException("Color doesn't start with hashtag");

            string numbers = input.Substring(1);

            if(numbers.Length == 6) // RGB
            {
                int result = Convert.ToInt32(numbers, 16);

                int r = (byte)(result >> 16);
                int g = (byte)(result >> 8);
                int b = (byte)(result >> 0);

                return new Color(r, g, b);
            }
            else if(numbers.Length == 8) // ARGB
            {
                int result = Convert.ToInt32(numbers, 16);

                int a = (byte)(result >> 24);
                int r = (byte)(result >> 16);
                int g = (byte)(result >> 8);
                int b = (byte)(result >> 0);

                return new Color(r, g, b, a);
            }
            else
            {
                throw new ArgumentException($"Cannot parse ${input}. Not in RGB or ARGB format.");
            }
        }
    }
}
