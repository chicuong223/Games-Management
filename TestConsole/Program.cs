using DataAccess.Configuration;
using DataAccess.DatabaseDAO;
using Oracle.ManagedDataAccess.Client;

Config config = new Config
{
    SaveType = "database",
    DatabaseConfig = new DatabaseConfig("localhost", 1521, "ORCLCDB", "sys", "sys", true)
};

string connectionString = DatabaseUtils.CreateConnectionStringFromConfig(config);
try
{
    bool success = DatabaseUtils.TestConnection(connectionString);
    Console.WriteLine(success);
}
catch
{
    throw;
}
