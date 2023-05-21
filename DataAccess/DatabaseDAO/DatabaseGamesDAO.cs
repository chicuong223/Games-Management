using Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DatabaseDAO
{
    public class DatabaseGamesDAO : GamesDAO
    {
        public override bool AddGame(Game game)
        {
            bool result = false;
            string query = "INSERT INTO game(id, title, executablePath, imagePath, genres) VALUES(:id, :title, :executablePath, :imagePath, :genres)";
            try
            {
                //Convert list of genres to genres string
                string genresString = string.Empty;
                int index = 0;
                foreach(var genre in game.Genres)
                {
                    index++;
                    if (index == game.Genres.Count)
                    {
                        genresString += genre.Name;
                    }
                    else
                    {
                        genresString += $"{genre.Name},";
                    }
                }

                using(OracleConnection connection = DatabaseUtils.MakeConnection(Globals.Config))
                {
                    if(connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using(OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add("id", Guid.NewGuid().ToString());
                        command.Parameters.Add("title", game.Title);
                        command.Parameters.Add("executablePath", game.ExecutablePath);
                        command.Parameters.Add("imagePath", game.ImagePath);
                        command.Parameters.Add("genres", genresString);
                        command.ExecuteNonQuery();
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public override bool DeleteGame(Game game)
        {
            throw new NotImplementedException();
        }

        public override Game? FindGameById(Guid id)
        {
            throw new NotImplementedException();
        }

        //public override IEnumerable<Game> GetGames(string? title = null, string[]? genres = null)
        //{
        //    throw new NotImplementedException();
        //}

        public override IEnumerable<Game> ReloadGames()
        {
            List<Game> games = new List<Game>();
            try
            {
                using (OracleConnection connection = DatabaseUtils.MakeConnection(Globals.Config))
                {
                    connection.Open();
                    string query = "SELECT id, title, imagepath, executablepath, genres FROM game";
                    using(OracleCommand command = new OracleCommand(query, connection))
                    {
                        var reader = command.ExecuteReader();
                        {
                            while(reader.Read())
                            {
                                
                                string id = reader.GetString(0);
                                string title = reader.GetString(1);
                                string? imagePath = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                string executablePath = reader.GetString(3);

                                //Parse genres from string to List<Genre>
                                List<Genre> genresList = new List<Genre>();
                                string genresFromDb = reader.GetString(4);
                                string[] genresArray = genresFromDb.Split(',');
                                foreach(var genreString in genresArray)
                                {
                                    Genre genre = new Genre(genreString);
                                    genresList.Add(genre);
                                }
                                Game game = new Game(new Guid(id), title, imagePath, executablePath, genresList);
                                games.Add(game);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return games;
        }

        public override bool UpdateGame(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
