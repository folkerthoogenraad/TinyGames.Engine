using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collections
{
    public interface IServiceCollection
    {
        public T RegisterService<T>();
        public T RegisterService<T>(T value);
        public T GetService<T>();
        public T CreateInstance<T>();
    }
}
