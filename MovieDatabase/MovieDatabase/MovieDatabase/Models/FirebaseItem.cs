using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase.Models
{
    public class FirebaseItem
    {
        public string Owner { get; set; }
        public string Type { get; set; }
        public string ID { get; set; }
        public bool ToWatch { get; set; }
        public bool Watched { get; set; }
    }
}
