using System;
using System.Collections.Generic;
using System.Text;

namespace RoadsOfRussiaMobile.Model
{
    internal class EventsModel
    {
        public int IDEvent { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime RawDate { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
    }
}
