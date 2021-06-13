using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase.Models.ActorMovies
{
    public class ActorMovies
    {
        public List<Cast> cast { get; set; }
        public List<Crew> crew { get; set; }
        public int id { get; set; }
    }
}
