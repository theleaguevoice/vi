using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core.Abstraction
{
    public abstract class ProcessWatcher
    {
        public string ProcessName { get; }
        public string ExecutablePath { get; protected set; }

        protected ProcessWatcher(string processName)
        {
            ProcessName = processName;

            var process = GetProcessesByName(ProcessName);

            if (process.Any())
                ExecutablePath = GetProcessPath(process.First());
            
        }
        
        protected ProcessWatcher()
        {
        }

        protected bool IsProcessStillAlive() => GetProcessesByName(ProcessName).Any();

        protected static Process[] GetProcessesByName(string name)
        {
            var withoutExe = Process.GetProcessesByName(name.Split('.')[0]);

            if (withoutExe.Any())
                return withoutExe;

            var nameDotExe = Process.GetProcessesByName($"{name}.exe");
            return nameDotExe;
        }

        protected string GetProcessPath()
        {
            var process = GetProcessesByName(ProcessName).First();
            return process.MainModule?.FileName;
        }

        public static string GetProcessPath(Process process)
        {
            return process.MainModule?.FileName;
        }

        protected void Invoke()
        {
            if (string.IsNullOrWhiteSpace(ExecutablePath)) return;

            Invoke(ExecutablePath, out _);
        }

        public static void Invoke(string processPath, out Process process)
        {
            process = new Process
            {
                StartInfo =
                {
                    FileName = processPath,
                    UseShellExecute = true,
                    Verb = "runas"
                }
            };
            process.Start();
        }
        
        public void Finish()
        {
            if (string.IsNullOrWhiteSpace(ExecutablePath)) return;

            var process = GetProcessesByName(ExecutablePath);
            
            if (!process.Any()) return;
            
            foreach (var p in process)
            {
                p?.Kill();
            }
        }

        public abstract void Start();

        public abstract void Stop();
    }
}