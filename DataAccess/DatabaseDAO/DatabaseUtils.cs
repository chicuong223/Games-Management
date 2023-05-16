using DataAccess.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Utils;

namespace DataAccess.DatabaseDAO
{
    public static class DatabaseUtils
    {
        public static string CreateConnectionStringFromConfig(Config config)
        {
            string connectionString = "";
            try
            {
                //XmlSerializer xmlSerializer = new XmlSerializer(typeof(Config));
                //Cconfig = XmlUtils.ReadFromXml<Config>(configFilePath);
                //if(config != null)
                //{
                DatabaseConfig? dbConfig = config.DatabaseConfig;
                if (dbConfig != null)
                {
                    connectionString = $"Data Source={dbConfig.DatabaseName};User Id={dbConfig.UserId};password={dbConfig.Password}";
                    if(dbConfig.UserId.ToLower().Equals("sys"))
                    {
                        connectionString = string.Concat(connectionString, ";DBA Privilege=SYSDBA;");
                    }
                }
                //}
            }
            catch
            {
                throw;
            }
            return connectionString;
        }

        public static string CreateConnectionString(string databaseSource, string userId, string password)
        {
            string connectionString
                = $"Data Source={databaseSource};User Id={userId};password={password}";
            return connectionString;
        }

        public static OracleConnection MakeConnection(string connectionString)
        {
            try
            {
                OracleConnection connection = new OracleConnection(connectionString);
                return connection;
            }
            catch
            {
                throw;
            }
        }
        
        public static bool TestConnection(string connectionString)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            try
            {
                connection.Open();
                return true;    
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
