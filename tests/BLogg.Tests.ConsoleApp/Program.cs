using BLogg.Core;
using BLogg.Core.Events;
using BLogg.Core.Processing.BuiltIn;
using System;

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
                })
                .WithDefaultLogLevel(LogLevel.Debug)
                .Build();

            var my = new MyClass();

            logger.Log("Hello");
            logger.LogInformation("An information message with a property formatted: {!MyClass}", my);
            logger.LogFatal("A fatal error", new Exception("Failed"));

            Console.ReadLine();
        }
    }

    public class MyClass
    {
        public int Number = 5;

        public bool Boolean = true;
    }
}
