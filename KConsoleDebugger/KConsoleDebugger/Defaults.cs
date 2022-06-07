using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KConsoleDebugger
{
    public static class Defaults
    {
        private static string _path;
        private static Log _log;

        public static string Path => _path;

        public static Log Log
        {
            get
            {
                return _log;
            }
            set
            {
                if (value is not null) _log = value;
            }
        }
        public static ILogger Logger = new ConsoleLogger(Log);

        static Defaults()
        {
            _path = $"{DateTime.Now:d}__{DateTime.Now:t}".Replace('.', '_').Replace(':', '_');
            _log = new Log(Path);
        }
    }
}
