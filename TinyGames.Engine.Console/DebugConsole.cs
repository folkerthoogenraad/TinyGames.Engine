using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TinyGames.Engine.Console
{
    public class DebugConsoleParser
    {
        public static IEnumerable<string> Parse(string input)
        {
            return Regex.Matches(input, @"[\""].+?[\""]|[^ ]+").Select(m => m.Value);
        }
    }

    public class DebugConsoleCommandAttribute : Attribute
    {
        public string Name;

        public DebugConsoleCommandAttribute(string name) { Name = name; }
    }

    public class DebugConsoleCommand
    {
        public string Name { get; }
        public IEnumerable<DebugConsoleCommandParameter> Parameters => _parameters;

        private DebugConsoleCommandParameter[] _parameters;

        public DebugConsoleCommand(string name, IEnumerable<DebugConsoleCommandParameter> parameters)
        {
            Name = name;
            _parameters = parameters.ToArray();
        }

        public void Invoke(object[] arguments)
        {
            if (_parameters.Length != arguments.Length) throw new Exception();
        }
    }
    public class DebugConsoleCommandParameter
    {
        public string Name;
        public Type Type;
    }

    public class DebugConsole
    {
        public IEnumerable<DebugConsoleCommand> Commands => _commands;
        private List<DebugConsoleCommand> _commands;

        public DebugConsole(IEnumerable<DebugConsoleCommand> commands)
        {
            _commands = commands.ToList();
        }

        public DebugConsoleCommand FindCommand(string input)
        {
            var cmd = DebugConsoleParser.Parse(input).FirstOrDefault();

            if (cmd == null) return null;

            return Commands.Where(x => x.Name == cmd).FirstOrDefault();
        }

        public static IEnumerable<DebugConsoleCommand> FromType(Type type)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttributes(typeof(DebugConsoleCommandAttribute), false).FirstOrDefault() as DebugConsoleCommandAttribute;

                if (attribute == null) continue;

                yield return new DebugConsoleCommand(attribute.Name, 
                    method.GetParameters().Select(x => new DebugConsoleCommandParameter() { Name = x.Name, Type = x.ParameterType })
                    );
            }
        }
    }
}
