using Microsoft.Xna.Framework.Content;
using PinguinGame.IO.Characters;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PinguinGame.Gameplay;
using System.IO;
using PinguinGame.IO.Levels;

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

        public List<Gameplay.LevelInfo> LoadLevelInfos(string file)
        {
            var infos = _storage.Load<LevelsInfo>(file);

            return infos.Levels.Select(x => ConvertInfo(x)).ToList();
        }

        public List<Gameplay.CharacterInfo> LoadCharacterInfos(string file)
        {
            var infos = _storage.Load<CharactersInfo>(file);

            return infos.Characters.Select(x => ConvertInfo(x)).ToList();
        }

        private Gameplay.LevelInfo ConvertInfo(Levels.LevelInfo data)
        {
            var info = new Gameplay.LevelInfo();

            info.Name = data.Name;
            info.Identifier = data.Identifier;
            info.Description = data.Description;
            info.File = data.File;
            info.Icon = ConvertSprite(data.Icon);

            return info;
        }

        private Gameplay.CharacterInfo ConvertInfo(Characters.CharacterInfo data)
        {
            var info = new Gameplay.CharacterInfo();

            info.Name = data.Name;
            info.Identifier = data.Identifier;
            info.Icon = ConvertSprite(data.Icon);
            info.Graphics = new Gameplay.CharacterGraphics(_content.Load<Texture2D>(data.Sheet));
            info.Sound = CharacterSound.CreateCharacterSound(_content);

            return info;
        }

        private TinyGames.Engine.Graphics.Sprite ConvertSprite(Sprite input)
        {
            var texture = _content.Load<Texture2D>(input.Texture);

            var spr = new TinyGames.Engine.Graphics.Sprite(texture, new Rectangle(input.X, input.Y, input.Width, input.Height));

            spr.SetOrigin(input.OriginX, input.OriginY);

            return spr;
        }

        List<Gameplay.CharacterInfo> IMiniGameLoader.LoadCharacterInfos(string file)
        {
            throw new NotImplementedException();
        }

        List<Gameplay.LevelInfo> IMiniGameLoader.LoadLevelInfos(string file)
        {
            throw new NotImplementedException();
        }
    }
}
