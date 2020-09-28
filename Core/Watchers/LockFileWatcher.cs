using System;
using Core.Abstraction;
using Core.Data.Model;

namespace Core.Watchers
{
    public class LockFileWatcher : TextFileWatcher<Lockfile>
    {
        public event Action<Lockfile> OnFileChange; 
        public LockFileWatcher(string watchPath, IFileHandler<Lockfile> fileHandler) : base(watchPath, fileHandler)
        {
        }

        protected override void OnFileChanged(Lockfile file)
        {
           Console.WriteLine($"League Client Authenticated url => {file}");
           OnFileChange?.Invoke(file);
        }
    }
}