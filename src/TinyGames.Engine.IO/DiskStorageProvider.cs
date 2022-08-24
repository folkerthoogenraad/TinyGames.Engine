using System;
using System.IO;

namespace TinyGames.Engine.IO
{
    public class DiskStorageProvider : IStorageProvider
    {
        public string RootDirectory { get; set; }

        public DiskStorageProvider(string rootDirectory = "")
        {
            RootDirectory = rootDirectory;
        }

        public Stream OpenRead(string file)
        {
            var path = ConvertToPath(file);

            return File.OpenRead(path);
        }

        public Stream OpenWrite(string file)
        {
            var path = ConvertToPath(file);
            var directory = Directory.GetParent(path);

            var absolute = Path.GetFullPath(path);

            if (!directory.Exists)
            {
                directory.Create();
            }

            return File.OpenWrite(path);
        }

        private string ConvertToPath(string file)
        {
            if(RootDirectory.Length > 0)
            {
                return RootDirectory + Path.DirectorySeparatorChar + file;
            }
            else {  return file; }
        }
    }
}
