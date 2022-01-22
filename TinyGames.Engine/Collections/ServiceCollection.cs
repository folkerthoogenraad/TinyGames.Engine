using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace TinyGames.Engine.Collections
{
    public class ServiceCollection : IServiceCollection
    {
        public Dictionary<Type, object> _instances;

        public ServiceCollection()
        {
            _instances = new Dictionary<Type, object>();
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }
        public object CreateInstance(Type type)
        {
            ConstructorInfo suitableConstructor = 
                type.GetConstructors()
                .Where(constructor => constructor.IsPublic)
                .Where(constructor => constructor.GetParameters().All(param => IsServiceRegistered(param.ParameterType)))
                .FirstOrDefault();

            if (suitableConstructor == null) throw new ArgumentException("No suitable constructor found with registered services");

            object[] arguments = suitableConstructor.GetParameters().Select(param => GetServiceRaw(param.ParameterType)).ToArray();

            return suitableConstructor.Invoke(arguments);
        }

        
        public T GetService<T>()
        {
            if (!IsServiceRegistered<T>()) throw new ArgumentException($"Type of service ({typeof(T)}) is not found.");

            T value = (T)GetServiceRaw(typeof(T));

            return value;
        }

        public T RegisterService<T>()
        {
            if (IsServiceRegistered<T>()) throw new ArgumentException($"Type of service ({typeof(T)}) is already registered.");

            T value = CreateInstance<T>();

            return RegisterService<T>(value);
        }

        public T RegisterService<T>(T value)
        {
            if (IsServiceRegistered<T>()) throw new ArgumentException($"Type of service ({typeof(T)}) is already registered.");

            StoreServiceRaw(value);

            return value;
        }

        private bool IsServiceRegistered<T>()
        {
            return IsServiceRegistered(typeof(T));
        }
        private bool IsServiceRegistered(Type t)
        {
            return _instances.ContainsKey(t);
        }

        private object GetServiceRaw(Type t)
        {
            object value;
            
            if(_instances.TryGetValue(t, out value))
            {
                return value;
            }

            return null;
        }
        private void StoreServiceRaw<T>(T t)
        {
            _instances[typeof(T)] = t;
        }

    }
}
