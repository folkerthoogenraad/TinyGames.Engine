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
        public bool IsStringValue() => Type == "string";
        public bool IsValueFloat() => Type == "float";

        public bool GetValueAsBool()
        {
            if (!IsBoolValue()) throw new Exception("Value is not a boolean.");

            var element = (JsonElement)Value;

            return element.GetBoolean();
        }
        public int GetValueAsInt()
        {
            if (!IsIntValue()) throw new Exception("Value is not a int.");

            var element = (JsonElement)Value;

            return element.GetInt32();
        }
        public string GetValueAsString()
        {
            if (!IsStringValue()) throw new Exception("Value is not a string.");

            var element = (JsonElement)Value;

            return element.GetString();
        }
        public float GetValueAsFloat()
        {
            if (!IsValueFloat()) throw new Exception("Value is not a float.");

            var element = (JsonElement)Value;

            return (float)element.GetDouble();
        }
    }
}
