using System;
using Vi.Abstraction;
using Vi.Data.Model;

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