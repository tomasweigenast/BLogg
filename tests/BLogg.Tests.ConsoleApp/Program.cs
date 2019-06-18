using BLogg.Core;
using BLogg.Core.Events;
using BLogg.Core.Processing.BuiltIn;
using System;
using System.IO;

namespace BLogg.Tests.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LoggerMaker.MakeNew()
                .WithProcessor.File(settings => { settings.Path = $"{Directory.GetCurrentDirectory()}\\logs"; })
                .WithProcessor.Console()
                .WithDefaultLogLevel(LogLevel.Debug)
                .Build();

            Console.ReadLine();
        }
    }
}
