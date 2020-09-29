using System;

namespace Vi.Abstraction
{
    public interface ILeagueProcessWatcher
    {
        public event Action<string> OnLeagueStarted;
        public event Action OnLeagueStopped;
    }
}