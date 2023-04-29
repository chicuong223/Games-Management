using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class FileUtils
    {
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}