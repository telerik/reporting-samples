using System;
using Telerik.Reporting.Interfaces;

namespace CrystalReportsConverter
{
    class ConsoleLogger : Telerik.Reporting.Interfaces.ILog
    {
        public void LogError(string message)
        {
            Console.WriteLine($"Error: {message}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"Warning: {message}");
        }

        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        void ILog.Log(string message)
        {
            this.LogInfo(message);
        }
    }
}
