using System;
using CamelliaBackend.Manager;
using Godot;
using Environment = System.Environment;

namespace CamelliaFrontend.scripts.Implementation;

public class LoggingImpl : LoggingManager.ILoggingImpl
{
    public void Log(LoggingManager.ILoggingImpl.Levels level, params string[] messages)
    {
        var levelTag = level switch
        {
            LoggingManager.ILoggingImpl.Levels.Debug => "DE",
            LoggingManager.ILoggingImpl.Levels.Info => "IN",
            LoggingManager.ILoggingImpl.Levels.Warning => "WA",
            LoggingManager.ILoggingImpl.Levels.Error => "ER",
            LoggingManager.ILoggingImpl.Levels.Fatal => "FA",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        var logText = $"[{DateTime.Now:g}] <{levelTag}> {string.Join(Environment.NewLine, messages)}";
        
        if (level <= LoggingManager.ILoggingImpl.Levels.Warning) GD.Print(logText);
        else GD.PrintErr(logText);
    }
}