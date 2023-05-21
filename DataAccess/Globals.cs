using DataAccess;
using DataAccess.Configuration;
using DataAccess.FileDAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class Globals
    {
        public static Config Config = new Config();
        public static GamesDAO GamesDAO = new FileGamesDAO();
    }
}
