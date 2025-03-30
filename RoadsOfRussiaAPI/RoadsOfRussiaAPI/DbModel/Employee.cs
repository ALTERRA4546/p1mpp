using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RoadsOfRussiaAPI.DbModel
{
    public class Employee
    {
        [Key]
        public int IDEmployee { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronimyc { get; set; }
        [ForeignKey("Post")]
        [Column("IDPost")]
        [MaybeNull]
        public int? IDPost { get; set; }
        [MaybeNull]
        public int? IDDivision { get; set; }
        [MaybeNull]
        public string PersonalPhone { get; set; }
        public string CorpPhone { get; set; }
        public string Email { get; set; }
        [ForeignKey("Employee")]
        [Column("IDDirector")]
        [MaybeNull]
        public int? IDDirector { get; set; }
        [ForeignKey("Employee")]
        [Column("IDAssistent")]
        [MaybeNull]
        public int? IDAssistent { get; set; }
        [MaybeNull]
        public DateTime? DateOfBirth { get; set; }
        public string Cabinet { get; set; }
        [MaybeNull]
        public string OtherInformaion { get; set; }
        [MaybeNull]
        public DateTime? DateOfFired { get; set; }
    }
}
