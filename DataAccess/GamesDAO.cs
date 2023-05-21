using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public abstract class GamesDAO
    {
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
        public abstract bool AddGame(Game game);
        public abstract bool UpdateGame(Game game);
        public abstract Game? FindGameById(Guid id);
        public abstract bool DeleteGame(Game game);
        public abstract IEnumerable<Game> ReloadGames();
    }
}
