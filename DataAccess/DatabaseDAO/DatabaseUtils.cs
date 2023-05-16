using DataAccess.Configuration;
using Models;
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
        private static readonly string ScriptsFolder = "SqlScripts";
        private static readonly string InitializeScript = "InitializeDBScript.txt";
        private static readonly string InsertGenresScript = "InsertGenreScript.txt";

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
                    if (dbConfig.UserId.ToLower().Equals("sys"))
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

        public static void CreateTables(OracleConnection connection)
        {
            try
            {
                string query = createQueryStringFromFileName(InitializeScript);
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection = connection;
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static void InsertGenres(OracleConnection connection)
        {
            try
            {
                List<Genre>? genres = XmlUtils.ReadFromXml<List<Genre>>(Path.Combine(Directory.GetCurrentDirectory(),
                    AppConstants.ResourceFolderName,
                    AppConstants.GenresFileName));
                if (genres != null)
                {

                    //Get genre names and remove duplicates
                    HashSet<string> genreSet = new HashSet<string>(
                        from genre in genres
                        select genre.Name
                        );

                    //open database connection
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    //insert genres
                    foreach (string genre in genreSet)
                    {
                        //Execute query
                        using (OracleCommand cmd = new OracleCommand())
                        {
                            cmd.Connection = connection;
                            cmd.CommandText = createQueryStringFromFileName(InsertGenresScript);
                            cmd.Parameters.Add("name", genre);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private static string createQueryStringFromFileName(string fileName)
        {
            string query = "";
            string? line;

            try
            {
                //Create query from script file
                using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(),
                            ScriptsFolder,
                            fileName)))
                {
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        query = string.Concat(query, line.Trim());
                        line = reader.ReadLine();
                    }
                }
                return query;
            }
            catch
            {
                throw;
            }
        }
    }
}
