using System;
using System.IO;
using System.Threading;
using Core.Abstraction;
using Core.Data.Model;

namespace Core
{
    public class SecurityManager : IProcess
    {
        private ILeagueProcessWatcher _processWatcher;
        private TextFileWatcher<Lockfile> _lockFileWatcher;
        private bool _lock;

        private string LeaguePath { get; set; }
        
        public SecurityManager(ILeagueProcessWatcher processWatcher, TextFileWatcher<Lockfile> lockFileWatcher
           )
        {
            _processWatcher = processWatcher;
            _lockFileWatcher = lockFileWatcher;
        }

        private void StartProcessWatcher()
        {
            _processWatcher.OnLeagueStarted += OnLeagueStart;
            _processWatcher.OnLeagueStopped += OnLeagueStop;
            ((IProcess) _processWatcher).Start();
        }

        private static void OnLeagueStop()
        {
            Console.WriteLine("League Stopped");
        }

        private void OnLeagueStart(string path)
        {
            Console.WriteLine($"League has started on path => {path}");
            if (File.Exists(path)) 
                LeaguePath = Path.GetDirectoryName(path);
        }

        public void Start()
        {
            _lock = true;
            StartProcessWatcher();
            StartMainLoop();
        }

        private void StartMainLoop()
        {
            while (_lock)
            {
                MainLoop();
                Thread.Sleep(15);
            }
        }

        private void MainLoop()
        {
            if(string.IsNullOrWhiteSpace(LeaguePath))
                return;
            
            if(_lockFileWatcher.IsRunning)
               return;
            
            _lockFileWatcher.SetPath(LeaguePath);
            _lockFileWatcher.Start();
        }

        public void Stop()
        {
            ((IProcess) _processWatcher)?.Stop();
            _lockFileWatcher?.Stop();
            _lock = false;
        }
    }
}