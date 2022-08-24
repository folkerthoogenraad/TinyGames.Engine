using System;
using System.IO;

namespace TinyGames.Engine.IO
{
    public interface IStorageProvider
    {
        public Stream OpenWrite(string file);
        public Stream OpenRead(string file);
    }
}
