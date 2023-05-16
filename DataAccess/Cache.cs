#nullable disable

using DataAccess.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DataAccess
{
    public static class Cache
    {
        public static List<Game> Games = new List<Game>();
        public static List<Genre> Genres = new List<Genre>();
        public static Config Config = new Config();

        public static void ReloadGames()
        {
            try
            {
                Games.Clear();
                Games = XmlUtils.ReadFromXml<List<Game>>(Path.Combine(Directory.GetCurrentDirectory(),
                    AppConstants.ResourceFolderName,
                    AppConstants.GamesFileName));
                if (Games == null)
                {
                    Games = new List<Game>();
                }

                //TODO: if config.SaveType is database, reload from database
            }
            catch
            {
                throw;
            }
        }

        public static void ReloadGenres()
        {
            try
            {
                Genres.Clear();
                Genres = XmlUtils.ReadFromXml<List<Genre>>(Path.Combine(Directory.GetCurrentDirectory(),
                    AppConstants.ResourceFolderName,
                    AppConstants.GenresFileName));

                //TODO: if config.SaveType is database, reload from database
            }
            catch
            {
                throw;
            }
        }

        public static void ReloadConfig()
        {
            try
            {
                string filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(),
                    AppConstants.ResourceFolderName,
                    AppConstants.ConfigFileName));
                Config = XmlUtils.ReadFromXml<Config>(filePath);

                //TODO: if Config.SaveType = database, connect to database
            }
            catch
            {
                throw;
            }
        }

        public static void Reload()
        {
            try
            {
                ReloadConfig();
                ReloadGames();
                ReloadGenres();
            } 
            catch
            {
                throw;
            }
        }
    }
}