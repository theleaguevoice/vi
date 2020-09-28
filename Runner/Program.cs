using System;
using System.IO;
using System.Threading;
using Core.Handlers;
using Core.Watchers;

namespace Runner
{
    internal static class Program
    {
        private static LeagueProcessWatcher _processWatcher;
        private static LockFileWatcher _lockfileWatcher = null;

        public static void Main()
        {
            ProcessWatcher();

            Init();

            Console.WriteLine("Press any key to exit");
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(50);

                var leaguePath = _processWatcher.ExecutablePath;
                                    
                if(string.IsNullOrEmpty(leaguePath))
                    continue;

                var leagueDir = Path.GetDirectoryName(leaguePath);

                if (_lockfileWatcher != null)
                    continue;

                _lockfileWatcher = new LockFileWatcher(leagueDir, new LockFileHandler());
                _lockfileWatcher.Start();
            }

            Stop();
        }

        private static void Init()
        {
            _processWatcher.Start();
        }

        private static void Stop()
        {
            _processWatcher.Stop();
            _lockfileWatcher.Stop();
        }

        private static void ProcessWatcher()
        {
            _processWatcher = new LeagueProcessWatcher();

            _processWatcher.OnLeagueStarted += path => Console.WriteLine($"League has started on path: {path}");
            _processWatcher.OnLeagueStopped += () => Console.WriteLine("League Stopped");
        }
    }
}