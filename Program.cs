using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Azure.Storage.Blobs;
using CommandLine;

namespace BackupStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
              .WithParsed(Run)
              .WithNotParsed(HandleParseError);
        }
        static void Run(Options opts)
        {
            if (opts.Settings)
                SetConfigurations(opts);


            if (opts.Backup)
            {
                var config = JsonSerializer.Deserialize<Configuration>(File.ReadAllText("config.json"));
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
                    BlobClient blobClient = container.GetBlobClient(file.Path);
                    using FileStream uploadFileStream = File.OpenRead(file.Path);
                    blobClient.Upload(uploadFileStream, true);
                    uploadFileStream.Close();
                    Message($"Arquivo:{file.Path}, Container: {file.Container}. ok!");
                }
                Message("Backup finalizado");
            }
        }

        private static void SetConfigurations(Options opts)
        {
            Console.WriteLine("Configurar backup");
            string fileName = "config.json";
            JsonSerializerOptions options = new() { WriteIndented = true };
            var config = new Configuration();

            if (!File.Exists(fileName))
            {                
                config.Create();
                File.WriteAllText(fileName, config.ToString());
            }

            config = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(fileName), options);

            if (!string.IsNullOrEmpty(opts.ConnectionString))
            {
                config.StorageAccount.SetConnectionString(opts.ConnectionString);
                Message("Conexão adicionada");
            }

            if (!string.IsNullOrEmpty(opts.AddFile))
            {
                config.AddFile(opts.AddFile);
                Message("Arquivo adicionado.");
            }

            if (!string.IsNullOrEmpty(opts.RemoveFile))
            {
                config.RemoveFile(opts.RemoveFile);
                Message("Arquivo removido.");
            }

            if (opts.CleanConfiguration)
            {
                config.Clean();
                Message("Arquivo limpado.");
            }

            File.WriteAllText(fileName, config.ToString());

            Message(config.ToString());
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Erros");
        }

        static void Message(string message, bool success = true)
        {
            if (success)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
