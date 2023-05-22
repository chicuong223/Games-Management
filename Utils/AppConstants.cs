using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class AppConstants
    {
        public static readonly string ResourceFolderName = "res";
        public static readonly string ImageFolderPath = ResourceFolderName + Path.PathSeparator + "images";
        public static readonly string GamesFileName = "games.xml";
        public static readonly string GenresFileName = "genres.xml";
        public static readonly string DefaultImageFileName = "default.jpg";
        public static readonly string FileSaveType = "file";
        public static readonly string DatabaseSaveType = "database";
        public static readonly string ConfigFileName = "config.xml";
    }
}