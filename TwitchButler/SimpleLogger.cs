using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TwitchButler
{
    public enum LoggingSeverity
    {
        Information,
        Warning,
        Error,
        Ciritical,
        Exception
    }

    public static class SimpleLogger
    {
        public static void Log(LoggingSeverity severity, string message)
        {
            string severityPrefix = string.Format("[{0}]", severity.ToString().ToUpper());
            SetConsoleColor(severity);
            Console.WriteLine(string.Format("[{0}]{1} : {2}", DateTime.Now.ToString(),severityPrefix, message));
        }

        private static void SetConsoleColor(LoggingSeverity severity)
        {
            Console.ForegroundColor = ConsoleColor.White;
            switch(severity)
            {
                case LoggingSeverity.Ciritical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LoggingSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LoggingSeverity.Exception:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case LoggingSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
        }
        

    }
}
