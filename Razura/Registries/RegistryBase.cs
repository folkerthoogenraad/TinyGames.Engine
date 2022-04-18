using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Razura.Registries
{
    public abstract class RegistryBase<T>
    {
        private Dictionary<string, T> _registry;

        public RegistryBase()
        {
            _registry = new Dictionary<string, T>();
        }

        public void Register(string id, T value)
        {
            _registry.Add(id, value);
        }

        public bool UnRegister(string id)
        {
            // TODO checks if equal?
            return _registry.Remove(id, out T _);
        }

        public T Get(string key)
        {
            return _registry[key];
        }
    }
}
