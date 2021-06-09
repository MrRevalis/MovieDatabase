using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase.Models
{
    public class BrowseItem
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public bool ToWatch { get; set; }
        public bool Watched { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
    }
}
