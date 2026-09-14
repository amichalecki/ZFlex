using ZFlex.Enums;

namespace ZFlex.Abstracts
{
    public interface ILogger
    {
        void Log(LogLevel level, string message, params object[] args);

        void LogDebug(string message, params object[] args);
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(string message, params object[] args);
    }
}
