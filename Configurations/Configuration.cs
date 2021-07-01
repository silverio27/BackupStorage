using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BackupStorage
{
    public class Configuration
    {
        public StorageAccount StorageAccount { get; set; }
        public IList<FileToBackup> FilesToBackup { get; set; }
        public int Retention { get; set;}

        public override string ToString()
        {
            JsonSerializerOptions options = new() { WriteIndented = true };
            return JsonSerializer.Serialize(this, options);
        }
        public void AddFile(string file)
        {
            if (FilesToBackup == null)
                FilesToBackup = new List<FileToBackup>();

            var fileContainer = file.Split(';');
            var fileToBackup = new FileToBackup(fileContainer[0], fileContainer[1]);
            FilesToBackup.Add(fileToBackup);
        }

        public void RemoveFile(string path)
        {
            if (FilesToBackup == null)
                return;

            var file = FilesToBackup.FirstOrDefault(x => x.Path == path);
            FilesToBackup.Remove(file);
        }

        public void Clean()
        {
            this.StorageAccount.SetConnectionString("");
            this.Retention = 0;
            this.FilesToBackup = new List<FileToBackup>();
        }
        public void Create()
        {
            this.StorageAccount = new StorageAccount("");
            this.FilesToBackup = new List<FileToBackup>();
            this.Retention = 0;
        }

    }

}