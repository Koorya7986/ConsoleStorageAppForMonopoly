namespace ConsoleStorageApp.Common;

public interface ILogger
{
    void Log(string message, LogType type);
}
