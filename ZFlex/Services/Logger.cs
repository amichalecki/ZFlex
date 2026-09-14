using ZFlex.Abstracts;
using ZFlex.Enums;

namespace ZFlex.Services
{

    public class Logger : ILogger
    {
        private static Logger? _instance = null;
        private string? _filePath;
        private string _dateFormat = "yyyy-MM-dd HH:mm:ss.fff";

        private Logger(string? filePath) 
        { 
            if (File.Exists(filePath))
                _filePath = filePath;
        }

        public static ILogger Create(string? filePath = null) => _instance = new(filePath);

        public static ILogger Get() => _instance ??= new(null);

        public void Log(LogLevel level, string message, params object[] args)
        {
            var exportMessage = $"[{DateTime.Now.ToString(_dateFormat)} - {Level(level)}]: {string.Format(message, args)}";
            if (_filePath != null)
                File.AppendAllText(_filePath, $"{exportMessage}\n");

            Console.WriteLine(exportMessage);

        }

        public void LogDebug(string message, params object[] args) => Log(LogLevel.Debug, message, args);
        public void LogInformation(string message, params object[] args) => Log(LogLevel.Info, message, args);
        public void LogWarning(string message, params object[] args) => Log(LogLevel.Warning, message, args);
        public void LogError(string message, params object[] args) => Log(LogLevel.Error, message, args);

        private string Level(LogLevel level)
            => level switch
            {
                LogLevel.Info => "INF",
                LogLevel.Warning => "WRN",
                LogLevel.Error => "ERR",
                _ => "DBG"
            };
    }
}
