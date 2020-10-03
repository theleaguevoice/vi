using Vi;
using Vi.Abstraction;
using Vi.Data.Model;
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
            TextFileWatcher<Lockfile> lockfileWatcher = new LockfileWatcher(new LockFileHandler());
            ILeagueProcessWatcher processWatcher = new LeagueProcessWatcher();

            // references may be provided by some IOC container
            _manager = new SecurityManager(processWatcher, lockfileWatcher);
            
            // this call will block the main Thread, this whole manager should run within dedicated Thread. 
            _manager.Start();
        }
    }
}