using System.Reflection;
using System.Text;

namespace KConsoleDebugger
{
    public class CommandList
    {
        private List<ICommand> _data;

        public static readonly CommandList Singleton = new CommandList();

        public CommandList()
        {
            Action<int> help = (page) =>
            {
                int pageLength = 5;
                int startIndex = (page - 1) * pageLength;
                int lastIndex = page * pageLength;
                string s = $"Commands page #{page} [{startIndex + 1} - {lastIndex}]:\n\n";
                Type?[] argsTypes = null;

                for (int i = startIndex; i < lastIndex; i++)
                {
                    ICommand c;
                    try
                    {
                        c = _data[i];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }

                    Type tp = c.GetType();

                    //argsTypes = tp.GetGenericArguments();

                    s += $"  {i + 1}. {c.Name}";
                    if (c.ArgsInfo is not null)
                    {
                        string types = string.Empty;
                        foreach (KeyValuePair<string, string> pair in c.ArgsInfo)
                        {
                            types += $"{pair.Value} {pair.Key}, ";
                            //types += "{=DarkBlue}" + pair.Value + " {=DarkCyan}" + pair.Key + "{/}, ";
                        }
                        types = types.Substring(0, types.Length - 2); // Delete last ", "
                        s += $"({types})";
                    }

                    s += "\n";

                    // Add desc.
                    if (!string.IsNullOrEmpty(c.Description))
                        s += "    " + c.Description + "\n";
                    s += "\n";
                }

                Console.Write(s);
                //Program.Print(s);
            };
            Action<string> revStr = (a) =>
            {
                string s = string.Empty;
                for (int i = 0; i < a.Length; i++)
                {
                    s += a[a.Length - 1 - i];
                }
                Defaults.Logger.Info(s);
            };
            Action<string, string> ass = (a1, a2) => // thicc ass delegate
            {
                FieldInfo info = Singleton[a1].GetType().GetField("_execute", BindingFlags.NonPublic | BindingFlags.Instance);
                info.SetValue(Singleton[a1], info.GetValue(Singleton[a2]));
            };

            _data = new List<ICommand>()
            {
                new Command<int>("help", help) { Description = "Writes names of all commands and their descriptions." },
                new Command("clear", () => Console.Clear()) { Description = "Clears console's output." },
                new Command<string>("log", (a1) => Defaults.Logger.Info(a1)) { Description = "Writes arg 'a1'." },
                new Command<string, string>("associate", ass) { Description = "Replaces func of cmd 'arg1' with func of cmd 'arg2'." },

                #region Basic cmds for testing
                //new Command("stop", () => Program.ProgramRunning = false)  { Description = "Ends main cycle." },

                //new Command<int>("revInt", (a1) => Defaults.Logger.Info(-a1)),
                //new Command<string>("revStr", revStr),
                //new Command<bool>("revBool", (a1) => Defaults.Logger.Info(!a1)),

                //new Command<int, int>("sum", (a1, a2) => Defaults.Logger.Info(a1 + a2)),
                //new Command<int, int>("minus", (a1, a2) => Defaults.Logger.Info(a1 - a2)),
                //new Command<int, int>("mult", (a1, a2) => Defaults.Logger.Info(a1 * a2)),
                //new Command<int, int>("divide", (a1, a2) => Defaults.Logger.Info(a1 / a2)),
                //// 'Float'-s
                //new Command<float, float>("sumF", (a1, a2) => Defaults.Logger.Info(a1 + a2)),
                //new Command<float, float>("minusF", (a1, a2) => Defaults.Logger.Info(a1 - a2)),
                //new Command<float, float>("multF", (a1, a2) => Defaults.Logger.Info(a1 * a2)),
                //new Command<float, float>("divideF", (a1, a2) => Defaults.Logger.Info(a1 / a2)),

                //new Command<string, string>("isEqualS", (a1, a2) => Defaults.Logger.Info(a1 == a2)),
                //new Command<int, int>("isEqualI", (a1, a2) => Defaults.Logger.Info(a1 == a2)),
                //new Command<bool, bool>("isEqualB", (a1, a2) => Defaults.Logger.Info(a1 == a2)),
                #endregion
            };
        }

        public ICommand this[string name]
        {
            get
            {
                foreach (ICommand c in _data)
                {
                    if (c.Name == name)
                        return c;
                }
                throw new Exception($"There is no command with name \"{name}\"");
            }
        }

        public void Add(ICommand cmd) => _data.Add(cmd);
    }
}
