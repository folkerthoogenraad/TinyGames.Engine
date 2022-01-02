using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators.Contracts;

namespace TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators
{
    public static class FontContentManagerExtension
    {
        public static Font LoadFont(this ContentManager manager, string asset)
        {
            var texture = manager.Load<Texture2D>(asset);

            FontLoaderSettings settings;

            using(var reader = new StreamReader(TitleContainer.OpenStream(manager.RootDirectory + Path.DirectorySeparatorChar + asset + ".json")))
            {
                string input = reader.ReadToEnd();

                settings = JsonSerializer.Deserialize<FontLoaderSettings>(input, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            settings.Texture = texture;

            return FontLoader.Load(settings);
        }
    }
}
