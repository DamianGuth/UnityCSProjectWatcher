namespace UnityCSProjWatcher
{
    internal class AssemblyCSharpRule : IFileRule
    {
        public void ReplaceAllDataInLines(string[] lines)
        {
            for(int i = 0; i < lines.Length; i++)
            {
                if(lines[i].Contains("<NoWarn>"))
                {
                    lines[i] = "<NoWarn>0169;S3903;IDE0090<NoWarn>";
                }
            }
        }
    }
}
