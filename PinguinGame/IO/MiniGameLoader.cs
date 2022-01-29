using Microsoft.Xna.Framework.Content;
using PinguinGame.IO.Character;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PinguinGame.MiniGames;
using System.IO;

namespace PinguinGame.IO
{
    public class MiniGameLoader : IMiniGameLoader
    {
        private ContentManager _content;
        private IStorageSystem _storage;

        public MiniGameLoader(IStorageSystem storage, ContentManager content)
        {
            _content = content;
            _storage = storage;
        }

        public List<MiniGames.Generic.CharacterInfo> LoadCharacterInfo(string file)
        {
            var infos = _storage.Load<CharactersInfo>(file);

            return infos.Characters.Select(x => ConvertInfo(x)).ToList();
        }

        private MiniGames.Generic.CharacterInfo ConvertInfo(CharacterInfo data)
        {
            var info = new MiniGames.Generic.CharacterInfo();

            info.Name = data.Name;
            info.Identifier = data.Identifier;
            info.Icon = ConvertSprite(data.Icon);
            info.Graphics = new MiniGames.Generic.CharacterGraphics(_content.Load<Texture2D>(data.Sheet));

            return info;
        }

        private TinyGames.Engine.Graphics.Sprite ConvertSprite(Sprite input)
        {
            var texture = _content.Load<Texture2D>(input.Texture);

            var spr = new TinyGames.Engine.Graphics.Sprite(texture, new Rectangle(input.X, input.Y, input.Width, input.Height));

            spr.SetOrigin(input.OriginX, input.OriginY);

            return spr;
        }
    }
}
