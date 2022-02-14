using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Scenes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class RequireComponent : Attribute
    {
        public Type Type { get; }

        public RequireComponent(Type type)
        {
            Type = type;
        }
    }
}
