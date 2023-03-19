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

        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            // Update the hash of the file.
            ReadFileAndApplyRules(e.FullPath, new AssemblyCSharpRule());
        }

        private static void ReadFileAndApplyRules(string path, IFileRule fileRule)
        {
            string[] lines = File.ReadAllLines(path);
            fileRule.ReplaceAllDataInLines(lines);
            File.WriteAllLines(path, lines);
        }
    }
}