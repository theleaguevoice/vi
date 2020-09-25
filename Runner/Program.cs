using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using Core.Watchers;

namespace Runner
{
    internal static class Program
    {
        public static void Main()
        {
         
            var watcher = new LeagueProcessWatcher();
            
            watcher.OnLeagueStarted += path => Console.WriteLine($"League has started on path: {path}"); 
            watcher.OnLeagueStopped += () => Console.WriteLine("League Stopped");
            
            watcher.Start();

            Console.WriteLine("Press any key to exit");
            while (!Console.KeyAvailable) System.Threading.Thread.Sleep(50);

           watcher.Stop();
        }
        
    }
}