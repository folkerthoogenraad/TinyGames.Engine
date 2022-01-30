using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace TinyGames.Engine.Collections
{
    public static class ServiceProviderExtensions
    {
        public static T CreateInstance<T>(this IServiceProvider self)
        {
            return (T)self.CreateInstance(typeof(T));
        }

        public static object CreateInstance(this IServiceProvider self, Type type)
        {
            ConstructorInfo suitableConstructor = 
                type.GetConstructors()
                .Where(constructor => constructor.IsPublic)
                .Where(constructor => constructor.GetParameters().All(param => self.GetService(param.ParameterType) != null))
                .FirstOrDefault();

            if (suitableConstructor == null) throw new ArgumentException("No suitable constructor found with registered services");

            object[] arguments = suitableConstructor.GetParameters().Select(param => self.GetService(param.ParameterType)).ToArray();

            return suitableConstructor.Invoke(arguments);
        }

        public static T GetService<T>(this IServiceProvider self)
        {
            T value = (T)self.GetService(typeof(T));

            return value;
        }
    }
}
