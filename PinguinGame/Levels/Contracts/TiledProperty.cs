using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PinguinGame.Levels.Contracts
{
    public class TiledProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public object Value{ get; set; }

        public bool IsBoolValue() => Type == "bool";
        public bool IsIntValue() => Type == "int";

        public bool GetValueAsBool()
        {
            if (!IsBoolValue()) throw new Exception("Value is not a boolean.");

            var element = (JsonElement)Value;

            return element.GetBoolean();
        }
        public int GetValueAsInt()
        {
            if (!IsIntValue()) throw new Exception("Value is not a boolean.");

            var element = (JsonElement)Value;

            return element.GetInt32();
        }
    }
}
