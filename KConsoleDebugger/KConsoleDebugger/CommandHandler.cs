using System.Collections;

namespace KConsoleDebugger
{
    public interface ICommandHandler
    {
        public bool IsNotCommandWarns { get; set; }
        public bool TooManyArgsWarns { get; set; }

        /// <summary>
        /// Gets user's input in string format, then splits it into args and calls certain command.
        /// </summary>
        /// <param name="input">
        /// User's input with args (if needed for command)
        /// </param>
        /// <param name="separatorReplacer">
        /// If Separator, for example, is space, then separatorReplacer '|' can be used to used space as non-separator in argument;
        /// Example: '/log This|sentance|has|spaces|in|it';
        /// Output will be 'This sentance has spaces in it'.
        /// </param>
        public void HandleInput(string input, char separatorReplacer);
    }

    public class CommandHandler : ICommandHandler
    {
        private readonly ILogger _logger;
        private readonly char _prefix;
        private readonly char _separator;
        private readonly CommandList _commands;

        public bool IsNotCommandWarns { get; set; } = true;
        public bool TooManyArgsWarns { get; set; } = true;

        public static Dictionary<string, Func<string, object>> Converters = new()
        {
            { typeof(int).ToString(),    (val) => Convert.ToInt32(val) },
            { typeof(string).ToString(), (val) => val },
            { typeof(float).ToString(),  (val) => Convert.ToSingle(val) },
            { typeof(bool).ToString(),   (val) => Convert.ToBoolean(val) },
        };

        public CommandHandler(ILogger logger, char firstSymbol, char separator, CommandList commands!!)
        {
            _logger = logger;
            _prefix = firstSymbol;
            _separator = separator;
            _commands = commands;
        }

        public void HandleInput(string input, char separatorReplacer = '|')
        {
            if (!input.StartsWith(_prefix))
            {
                if (IsNotCommandWarns)
                    _logger.Warn("This is not a command.\n");
                return;
            }

            string[] formattedInput = input.TrimStart(_prefix).Split(_separator);
            for (int i = 0; i < formattedInput.Length; i++)
            {
                formattedInput[i] = formattedInput[i].Replace(separatorReplacer, _separator);
            }
            string cmdName = formattedInput[0];
            string[] textArgs = formattedInput.Skip(1).ToArray();
            ArrayList finalArgs = new ArrayList();
            ICommand command = null;
            try
            {
                command = _commands[cmdName];
            }
            catch
            {
                _logger.Warn($"There is no command with name \"{cmdName}\"!\n");
                return;
            }
            Type tp = command.GetType();
            Type[] argsTypes = tp.GetGenericArguments();

            if (command is Command commonCommand)
            {
                commonCommand.Invoke();
                return;
            }

            if (textArgs.Length < argsTypes.Length)
            {
                _logger.Warn($"Not enough args! Needed: {argsTypes.Length}\n");
                return;
            }
            else if (textArgs.Length > argsTypes.Length && TooManyArgsWarns)
                _logger.Warn("Too many args. Is this alright?");

            // _logger.Info("");
            for (int i = 0; i < argsTypes.Length; i++)
            {
                string arg = textArgs[i];
                Type type = argsTypes[i];
                object? instance = new();

                try
                {
                    if (type.FullName != "System.Object")
                        instance = Converters[type.FullName](arg);
                }
                catch { _logger.Warn($"Incorrect type! Arg #{i} must be {type.Name}\n"); return; }

                finalArgs.Add(instance);
            }

            if (command is IParametrizedCommand c)
                c.Invoke(finalArgs);

            Console.Write($"\n");
        }
    }
}
