using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.CommandLine.Functional
{
    internal class Iterator<T>
    {
        private IEnumerator<T> _iterator;

        public Iterator(IEnumerable<T> input)
        {
            _iterator = input.GetEnumerator();
        }

        public bool HasCurrent => _index <= _chars.Length - 1;

        public char Next()
        {
            _index++;

            return Current();
        }

        public char Current()
        {
            if (!HasCurrent) return '\0';

            return _chars[_index];
        }
    }

    public class Token
    {
        public enum TokenType
        {
            Identifier,
            Equals,
            Number,
            String,
            EOF
        }

        public TokenType Type { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: ({1})", Type, Data);
        }
    }

    public class Lexer
    {
        private char[] _chars;
        private int _index = 0;

        public Lexer(string data)
        {
            _chars = data.ToCharArray();
            _index = 0;
        }

        public IEnumerable<Token> Lex()
        {
            while (HasCurrent)
            {
                char c = Current();

                // strings
                if(c == '"')
                {
                    c = Next();

                    StringBuilder builder = new StringBuilder();

                    while(HasCurrent && c != '"')
                    {
                        builder.Append(c);

                        c = Next();
                    }

                    if (HasCurrent) Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.String,
                        Data = builder.ToString()
                    };
                }
                else if (c == '=')
                {
                    Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.Equals,
                        Data = "c"
                    };
                }
                else if (IsCharacter(c))
                {
                    StringBuilder builder = new StringBuilder();

                    while (HasCurrent && (IsCharacter(c) || IsNumber(c)))
                    {
                        builder.Append(c);

                        c = Next();
                    }

                    yield return new Token()
                    {
                        Type = Token.TokenType.Identifier,
                        Data = builder.ToString()
                    };
                }
                else if (IsNumber(c))
                {
                    StringBuilder builder = new StringBuilder();

                    while (HasCurrent && (IsNumber(c) || c == '.'))
                    {
                        builder.Append(c);

                        c = Next();
                    }

                    yield return new Token()
                    {
                        Type = Token.TokenType.Number,
                        Data = builder.ToString()
                    };
                }
                else
                {
                    Next();
                }
            }

            yield return new Token()
            {
                Type = Token.TokenType.EOF,
                Data = ""
            };
        }

        public bool HasCurrent => _index <= _chars.Length - 1;

        public char Next()
        {
            _index++;

            return Current();
        }

        public char Current()
        {
            if (!HasCurrent) return '\0';

            return _chars[_index];
        }

        public bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\r' || c == '\n';
        }
        public bool IsCharacter(char c)
        {
            return (c >= 'a' && c <= 'z')
                || (c >= 'A' && c <= 'Z');
        }
        public bool IsNumber(char c)
        {
            return (c >= '0' && c <= '9');
        }
    }

    public class Command
    {
        public string Name { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }

    public class Parser
    {
        private IEnumerable<Token> _tokens;

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens;
        }

        public Command Parse()
        {
            var command = new Command();



            return command;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                string line = Console.ReadLine();

                Lexer lexer = new Lexer(line);

                foreach(var token in lexer.Lex())
                {
                    Console.WriteLine(token);
                }
            }
        }
    }
}
