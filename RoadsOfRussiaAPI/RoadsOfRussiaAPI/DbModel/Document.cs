using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RoadsOfRussiaAPI.DbModel
{
    public class Document
    {
        [Key]
        public int IDDocument { get; set; }
        public string Title { get; set; }
        public DateTime DateCreate { get; set; }
        [MaybeNull]
        public DateTime? DateUpdate { get; set; }
        [ForeignKey("DocumentCategory")]
        [Column("IDDocumentCategory")]
        [MaybeNull]
        public int IDDocumentCategory { get; set; }
    }
}
