using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Game
    {
        public Game()
        {
        }

        public Game(Guid id, string title, string? imagePath, string executablePath, List<Genre> genres)
        {
            Id = id;
            Title = title;
            ImagePath = imagePath;
            ExecutablePath = executablePath;
            Genres = genres;
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string? ImagePath { get; set; } = "";
        public string ExecutablePath { get; set; } = "";
        public List<Genre> Genres { get; set; } = new List<Genre>();
    }
}