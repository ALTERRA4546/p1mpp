using System;

namespace RoadsOfRussiaAPI.Controllers.Model
{
    public class CommentModel
    {
        public int id { get; set; }
        public int? document_id { get; set; }
        public string text { get; set; }
        public DateTime date_created { get; set; }
        public DateTime? date_updated { get; set; }
        public authorClass author { get; set; }
        public class authorClass
        {
            public string name { get; set; }
            public string position { get; set; }
        }
    }
}
