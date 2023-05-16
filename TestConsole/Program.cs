using DataAccess.Configuration;
using DataAccess.DatabaseDAO;
using Oracle.ManagedDataAccess.Client;

Config config = new Config
{
    SaveType = "database",
    DatabaseConfig = new DatabaseConfig("localhost:1521/ORCLCDB", "sys", "sys")
};

string connectionString = DatabaseUtils.CreateConnectionStringFromConfig(config);
try
{
    OracleConnection conn = DatabaseUtils.MakeConnection(connectionString);
    DatabaseUtils.InsertGenres(conn);
}
catch
{
    throw;
}
