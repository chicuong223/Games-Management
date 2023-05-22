using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DataAccess.FileDAO
{
    public class FileGamesDAO : GamesDAO
    {
        public FileGamesDAO()
        { }

        //private static GamesDAO? instance;
        //private static readonly object instanceLock = new object();

        private static readonly string resourcePath = Path.Combine(Directory.GetCurrentDirectory(), AppConstants.ResourceFolderName);

        //public static GamesDAO Instance
        //{
        //    get
        //    {
        //        lock (instanceLock)
        //        {
        //            if (instance == null)
        //            {
        //                instance = new GamesDAO();
        //            }
        //            return instance;
        //        }
        //    }
        //}

        //public override IEnumerable<Game> GetGames(string? title = null, string[]? genres = null)
        //{
        //    IEnumerable<Game> games = Cache.Games;
        //    if (!string.IsNullOrEmpty(title))
        //    {
        //        games = games.Where(game => game.Title.Contains(title, StringComparison.InvariantCultureIgnoreCase));
        //    }
        //    if (genres != null && genres.Length > 0)
        //    {
        //        List<Game> tmp = new List<Game>();
        //        foreach (var game in games)
        //        {
        //            foreach (var genre in genres)
        //            {
        //                if (game.Genres.Any(g => g.Name.Equals(genre)))
        //                {
        //                    tmp.Add(game);
        //                    break;
        //                }
        //            }
        //        }
        //        games = tmp;
        //    }
        //    return games;
        //}

        public override bool AddGame(Game game)
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

        public override bool UpdateGame(Game game)
        {
            try
            {
                Game? gameToUpdate = Cache.Games.FirstOrDefault(g => g.Id.Equals(game.Id));
                if (gameToUpdate == null)
                {
                    throw new Exception($"Game with ID {game.Id} could not be found!");
                }
                gameToUpdate.Title = game.Title;
                gameToUpdate.ImagePath = game.ImagePath;
                gameToUpdate.ExecutablePath = game.ExecutablePath;
                gameToUpdate.Genres = game.Genres;
                //Cache.Games.Remove(gameToUpdate);
                //Cache.Games.Add(game);
                //var dic = Cache.Games.ToDictionary(g => g.Id);
                //Game? gameToUpdate;
                //if (dic.TryGetValue(game.Id, out gameToUpdate))
                //{
                //    gameToUpdate = game;
                //}
                XmlUtils.WriteToXml(Cache.Games, resourcePath, AppConstants.GamesFileName);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public override Game? FindGameById(Guid id)
        {
            Game? result = null;
            try
            {
                List<Game>? games = XmlUtils.ReadFromXml<List<Game>>(Path.Combine(resourcePath, AppConstants.GamesFileName));
                if(games != null)
                {
                    result = games.FirstOrDefault(g => g.Id == id);
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

        //public void ReloadData()
        //{
        //    Cache.ReloadGames();
        //    Cache.ReloadGenres();
        //}

        public override bool DeleteGame(Game game)
        {
            if (Cache.Games.Contains(game))
            {
                try
                {
                    Cache.Games.Remove(game);
                    XmlUtils.WriteToXml(Cache.Games, resourcePath, AppConstants.GamesFileName);
                    return true;
                }
                catch
                {
                    throw;
                }
            }
            return false;
        }

        public override IEnumerable<Game> ReloadGames()
        {
            IEnumerable<Game>? games;
            try
            {
                games = XmlUtils.ReadFromXml<List<Game>>(Path.Combine(Directory.GetCurrentDirectory(),
                    AppConstants.ResourceFolderName,
                    AppConstants.GamesFileName));
            }
            catch(FileNotFoundException)
            {
                games = new List<Game>();
            }
            return games != null ? games : new List<Game>();

        }
    }
}