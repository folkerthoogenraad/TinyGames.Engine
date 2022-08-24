using Microsoft.Xna.Framework.Content;
using System;
using System.IO;

namespace TinyGames.Engine.IO
{
    public class ContentManagerStorageProvider : IStorageProvider
    {
        public ContentManager Content { get; set; }

        public ContentManagerStorageProvider(ContentManager content)
        {
            Content = content;
        }

        public Stream OpenRead(string file)
        {
            return File.OpenRead(Path.ChangeExtension(file, null));
        }

        public Stream OpenWrite(string file)
        {
            throw new NotImplementedException();
        }
    }
}
