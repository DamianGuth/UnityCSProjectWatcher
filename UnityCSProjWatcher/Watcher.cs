namespace UnityCSProjWatcher
{
    public class Watcher
    {

        private readonly string _assemblyCSharpName = "Assembly-CSharp.csproj";
        private string? _currentDirectory;
        private string? _projectDirectory;

        public void WaitForFileChanges()
        {
            _currentDirectory = Directory.GetCurrentDirectory();

            DirectoryInfo? parentDirectory = Directory.GetParent(_currentDirectory);

            if (parentDirectory != null)
            {
                _projectDirectory = parentDirectory.FullName;
            }
            else
            {
                _projectDirectory = _currentDirectory;
            }

            FileSystemWatcher fsw = new(_projectDirectory, _assemblyCSharpName);
            fsw.Changed += Fsw_Changed;
            fsw.EnableRaisingEvents = true;
        }

        public void ApplyManually()
        {
            ReadFileAndApplyRules(_projectDirectory.TrimEnd('/').TrimEnd('\\') + "/" + _assemblyCSharpName, new AssemblyCSharpRule());
        }

        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            // Update the hash of the file.
            ReadFileAndApplyRules(e.FullPath, new AssemblyCSharpRule());
        }

        private static void ReadFileAndApplyRules(string path, IFileRule fileRule)
        {
            // Here we want to check if the file is locked and wait when it is locked.
            int accesRequestCount = 0;

            while (accesRequestCount < 10 && !TryAndApplyFileRules(path, fileRule))
            {
                // Increase request count.
                accesRequestCount++;

                if (accesRequestCount == 10)
                {
                    Console.WriteLine("Failed to access file after 10 tries.");
                }
                else
                {
                    Console.WriteLine("Failed to access file... retry in 500ms.");
                    // Wait 500 ms.
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        private static bool TryAndApplyFileRules(string path, IFileRule fileRule)
        {
            try
            {
                string[] lines = File.ReadAllLines(path);
                fileRule.ReplaceAllDataInLines(lines);
                File.WriteAllLines(path, lines);
            }
            catch (IOException)
            {
                return false;
            }

            return true;
        }
    }
}