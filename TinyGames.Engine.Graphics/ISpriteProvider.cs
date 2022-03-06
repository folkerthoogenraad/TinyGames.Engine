using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Graphics
{
    public interface ISpriteProvider
    {
        Sprite GetSprite(string name);
    }
}
