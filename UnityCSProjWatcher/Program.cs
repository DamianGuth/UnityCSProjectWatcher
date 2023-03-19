using UnityCSProjWatcher;

string? command;

Watcher watcher = new();
watcher.WaitForFileChanges();

while(true)
{
    command = Console.ReadLine();
    if(command == "exit")
    {
        Environment.Exit(0);
    }
}