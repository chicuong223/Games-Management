using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Genre
    {
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Genre()
        { }

        public Genre(string name)
        {
            this.name = name;
        }
    }
}