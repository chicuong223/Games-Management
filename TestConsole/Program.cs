using DataAccess.Configuration;
using DataAccess.DatabaseDAO;
using Oracle.ManagedDataAccess.Client;

Config config = new Config
{
    SaveType = "database",
    DatabaseConfig = new DatabaseConfig("localhost:1521/ORCLCDB", "sys", "sys")
};

string connectionString = DatabaseUtil.CreateConnectionStringFromConfig(config);
try
{
    OracleConnection conn = DatabaseUtil.MakeConnection(connectionString);
    conn.Open();
}
catch
{
    throw;
}
