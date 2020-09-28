using System;
using Core.Abstraction;
using Core.Data.Model;

namespace Runner
{
    public class LockfileWatcher : TextFileWatcher<Lockfile>
    {
        public LockfileWatcher(IFileHandler<Lockfile> fileHandler) : base(fileHandler)
        {
        }

        protected override void OnFileChanged(Lockfile file)
        {
            Console.WriteLine(file.ToString());
        }
    }
}