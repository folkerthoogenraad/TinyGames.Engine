using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TinyGames.Engine.Util
{
    public static class LinqExtensions
    {
        public static IEnumerable<(T, T)> Loop<T>(this IEnumerable<T> input)
        {
            if (input.Count() < 2) yield break;

            var first = input.First();
            var last = first;

            foreach (var t in input.Skip(1))
            {
                yield return (last, t);
                last = t;
            }

            yield return (last, first);
        }

        public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> input)
        {
            if (input.Count() < 2) yield break;

            var first = input.First();
            var last = first;

            foreach (var t in input.Skip(1))
            {
                yield return (last, t);
                last = t;
            }
        }

    }
}
