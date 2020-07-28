using System;
using System.Transactions;

namespace TwitchButler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            SimpleLogger.Log(LoggingSeverity.Ciritical, "lol xd critical");
            SimpleLogger.Log(LoggingSeverity.Error, "lol xd error test");
            SimpleLogger.Log(LoggingSeverity.Information, "lol xd information");
            SimpleLogger.Log(LoggingSeverity.Warning, "lol xd warning");

            Console.ForegroundColor = ConsoleColor.White;


        }
    }
}
