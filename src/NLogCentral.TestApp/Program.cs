using System;
using NLog;

namespace NLogCentral.TestApp
{
    class Program
    {
        private static Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Press enter to send log");
                if (Console.ReadLine() == "quit")
                    return;

                Logger.Debug("Debug @ {0}", DateTime.Now);
                Logger.Error("Error @ {0}", DateTime.Now);
                Logger.Warn("Warn @ {0}", DateTime.Now);
                Logger.Fatal("Fatal @ {0}", DateTime.Now);
            }
        }
    }
}
