using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Microsoft.Xna.Framework;

namespace TinyGames.Engine.Collections
{
    public static class GameServiceContainerExtensions
    {
        public static T AddAndGetService<T>(this GameServiceContainer self)
        {
            var instance = self.CreateInstance<T>();

            return self.AddAndGetService<T>(instance);
        }

        public static T AddAndGetService<T>(this GameServiceContainer self, object value)
        {
            return (T) self.AddAndGetService(typeof(T), value);
        }

        public static object AddAndGetService(this GameServiceContainer self, Type t, object obj)
        {
            self.AddService(t, obj);

            return obj;
        }


        public static void AddService<T>(this GameServiceContainer self)
        {
            self.AddService(typeof(T), self.CreateInstance<T>());
        }
        public static void AddService<Tregister, Tinstance>(this GameServiceContainer self)
        {
            self.AddService(typeof(Tregister), self.CreateInstance<Tinstance>());
        }
        public static Tregister AddAndGetService<Tregister, Tinstance>(this GameServiceContainer self)
        {
            self.AddService<Tregister, Tinstance>();

            return (Tregister) self.GetService(typeof(Tregister));
        }
    }
}
