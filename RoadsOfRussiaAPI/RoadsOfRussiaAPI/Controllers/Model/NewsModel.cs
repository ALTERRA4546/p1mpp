namespace RoadsOfRussiaAPI.Controllers.Model
{
    public class NewsModel
    {
        public int IDNews { get; set; }
        public byte[] Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PositiveVote { get; set; }
        public int NegativeVote { get; set; }
        public DateTime Date { get; set; }
    }
}
