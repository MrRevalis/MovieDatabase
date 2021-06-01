using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase.Models.Cast
{
    public class Credits
    {
        public int id { get; set; }
        public List<Cast> cast { get; set; }
        public List<Crew> crew { get; set; }
    }
}
