namespace BackupStorage
{
    public class FileToBackup
    {
        public FileToBackup(string path, string container)
        {
            Path = path;
            Container = container;
        }
        public string Path { get; private set; }
        public string Container { get; private set; }
    }

}