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
        public bool ToRealise { get; set; }
        public bool Realised { get; set; }
    }
}
