using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Azure.Storage.Blobs;

namespace BackupStorage
{
    public partial class Program
    {
        private static void Backup()
        {
            if (string.IsNullOrEmpty(config.StorageAccount.ConnectionString))
            {
                Message("Informe uma conexão: dotnet run -s -connection-string <connectionString>");
                return;
            }
            if (config.FilesToBackup.Count == 0)
            {
                Message("Informe um arquivo para upload: dotnet run -s -add-file <caminho;container>");
                return;
            }

            BlobServiceClient blobServiceClient = new BlobServiceClient(config.StorageAccount.ConnectionString);
            foreach (var file in config.FilesToBackup)
            {
                var container = blobServiceClient.GetBlobContainerClient(file.Container);
                container.CreateIfNotExists();

                CheckBlobsToDelete(container);
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.Path)}";

                BlobClient blobClient = container.GetBlobClient(fileName);
                using FileStream uploadFileStream = new FileStream(file.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                blobClient.Upload(uploadFileStream, true);
                uploadFileStream.Close();
                Message($"Arquivo:{file.Path}, Container: {file.Container}. ok!");
            }
            Message("Backup finalizado");
        }

        private static void CheckBlobsToDelete(BlobContainerClient container)
        {
            var blobs = container.GetBlobs();
            if (blobs.Count() >= 7)
            {
                var oldDate = blobs.Min(x => x.Properties.CreatedOn);
                var blobToDelete = blobs.FirstOrDefault(x => x.Properties.CreatedOn == oldDate);
                container.DeleteBlob(blobToDelete.Name);
                Console.WriteLine("Arquivo Excluído:{0}, Data:{1}", blobToDelete.Name, blobToDelete.Properties.CreatedOn);
            }
        }
    }
}