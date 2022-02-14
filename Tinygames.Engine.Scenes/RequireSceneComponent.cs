using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Scenes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class RequireSceneComponent : Attribute
    {
        public Type Type { get; }

        public RequireSceneComponent(Type type)
        {
            Type = type;
        }
    }
}
