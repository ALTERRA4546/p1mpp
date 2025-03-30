using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RoadsOfRussiaAPI.DbModel
{
    public class Event
    {
        [Key]
        public int IDEvent { get; set; }
        public string Title { get; set; }
        [MaybeNull]
        public int? IDEventType { get; set; }
        [MaybeNull]
        public int? IDEventStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [ForeignKey("Employee")]
        [Column("IDResponsible")]
        [MaybeNull]
        public int? IDResponsible { get; set; }
        [MaybeNull]
        public string Description { get; set; }
    }
}
