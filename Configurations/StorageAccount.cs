namespace BackupStorage
{
    public class StorageAccount
    {
        public StorageAccount(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; private set; }


        public void SetConnectionString(string connectionString){
            ConnectionString = connectionString;
        }
    }

}