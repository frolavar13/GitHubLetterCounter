using System;
using System.IO;

public static class Logger
{
    // This route should be inclouded in a configuration file
    private static readonly string logFilePath = "logs/GitHubLetterCountLog.txt";

    static Logger()
    {
        // Create the directory
        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
    }

    // Write general information
    public static void Info(string message)
    {
        Log("INFO", message);
    }

    // Write warning information
    public static void Warning(string message)
    {
        Log("WARNING", message);
    }

    // Write error information
    public static void Error(string message)
    {
        Log("ERROR", message);
    }


    // Write the information on the log file
    private static void Log(string logLevel, string message)
    {
        var logMessage = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}";

        try
        {
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write log: {ex.Message}");
        }
    }
}
