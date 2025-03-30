using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RoadsOfRussiaAPI.DbModel
{
    public class Division
    {
        [Key]
        public int IDDivision { get; set; }
        [ForeignKey("Division")]
        [Column("IDMainDivision")]
        [MaybeNull]
        public int? IDMainDivision { get; set; }
        public string Title { get; set; }
        [MaybeNull]
        public string Description { get; set; }
        [ForeignKey("Employee")]
        [Column("IDDirector")]
        [MaybeNull]
        public int? IDDirector { get; set; }
        [MaybeNull]
        public virtual ICollection<Division> Division1 { get; set; }
    }
}
