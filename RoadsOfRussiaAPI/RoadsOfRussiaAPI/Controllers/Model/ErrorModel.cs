namespace RoadsOfRussiaAPI.Controllers.Model
{
    public class ErrorModel
    {
        public long timestamp { get; set; }
        public string message { get; set; }
        public int errorCode { get; set; }
    }
}
