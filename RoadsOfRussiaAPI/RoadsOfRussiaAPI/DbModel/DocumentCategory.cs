using System.ComponentModel.DataAnnotations;

namespace RoadsOfRussiaAPI.DbModel
{
    public class DocumentCategory
    {
        [Key]
        public int IDDocumentCategory { get; set; }
        public string Title { get; set; }
    }
}
