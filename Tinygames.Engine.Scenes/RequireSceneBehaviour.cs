using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Scenes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class RequireSceneBehaviour : Attribute
    {
        public Type Type { get; }

        public RequireSceneBehaviour(Type type)
        {
            Type = type;
        }
    }
}
