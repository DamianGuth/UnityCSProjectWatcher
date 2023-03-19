using UnityCSProjWatcher;

string? command;

Console.WriteLine("Registering watcher");
Watcher watcher = new();
watcher.WaitForFileChanges();
Console.WriteLine("Waiting for filechanges");

Console.WriteLine("type 'exit' to exit the application.");

while(true)
{
    command = Console.ReadLine();
    if(command == "exit")
    {
        Console.WriteLine("Exiting...");
        Environment.Exit(0);
    }
}