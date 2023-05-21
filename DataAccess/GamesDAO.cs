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
        public abstract IEnumerable<Game> GetGames(string? title = null, string[]? genres = null);
        public abstract bool AddGame(Game game);
        public abstract bool UpdateGame(Game game);
        public abstract Game? FindGameById(Guid id);
        public abstract bool DeleteGame(Game game);
        public abstract IEnumerable<Game> ReloadGames();
    }
}
