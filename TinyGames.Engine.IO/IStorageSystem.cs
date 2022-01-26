using System;

namespace TinyGames.Engine.IO
{
    public interface IStorageSystem
    {
        public T Load<T>(string path, T @default = default(T));
        public void Save<T>(string path, T obj);
    }
}
