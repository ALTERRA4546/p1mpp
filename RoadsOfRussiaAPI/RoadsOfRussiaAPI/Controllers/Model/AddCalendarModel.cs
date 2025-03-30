namespace RoadsOfRussiaAPI.Controllers.Model
{
    public class AddCalendarModel
    {
        public int IDEmployee { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
