using System;
using System.IO;
using System.Text.Json;

namespace TinyGames.Engine.IO
{
    public class StorageSystem : IStorageSystem
    {
        private IStorageProvider _storageProvider;

        public StorageSystem(IStorageProvider provider)
        {
            _storageProvider = provider;
        }

        public T Load<T>(string path, T @default)
        {
            try
            {
                using (StreamReader reader = new StreamReader(_storageProvider.OpenRead(path)))
                {
                    string json = reader.ReadToEnd();

                    return JsonSerializer.Deserialize<T>(json);
                }
            }
            catch
            {
                return @default;
            }
        }

        public void Save<T>(string path, T obj)
        {
            var json = JsonSerializer.Serialize(obj);

            using (StreamWriter writer = new StreamWriter(_storageProvider.OpenWrite(path)))
            {
                writer.Write(json);
            }
        }
    }
}
