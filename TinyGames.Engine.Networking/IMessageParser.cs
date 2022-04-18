using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TinyGames.Engine.Networking
{
    public interface IMessageParser<T>
    {
        IEnumerable<T> ParseStream(Stream stream);
    }

    public class ASCIILineParser : IMessageParser<string>
    {
        public IEnumerable<string> ParseStream(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                while (true)
                {
                    string line = reader.ReadLine();

                    if (line == null) yield break;

                    yield return line;
                }
            }
        }
    }
}
