using Core;
using Core.Handlers;
using Core.Watchers;

namespace Runner
{
    internal static class Program
    {
        private static SecurityManager _manager;

        public static void Main()
        {
            Init();
        }

        private static void Init()
        {
            var processWatcher = new LeagueProcessWatcher();
            var lockfileWatcher = new LockfileWatcher(new LockFileHandler());

            _manager = new SecurityManager(processWatcher, lockfileWatcher);
            _manager.Start();
        }
    }
}