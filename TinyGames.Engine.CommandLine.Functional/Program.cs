using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace TinyGames.Engine.CommandLine.Functional
{
    internal class Iterator<T>
    {
        private T[] _array;
        private int _index = 0;

        public Iterator(IEnumerable<T> input)
        {
            _array = input.ToArray();
        }

        public bool HasCurrent => _index <= _array.Length - 1;

        public T Next()
        {
            _index++;

            return Current();
        }

        public T Current()
        {
            if (!HasCurrent) return default(T);

            return _array[_index];
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

            Param,

            BracketOpen,
            BracketClose,
            ArrayOpen,
            ArrayClose,

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
        private Iterator<char> _characters;

        public Lexer(string data)
        {
            _characters = new Iterator<char>(data.ToCharArray());
        }

        public IEnumerable<Token> Lex()
        {
            while (_characters.HasCurrent)
            {
                char c = _characters.Current();

                // strings
                if (c == '"')
                {
                    c = _characters.Next();

                    StringBuilder builder = new StringBuilder();

                    while (_characters.HasCurrent && c != '"')
                    {
                        builder.Append(c);

                        c = _characters.Next();
                    }

                    if (_characters.HasCurrent) _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.String,
                        Data = builder.ToString().Replace("\\n", "\n")
                    };
                }
                else if (c == '-')
                {
                    c = _characters.Next();

                    StringBuilder builder = new StringBuilder();

                    while (_characters.HasCurrent && IsCharacter(c))
                    {
                        builder.Append(c);

                        c = _characters.Next();
                    }

                    yield return new Token()
                    {
                        Type = Token.TokenType.Param,
                        Data = builder.ToString()
                    };
                }
                else if (c == '=')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.Equals,
                        Data = "="
                    };
                }
                else if (c == '(')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.BracketOpen,
                        Data = "("
                    };
                }
                else if (c == ')')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.BracketClose,
                        Data = ")"
                    };
                }
                else if (c == '[')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.ArrayOpen,
                        Data = "["
                    };
                }
                else if (c == ']')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.ArrayClose,
                        Data = "]"
                    };
                }
                else if (IsCharacter(c))
                {
                    StringBuilder builder = new StringBuilder();

                    while (_characters.HasCurrent && (IsCharacter(c) || IsNumber(c)))
                    {
                        builder.Append(c);

                        c = _characters.Next();
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

                    while (_characters.HasCurrent && (IsNumber(c) || c == '.'))
                    {
                        builder.Append(c);

                        c = _characters.Next();
                    }

                    yield return new Token()
                    {
                        Type = Token.TokenType.Number,
                        Data = builder.ToString()
                    };
                }
                else
                {
                    _characters.Next();
                }
            }

            yield return new Token()
            {
                Type = Token.TokenType.EOF,
                Data = ""
            };
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

        public Command()
        {
            Parameters = new Dictionary<string, string>();
        }

        public string GetString(string name)
        {
            return Parameters[name];
        }
        public int GetInt(string name)
        {
            return int.Parse(Parameters[name]);
        }
        public float GetFloat(string name)
        {
            return int.Parse(Parameters[name]);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Name);
            sb.Append(' ');
            foreach(var parameter in Parameters)
            {
                sb.Append(parameter.Key);
                sb.Append('=');
                sb.Append(parameter.Value);
                sb.Append(' ');
            }

            return sb.ToString();
        }
    }

    public class ParseResult<T> where T : class
    {
        public T Result { get;  }
        public string[] ErrorMessages { get; }
        public bool HasSucceeded => Result != null;

        public ParseResult(string[] messages)
        {
            ErrorMessages = messages;
        }

        public ParseResult(T result)
        {
            Result = result;
        }
    }

    public class ParserBase<T> where T: class
    {
        public ParseResult<T> Success(T data) => new ParseResult<T>(data);
        public ParseResult<T> Error(IEnumerable<string> messages) => new ParseResult<T>(messages.ToArray());
        public ParseResult<T> Error(params string[] messages) => new ParseResult<T>(messages);

        public ParseResult<T> UnexpectedToken(Token current) => Error($"Unexpected token: {current.Type} ({current.Data})");
        public ParseResult<T> UnexpectedToken(Token current, Token.TokenType expected) => Error($"Unexpected token: {current.Type} ({current.Data}). Expected {expected} instead.");
    }

    public class CommandParser : ParserBase<Command>
    {
        private Iterator<Token> _tokens;

        public CommandParser(IEnumerable<Token> tokens)
        {
            _tokens = new Iterator<Token>(tokens);
        }

        public ParseResult<Command> Parse()
        {
            var command = new Command();

            var commandName = _tokens.Current();

            if (commandName.Type != Token.TokenType.Identifier) return UnexpectedToken(commandName, Token.TokenType.Identifier);

            command.Name = commandName.Data;

            _tokens.Next();

            while (_tokens.HasCurrent && _tokens.Current().Type != Token.TokenType.EOF)
            {
                var parameterName = _tokens.Current();

                if (parameterName.Type != Token.TokenType.Identifier) return UnexpectedToken(parameterName, Token.TokenType.Identifier);

                var equals = _tokens.Next();

                if (equals.Type != Token.TokenType.Equals) return UnexpectedToken(equals, Token.TokenType.Equals);

                var data = _tokens.Next();

                command.Parameters.Add(parameterName.Data, data.Data);

                _tokens.Next();
            }

            return Success(command);
        }
    }

    public class SmolParser : ParserBase<Command>
    {
        private Iterator<Token> _tokens;

        public SmolParser(IEnumerable<Token> tokens)
        {
            _tokens = new Iterator<Token>(tokens);
        }

        public IEnumerable<SmolCommand> Parse()
        {
            while (_tokens.HasCurrent && _tokens.Current().Type != Token.TokenType.EOF)
            {
                yield return ParseCommand();
            }
        }

        public SmolCommand ParseCommand()
        {
            var current = _tokens.Current();

            if (current.Type == Token.TokenType.Identifier)
            {
                var param = _tokens.Next();

                var command = new FunctionSmolCommand();
                command.Name = current.Data;

                while(param != null && param.Type == Token.TokenType.Param)
                {
                    var name = param.Data;

                    _tokens.Next();

                    var value = ParseCommand();
                    command.Parameters.Add(name, value);

                    param = _tokens.Current();
                }

                return command;
            }
            else if (current.Type == Token.TokenType.Number)
            {
                _tokens.Next();
                return new ConstSmolCommand() { Value = new SmolNumber() { Data = double.Parse(current.Data) } };
            }
            else if (current.Type == Token.TokenType.String)
            {
                _tokens.Next();
                return new ConstSmolCommand() { Value = new SmolString() { Data = current.Data } };
            }
            else
            {
                _tokens.Next();
                return null;
            }
        }
    }

    public class SmolValue
    {
        public virtual bool IsString => false;
        public virtual bool IsNumber => false;
        public virtual bool IsObject => false;
        public virtual bool IsArray => false;

        public string AsString()
        {
            if (this is SmolString str) return str.Data;

            throw new ArgumentException("Not a string");
        }
        public double AsNumber()
        {
            if (this is SmolNumber num) return num.Data;

            throw new ArgumentException("Not a number");
        }
        public SmolValue[] AsArray()
        {
            if (this is SmolArray array) return array.Data;

            throw new ArgumentException("Not an array");
        }
    }
    public class SmolBase<T> : SmolValue
    {
        public T Data;
    }

    public class SmolObject : SmolBase<object>
    {
        public override bool IsObject => true;
        public override string ToString() => $"{Data}";
    }
    public class SmolNumber : SmolBase<double>
    {
        public override bool IsNumber => true;
        public override string ToString() => $"{Data}";
    }
    public class SmolString : SmolBase<string>
    {
        public override bool IsString => true;

        public override string ToString() => $"{Data}";
    }
    public class SmolArray : SmolBase<SmolValue[]>
    {
        public override bool IsArray => true;
        public override string ToString() => $"{ string.Join(',', Data.Select(x => x.ToString())) }";
    }

    public abstract class SmolCommand
    {
        public abstract void Execute(SmolRuntime runtime);
    }

    public class ConstSmolCommand : SmolCommand
    {
        public SmolValue Value { get; set; }

        public override void Execute(SmolRuntime runtime)
        {
            runtime.PushValue(Value);
        }
    }

    public class FunctionSmolCommand : SmolCommand
    {
        public string Name { get; set; }
        public Dictionary<string, SmolCommand> Parameters { get; set; }

        public FunctionSmolCommand()
        {
            Parameters = new Dictionary<string, SmolCommand>();
        }

        public string GetString(SmolRuntime runtime, string name)
        {
            var value = GetValue(runtime, name);

            if (value is SmolString) return (value as SmolString).Data;

            throw new ApplicationException($"Cannot get value {name} as string.");
        }

        public double GetNumber(SmolRuntime runtime, string name)
        {
            var value = GetValue(runtime, name);

            if (value is SmolNumber) return (value as SmolNumber).Data;

            throw new ApplicationException($"Cannot get value {name} as number.");
        }

        public SmolValue GetValue(SmolRuntime existingRuntime, string name)
        {
            if(!Parameters.TryGetValue(name, out var command)){
                throw new ApplicationException($"Cannot find parameter with name: {name}");
            }

            // Create a seperate runtime for this
            SmolRuntime runtime = new SmolRuntime(existingRuntime);

            command.Execute(runtime);

            return runtime.PopValue();
        }

        public override void Execute(SmolRuntime runtime)
        {
            var processor = runtime.FindProcessor(Name);

            if(processor == null)
            {
                throw new Exception($"Unknown function with name \"{Name}\"!");
            }

            processor.Invoke(this, runtime);
        }
    }
    
    public class SmolRuntime
    {
        public delegate void SmolProcessor(FunctionSmolCommand func, SmolRuntime runtime);

        private SmolRuntime _parentRuntime;
        private Stack<SmolValue> _stack;

        private Dictionary<string, SmolProcessor> _actions;
        
        public SmolRuntime(SmolRuntime parentRuntime = null)
        {
            _parentRuntime = parentRuntime;

            _stack = new Stack<SmolValue>();
            _actions = new Dictionary<string, SmolProcessor>();
        }

        public void RegisterCommand(string command, SmolProcessor action)
        {
            _actions.Add(command, action);
        }

        public SmolProcessor FindProcessor(string command)
        {
            if (_actions.ContainsKey(command)) return _actions[command];

            return _parentRuntime?.FindProcessor(command);
        }

        public SmolValue PopValue()
        {
            return _stack.Pop();
        }

        public void PushValue(string value)
        {
            _stack.Push(new SmolString() { Data = value });
        }
        public void PushValue(double value)
        {
            _stack.Push(new SmolNumber() { Data = value });
        }
        public void PushValue(SmolValue value)
        {
            _stack.Push(value);
        }
        public void PushValue(SmolValue[] value)
        {
            _stack.Push(new SmolArray() { Data = value });
        }

        public SmolValue Pop()
        {
            return _stack.Pop();
        }

        public void Execute(SmolCommand command)
        {
            command.Execute(this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach(var s in _stack)
            {
                builder.Append(s);
                builder.Append(' ');
            }

            return builder.ToString();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("$ ");
                Console.ForegroundColor = ConsoleColor.Blue;

                string line = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.White;

                Lexer lexer = new Lexer(line);
                SmolParser parser = new SmolParser(lexer.Lex());
                SmolRuntime runtime = new SmolRuntime();

                runtime.RegisterCommand("print", (func, runtime) => {
                    var value = runtime.PopValue();

                    Console.WriteLine(value);
                });
                runtime.RegisterCommand("split", (func, runtime) => {
                    var value = runtime.Pop().AsString();
                    var on = func.GetString(runtime, "on");

                    var splitted = value.Split(on);

                    runtime.PushValue(splitted.Select(x => new SmolString() { Data = x }).ToArray());
                });
                runtime.RegisterCommand("workingdir", (func, runtime) => {
                    runtime.PushValue(Directory.GetCurrentDirectory());
                });
                runtime.RegisterCommand("setworkingdir", (func, runtime) => {
                    var to = runtime.Pop().AsString();

                    Directory.SetCurrentDirectory(to);
                });
                runtime.RegisterCommand("dup", (func, runtime) => {
                    var value = runtime.PopValue();
                });
                runtime.RegisterCommand("clear", (func, runtime) => {
                    Console.Clear();
                });
                runtime.RegisterCommand("append", (func, runtime) => {
                    var to = runtime.Pop().AsString();
                    var from = runtime.Pop().AsString();

                    runtime.PushValue(from + to);
                });
                runtime.RegisterCommand("join", (func, runtime) => {
                    var array = runtime.Pop().AsArray();

                    var on = func.GetString(runtime, "with");

                    runtime.PushValue(string.Join(on, array.Select(x => x.ToString())));
                });
                runtime.RegisterCommand("readfile", (func, runtime) => {
                    var value = runtime.Pop().AsString();

                    runtime.PushValue(File.ReadAllText(value));
                });
                runtime.RegisterCommand("listfiles", (func, runtime) => {
                    var value = runtime.Pop().AsString();

                    var files = Directory.GetFiles(value);

                    runtime.PushValue(files.Select(x => new SmolString() { Data = x }).ToArray());
                });
                runtime.RegisterCommand("help", (func, runtime) => {
                    Console.WriteLine("Help will be provided later :)");
                });
                runtime.RegisterCommand("execute", (func, runtime) => {
                    Console.WriteLine("Oh boy this will be very very nice but holy shit unsafe. You sure?");
                });
                runtime.RegisterCommand("dump", (func, runtime) => {
                    Console.WriteLine("Stack dump: ");
                });
                runtime.RegisterCommand("exit", (func, runtime) => {
                    Environment.Exit(0);
                });

                try
                {
                    foreach(var command in parser.Parse())
                    {
                        if(command == null)
                        {
                            Console.WriteLine("null command");
                            continue;
                        }

                        runtime.Execute(command);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }
    }
}
