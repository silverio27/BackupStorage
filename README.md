# Backup Storage

Envie arquivo para o azure storage account através da linha de comando

## Como usar

### Parâmetros
```CMD
--settings "Configurações"
--connection-string "String de conexão do storage"
--add-file  "Adicionar caminho do arquivo para backup"
--remove-file "Remover arquivo para backup pelo caminho"
--clean "Limpar arquivo de configuração"
--backup "Executa a transferncia dos arquivos"
```


Ver as configurações atual
```CMD
 .\BackupStorage.exe -s
```
você verá o resultado:
```JSON
{
  "StorageAccount": {
    "ConnectionString": ""
  },
  "FilesToBackup": []
}
```
Adicionar um string de conexão
```CMD
 .\BackupStorage.exe -s --connection-string "<connectionString>"
```
Adicionar um arquivo para um container. O caminho para o arquivo fica na primeira parte da string,
depois do ponto e virgula(;) é informado o container.
>Caso o container não existir ele será criado, adicione quantos arquivos quiser
```CMD
.\BackupStorage.exe -s --add-file "C:\backup\teste.txt;backup"
```
Executar o backup, todos arquivos listado serão executados
```CMD
.\BackupStorage.exe -b
```


