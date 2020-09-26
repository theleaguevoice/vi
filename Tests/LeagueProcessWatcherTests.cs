using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Core.Abstraction;
using Core.Watchers;
using Xunit;

namespace Tests
{
    public class LeagueProcessWatcherTests
    {
        [Fact]
        public void CallInvokeActuallyInvokesTheProcess()
        {
            // true if there are any league processes running
            var firstState = Process.GetProcessesByName("LeagueClientUx").Any();

            var leaguePath = Environment.GetEnvironmentVariable("LeaguePath");

            // should create an League Client Instance
            ProcessWatcher.Invoke(leaguePath, out var process);
            
            Thread.Sleep(10*1000);

            var secondState = Process.GetProcessesByName("LeagueClientUx").Any();
            
            Assert.True(secondState && !firstState);

        }
    }
}