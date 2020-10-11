# Vi - LCU Auth Lib

When dealing with LCU Apis and WebSockets, apps must read auth parameters from a `lockfile`, which is generated on every League Client initialization.

### To address this problem we've created VI 🚔🚔

![VI](https://giffiles.alphacoders.com/527/52728.gif)

## How it works 
Vi is a library written in `C#` to be used, in dotnet projects.

It handles the flow of getting Lcu `auth token` and websocket `port`.

## How to use it 

````c#
using Vi;
using Vi.Handlers;
using Vi.Watchers;

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
        
        // this call will block the main Thread, this whole manager should run within his own dedicated Thread. 
        _manager.Start();
    }
}
````
