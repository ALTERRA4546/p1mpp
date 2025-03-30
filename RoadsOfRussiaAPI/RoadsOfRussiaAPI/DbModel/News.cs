using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoadsOfRussiaAPI.DbModel
{
    public class News
    {
        [Key]
        public int IDNews { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PositiveVote { get; set; }
        public int NegativeVote { get; set; }
        public byte[] Image { get; set; }
        public DateTime Date { get; set; }
    }
}
