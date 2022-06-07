namespace KConsoleDebugger
{
    public interface ILogger
    {
        void Info(object message);

        void Warn(object message);
    }

    public interface ILogger<T> : ILogger
    {

    }
}