using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DataAccess
{
    public class GamesDAO
    {
        private GamesDAO()
        { }

        private static GamesDAO? instance;
        private static readonly object instanceLock = new object();

        private static readonly string resourcePath = Path.Combine(Directory.GetCurrentDirectory(), AppConstants.ResourceFolderName);

        public static GamesDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new GamesDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Game> GetGames(string? title = null, string[]? genres = null)
        {
            IEnumerable<Game> games = Cache.Games;
            if (!string.IsNullOrEmpty(title))
            {
                games = games.Where(game => game.Title.Contains(title, StringComparison.InvariantCultureIgnoreCase));
            }
            if (genres != null && genres.Length > 0)
            {
                List<Game> tmp = new List<Game>();
                foreach (var game in games)
                {
                    foreach (var genre in genres)
                    {
                        if (game.Genres.Any(g => g.Name.Equals(genre)))
                        {
                            tmp.Add(game);
                            break;
                        }
                    }
                }
                games = tmp;
            }
            return games;
        }

        public bool AddGame(Game game)
        {
            try
            {
                Cache.Games.Add(game);
                XmlUtils.WriteToXml(Cache.Games, resourcePath, AppConstants.GamesFileName);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateGame(Game game)
        {
            try
            {
                Game? gameToUpdate = Cache.Games.FirstOrDefault(g => g.Id.Equals(game));
                if (gameToUpdate == null)
                {
                    throw new Exception($"Game with ID {game.Id} could not be found!");
                }
                gameToUpdate = game;
                XmlUtils.WriteToXml(Cache.Games, resourcePath, AppConstants.GamesFileName);
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}