using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Levels.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TinyGames.Engine.Graphics;

namespace PinguinGame.Levels
{
    public class LevelLoader
    {
        public TiledLevel Level { get; }
        public string Folder { get; }

        private ContentManager _content;

        private Dictionary<string, Texture2D> _textures;
        private Dictionary<int, Sprite> _sprites;

        public LevelLoader(ContentManager manager, string asset)
        {
            _content = manager;

            var contentPath =  asset + ".json";
            var path = manager.RootDirectory + Path.DirectorySeparatorChar + contentPath;
            Folder = Path.GetDirectoryName(contentPath);

            using (var reader = new StreamReader(TitleContainer.OpenStream(path)))
            {
                string input = reader.ReadToEnd();

                var level = JsonSerializer.Deserialize<TiledLevel>(input, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });

                Level = level;
            }

            _textures = new Dictionary<string, Texture2D>();
            _sprites = new Dictionary<int, Sprite>();
        }

        public Sprite GetSpriteForTile(int tile)
        {
            Sprite sprite;

            if(_sprites.TryGetValue(tile, out sprite))
            {
                return sprite;
            }

            sprite = LoadSpriteForTile(tile);
            _sprites.Add(tile, sprite);

            return sprite;
        }

        private Sprite LoadSpriteForTile(int tile)
        {
            var tileset = GetTilesetForTile(tile);

            if (tileset == null) return null;

            var texture = GetTextureFromPath(tileset.Image);

            int tileIndex = tile - tileset.FirstGID;
            int tileX = tileIndex % tileset.Columns;
            int tileY = tileIndex / tileset.Columns;

            return new Sprite(texture, new Rectangle(
                tileX * tileset.TileWidth, 
                tileY * tileset.TileHeight, 
                tileset.TileWidth, 
                tileset.TileHeight));
        }

        private Texture2D GetTextureFromPath(string path)
        {
            Texture2D texture;

            if(_textures.TryGetValue(path, out texture))
            {
                return texture;
            }

            texture = LoadTextureFromPath(path);
            _textures.Add(path, texture);

            return texture;
        }

        private Texture2D LoadTextureFromPath(string path)
        {
            var rawPath = Path.ChangeExtension(path, null); // null = no extension
            var fullPath = Folder + Path.DirectorySeparatorChar + rawPath;

            return _content.Load<Texture2D>(fullPath);
        }

        public TiledTileset GetTilesetForTile(int tile)
        {
            return Level.Tilesets.Where(x => 
                tile >= x.FirstGID 
                && tile < x.FirstGID + x.TileCount)
                .FirstOrDefault();
        }
    }
}
