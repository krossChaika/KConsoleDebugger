namespace KConsoleDebugger
{
    public class ConsoleLogger : ILogger
    {
        private Log _log;

        public ConsoleLogger(Log log)
        {
            _log = log;
        }

        public void Info(object message)
        {
            int lineNumber = _log.Content.Count;
            string time = $"{DateTime.Now:T}";
            string source = "INFO";

            string toLog = $"[{lineNumber}][{time}][{source}] {message}";

            Console.WriteLine(toLog);
            _log.Content.Add(toLog);
        }

        public void Warn(object message)
        {
            int lineNumber = _log.Content.Count;
            string time = $"{DateTime.Now:T}";
            string source = "WARN";

            string toLog = $"[{lineNumber}][{time}][{source}] {message}";

            Console.WriteLine(toLog);
            _log.Content.Add(toLog);
        }
    }

    public class ConsoleLogger<T> : ILogger<T>
    {
        protected readonly Log _log;

        public ConsoleLogger(Log log)
        {
            _log = log;
            //Info($"Log created at: {DateTime.Now:G}");
            //Print("");
        }

        public void Print(object message)
        {
            Console.WriteLine(message);
            _log.Content.Add((string)message);
        }

        public void Info(object message)
        {
            int lineNumber = _log.Content.Count;
            string time = $"{DateTime.Now:T}";
            string source = $"{typeof(T).Name}/INFO";

            string toLog = $"[{lineNumber}][{time}][{source}] {message}";

            Console.WriteLine(toLog);
            _log.Content.Add(toLog);
        }

        public void Warn(object message)
        {
            int lineNumber = _log.Content.Count;
            string time = $"{DateTime.Now:T}";
            string source = $"{typeof(T).Name}/WARN";

            string toLog = $"[{lineNumber}][{time}][{source}] {message}";

            Console.WriteLine(toLog);
            _log.Content.Add(toLog);
        }
    }
}