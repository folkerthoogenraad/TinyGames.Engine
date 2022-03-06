using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.IO;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Gameplay
{
    // TODO this can be refactored into something really nice I think.
    public class ShoppingGraphics : ISceneBehaviour
    {
        private Dictionary<string, Sprite> _sprites;
        private Dictionary<string, IOSprite> _registry;

        private ContentManager _content;
        
        public ShoppingGraphics(ContentManager content)
        {
            _content = content;

            _sprites = new Dictionary<string, Sprite>();
            _registry = new Dictionary<string, IOSprite>();

            // This is actually really nice, should be moved but still really nice.
            _registry.Add("fridge", new IOSprite("Sprites/Tilesets/SuperMarket", new Rectangle(80, 64, 16, 32)).SetOrigin(8, 32));
            _registry.Add("fridge_double", new IOSprite("Sprites/Tilesets/SuperMarket", new Rectangle(96, 64, 32, 32)).SetOrigin(16, 32));
            _registry.Add("crate_empty", new IOSprite("Sprites/Tilesets/SuperMarket", new Rectangle(80, 48, 16, 16)).SetOrigin(8, 16));
            _registry.Add("crate_apples", new IOSprite("Sprites/Tilesets/SuperMarket", new Rectangle(96, 48, 16, 16)).SetOrigin(8, 16));
        }

        public Sprite GetSprite(string name)
        {
            var cachedSprite = GetCachedSprite(name);

            if (cachedSprite != null) return cachedSprite;

            var registrySprite = GetRegistrySprite(name);
            var sprite = ConvertIOSprite(registrySprite);

            SetCachedSprite(name, sprite);

            return sprite;
        }

        private Sprite GetCachedSprite(string name)
        {
            if (_sprites.TryGetValue(name, out var sprite)) return sprite;
            return null;
        }

        private void SetCachedSprite(string name, Sprite sprite)
        {
            _sprites[name] = sprite;
        }

        private IOSprite GetRegistrySprite(string name)
        {
            if (!_registry.ContainsKey(name)) throw new ArgumentException($"Graphics don't contain '${name}'.");

            return _registry[name];
        }

        private Sprite ConvertIOSprite(IOSprite sprite)
        {
            var texture = _content.Load<Texture2D>(sprite.Texture);
            return new Sprite(texture, new Rectangle(sprite.X, sprite.Y, sprite.Width, sprite.Height)).SetOrigin(sprite.OriginX, sprite.OriginY);
        }

        // =========================================== //
        // Scene component
        // =========================================== //
        public void Init(Scene scene)
        {
        }

        public void BeforeUpdate(float delta)
        {
        }

        public void AfterUpdate(float delta)
        {
        }

        public void Destroy()
        {
        }
    }
}
