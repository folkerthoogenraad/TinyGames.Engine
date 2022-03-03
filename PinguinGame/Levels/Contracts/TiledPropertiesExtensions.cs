using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.Levels.Contracts
{
    public static class TiledPropertiesExtensions
    {
        public static bool GetBoolProperty(this TiledProperty[] self, string name, bool def = false)
        {
            if (self != null)
            {
                return self.Where(x => x.Name == name).Any(x => x.GetValueAsBool());
            }
            return def;
        }
        public static int GetIntProperty(this TiledProperty[] self, string name, int def = 0)
        {
            if (self != null)
            {
                return self.Where(x => x.Name == name).Select(x => x.GetValueAsInt()).FirstOrDefault();
            }
            return def;
        }
        public static string GetStringProperty(this TiledProperty[] self, string name, string def = null)
        {
            if (self != null)
            {
                return self.Where(x => x.Name == name).Select(x => x.GetValueAsString()).FirstOrDefault();
            }

            return def;
        }
        public static float GetFloatProperty(this TiledProperty[] self, string name, float def = 0)
        {
            if (self != null)
            {
                return self.Where(x => x.Name == name).Select(x => x.GetValueAsFloat()).FirstOrDefault();
            }

            return def;
        }
    }
}
