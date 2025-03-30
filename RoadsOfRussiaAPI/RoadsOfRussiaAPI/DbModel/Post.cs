using System.ComponentModel.DataAnnotations;

namespace RoadsOfRussiaAPI.DbModel
{
    public class Post
    {
        [Key]
        public int IDPost { get; set; }
        public string Title { get; set; }
    }
}
