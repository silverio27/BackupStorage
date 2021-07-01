using System;
using CommandLine;

namespace BackupStorage
{

    public class Options
    {
        [Option('s', "settings", Required = false, HelpText = "Configurações")]
        public bool Settings { get; set; }

        [Option('u', "connection-string", Required = false, HelpText = "String de conexão do storage")]
        public string ConnectionString { get; set; }

        [Option('a', "add-file", Required = false, HelpText = "Adicionar caminho do arquivo para backup")]
        public string AddFile { get; set; }

        [Option('r', "remove-file", Required = false, HelpText = "Remover arquivo para backup pelo caminho")]
        public string RemoveFile { get; set; }
        [Option( "retention", Required = false, HelpText = "Configurar retenção")]
        public int? Retention { get; set; }

        [Option('c', "clean", Required = false, HelpText = "Limpar arquivo de configuração")]
        public bool CleanConfiguration { get; set; }

        [Option('b', "backup", Required = false, HelpText = "Backup")]
        public bool Backup { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Exibe os resultados")]
        public bool Verbose { get; set; }

        void Log(string message, params string[] args)
        {
            if (Verbose)
            {
                Console.WriteLine(message, args);
            }
        }
    }
}