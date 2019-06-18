using BLogg.Core;
using BLogg.Core.Processing.BuiltIn;
using System;

namespace BLogg.Tests.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LoggerMaker.MakeNew()
                .Processors
                    .AddConsole(settings => 
                    {
                        settings.Writer = Console.Out;
                    }));

            Console.ReadLine();
        }
    }
}
