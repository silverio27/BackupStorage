using System;
using System.IO;
using System.Text.Json;

namespace BackupStorage
{
    public partial class Program
    {
        private static void SetConfigurations(Options opts)
        {
            Console.WriteLine("Configurar backup");

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

            if (opts.Retention != null)
            {
                config.Retention = opts.Retention ?? 0;
                Message("Arquivo removido.");
            }

            if (opts.CleanConfiguration)
            {
                config.Clean();
                Message("Arquivo limpado.");
            }

            File.WriteAllText(configFilePath, config.ToString());

            Message(config.ToString());
        }


    }
}