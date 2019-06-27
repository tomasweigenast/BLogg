using BLogg.Core;
using BLogg.Core.Events;
using BLogg.Core.Processing.BuiltIn;
using BLogg.Core.Processing.BuiltIn.File;
using System;
using System.IO;

namespace BLogg.Tests.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LoggerMaker.MakeNew()
                .WithProcessor.Console(settings =>
                {
                    //settings.Level = LogLevel.Debug;
                    settings.UseColors = true;
                })
                .WithProcessor.File(settings =>
                {
                    settings.Path = Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\logs").FullName;
                    settings.ChangePeriod = FileChangePeriod.PerMonth;
                    settings.LogLevels = LogLevel.Error | LogLevel.Fatal;
                })
                .WithDefaultLogLevel(LogLevel.Debug)
                .Build();

            var my = new MyClass();

            logger.Log("", LogLevel.Fatal);
            logger.LogDebug("");
            logger.LogInformation("An information message with a property formatted: {!MyClass}", my);
            logger.LogWarning("WARNING!");
            logger.LogError("An simple error.");
            logger.LogFatal("A fatal error", new FileNotFoundException("File not found.", "myFile.cs"));

            Console.ReadLine();
        }
    }

    public class MyClass
    {
        public int Number = 5;

        public bool Boolean = true;
    }
}
