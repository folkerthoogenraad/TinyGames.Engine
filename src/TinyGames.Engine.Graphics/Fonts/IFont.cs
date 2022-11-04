using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyGames.Engine.Graphics.Fonts
{
    public interface IFont
    {
        public Vector2 Measure(string t);
    }
}
