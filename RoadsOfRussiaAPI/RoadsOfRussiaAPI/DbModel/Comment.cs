using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RoadsOfRussiaAPI.DbModel
{
    public class Comment
    {
        [Key]
        public int IDComment { get; set; }
        public string Text { get; set; }
        [ForeignKey("Employee")]
        [Column("IDAuthor")]
        [MaybeNull]
        public int? IDAuthor { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        [ForeignKey("Document")]
        [Column("IDDocument")]
        [MaybeNull]
        public int? IDDocument { get; set; }
    }
}
