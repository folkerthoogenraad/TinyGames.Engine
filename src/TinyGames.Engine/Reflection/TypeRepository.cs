using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;

namespace TinyGames.Engine.Reflection
{
    public class TypeRepository<T>
    {
        private Type[] _types;

        public TypeRepository(Assembly assembly)
        {
            _types = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(T))).ToArray();
        }

        public IEnumerable<(Type Type, Match Match)> Search(string text)
        {
            Regex rx = new Regex(text, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return _types.Select(x => (Type: x, Match: rx.Match(x.FullName))).Where(x => x.Match.Success);
        }
    }
}
