using System;
using System.Linq;
using System.Management;
using Core.Abstraction;

namespace Core.Watchers
{
    public class LeagueProcessWatcher : ProcessWatcher
    {
        public event Action<string> OnLeagueStarted;
        public event Action OnLeagueStopped;

        private ManagementEventWatcher _startWatcher;
        private ManagementEventWatcher _stopWatcher;

        public bool IsLeagueRunning { get; private set; }

        public LeagueProcessWatcher() : base("LeagueClientUx")
        {
        }

        public override void Start()
        {

            _startWatcher = new ManagementEventWatcher(
                new WqlEventQuery(@$"SELECT * FROM Win32_ProcessStartTrace WHERE ProcessName = '{ProcessName}.exe'"));

            _stopWatcher = new ManagementEventWatcher(
                new WqlEventQuery(@$"SELECT * FROM Win32_ProcessStopTrace WHERE ProcessName = '{ProcessName}'"));

            _startWatcher.EventArrived += StartWatcherOnEventArrived;
            _stopWatcher.EventArrived += StopWatcherOnEventArrived;

            _startWatcher.Start();
            _stopWatcher.Start();
        }

        private void StopWatcherOnEventArrived(object sender, EventArrivedEventArgs e)
        {
            if (!IsLeagueRunning)
                return;

            IsLeagueRunning = false;
            OnLeagueStopped?.Invoke();
        }

        private void StartWatcherOnEventArrived(object sender, EventArrivedEventArgs e)
        {
            var processName = (string) e.NewEvent.Properties["ProcessName"].Value;
            var process = GetProcessesByName(processName);

            if (!process.Any())
                return;

            var league = process[0];
            var processPath = GetProcessPath(league);

            OnLeagueStarted?.Invoke(ExecutablePath);

            IsLeagueRunning = true;
            ExecutablePath = processPath;
        }

        public override void Stop()
        {
            _startWatcher?.Stop();
            _stopWatcher?.Stop();
        }
    }
}