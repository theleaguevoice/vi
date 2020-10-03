using Vi;
using Vi.Handlers;
using Vi.Watchers;

namespace Runner
{
    internal static class Program
    {
        private static SecurityManager _manager;
        public static void Main()
        {
            // you must provide an implementation of `TextFileWatcher<Lockfile>`
            var lockfileWatcher = new LockfileWatcher(new LockFileHandler());
            var processWatcher = new LeagueProcessWatcher();

            // references may be provided by some IOC container
            _manager = new SecurityManager(processWatcher, lockfileWatcher);
            
            // this call will block the main Thread, this whole manager should run within dedicated Thread. 
            _manager.Start();
        }
    }
}