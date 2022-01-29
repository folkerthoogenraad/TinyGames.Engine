using Microsoft.Xna.Framework.Content;
using PinguinGame.IO.Character;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PinguinGame.MiniGames
{
    public interface IMiniGameLoader
    {
        public List<MiniGames.Generic.CharacterInfo> LoadCharacterInfo(string file);
    }
}
