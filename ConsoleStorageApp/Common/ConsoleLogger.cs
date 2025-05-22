namespace ConsoleStorageApp.Common;

internal class ConsoleLogger : ILogger
{
    public void Log(string message, LogType type)
    {
        switch (type)
        {
            case LogType.Information:
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case LogType.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            default:
                break;
        }

        Console.WriteLine(message);
    }
}
