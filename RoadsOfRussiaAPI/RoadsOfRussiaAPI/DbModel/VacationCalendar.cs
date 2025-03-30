using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RoadsOfRussiaAPI.DbModel
{
    public class VacationCalendar
    {
        [Key]
        public int IDVacationCalendar { get; set; }
        public string Title { get; set; }
        [MaybeNull]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [ForeignKey("Employee")]
        [Column("IDEmployee")]
        [MaybeNull]
        public int IDEmployee { get; set; }
    }
}
