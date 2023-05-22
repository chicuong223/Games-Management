using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configuration
{
    public class DatabaseConfig
    {
        public DatabaseConfig()
        {
        }

        public DatabaseConfig(string serverAddress, int port, string serviceName, string userId, string password, bool sysDba)
        {
            ServerAddress = serverAddress;
            Port = port;
            ServiceName = serviceName;
            UserId = userId;
            Password = password;
            SysDba = sysDba;
        }

        public string ServerAddress { get; set; } = "";
        public int Port { get; set; }
        public string ServiceName { get; set; } = "";
        public string UserId { get; set; } = "";
        public string Password { get; set; } = "";
        public bool SysDba { get; set; }

    }
}
