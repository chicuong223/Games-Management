using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configuration
{
    public class DatabaseConfig
    {
        public string DatabaseName { get; set; } = "";
        public string UserId { get; set; } = "";
        public string Password { get; set; } = "";

        public DatabaseConfig(string databaseName, string userId, string password)
        {
            DatabaseName = databaseName;
            UserId = userId;
            Password = password;
        }
    }
}
