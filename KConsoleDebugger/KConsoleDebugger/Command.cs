using System.Collections;

namespace KConsoleDebugger
{
    public class Command : ICommand
    {
        private Action _execute;
        public string Name { get; init; }
        public string? Description { get; init; }
        public Dictionary<string, string> ArgsInfo { get; init; }

        public Command(string name, Action execution!!)
        {
            Name = name;
            _execute = execution;
        }

        public void Invoke()
        {
            _execute();
        }
    }

    public class Command<T1> : IParametrizedCommand
    {
        private Action<T1> _execute;
        public string Name { get; init; }
        public string? Description { get; init; }
        public Dictionary<string, string> ArgsInfo { get; init; } = new();

        public Command(string name, Action<T1> execution!!)
        {
            Name = name;
            _execute = execution;

            var j = _execute.GetType().GetMethod("Invoke").GetParameters();
            foreach (var f in j)
            {
                ArgsInfo.Add(f.Name, f.ParameterType.Name);
            }
        }

        public void Invoke(ArrayList args!!)
        {
            _execute((T1)args[0]);
        }
    }

    public class Command<T1, T2> : IParametrizedCommand
    {
        private Action<T1, T2> _execute;
        public string Name { get; init; }
        public string? Description { get; init; }
        public Dictionary<string, string> ArgsInfo { get; init; } = new();

        public Command(string name, Action<T1, T2> execution!!)
        {
            Name = name;
            _execute = execution;

            var j = _execute.GetType().GetMethod("Invoke").GetParameters();
            foreach (var f in j)
            {
                ArgsInfo.Add(f.Name, f.ParameterType.Name);
            }
        }

        public void Invoke(ArrayList args!!)
        {
            _execute((T1)args[0], (T2)args[1]);
        }
    }

    public class Command<T1, T2, T3> : IParametrizedCommand
    {
        private Action<T1, T2, T3> _execute;
        public string Name { get; init; }
        public string? Description { get; init; }
        public Dictionary<string, string> ArgsInfo { get; init; } = new();

        public Command(string name, Action<T1, T2, T3> execution!!)
        {
            Name = name;
            _execute = execution;

            var j = _execute.GetType().GetMethod("Invoke").GetParameters();
            foreach (var f in j)
            {
                ArgsInfo.Add(f.Name, f.ParameterType.Name);
            }
        }

        public void Invoke(ArrayList args!!)
        {
            _execute((T1)args[0], (T2)args[1], (T3)args[2]);
        }
    }

    public interface ICommand
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public Dictionary<string, string> ArgsInfo { get; init; }
    }

    public interface IParametrizedCommand : ICommand
    {
        public void Invoke(ArrayList args);
    }
}
