using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Azure.Storage.Blobs;
using CommandLine;

namespace BackupStorage
{
    public partial class Program
    {

        static string configFilePath = "config.json";
        static Configuration config;
        static JsonSerializerOptions options = new() { WriteIndented = true };
        static void Main(string[] args)
        {
            InitializeConfig();

            CommandLine.Parser.Default.ParseArguments<Options>(args)
              .WithParsed(Run)
              .WithNotParsed(HandleParseError);
        }

        private static void InitializeConfig()
        {
            config = new Configuration();

            if (!File.Exists(configFilePath))
            {
                config.Create();
                File.WriteAllText(configFilePath, config.ToString());
            }

            config = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(configFilePath), options);
        }

        static void Run(Options opts)
        {
            if (opts.Settings)
                SetConfigurations(opts);

            if (opts.Backup)
                Backup();
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
