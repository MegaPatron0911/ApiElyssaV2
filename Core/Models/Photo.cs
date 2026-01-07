using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public partial class Photo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PhotoId { get; set; }
        public string? StorageReference { get; set; }
        public string? StorageUrlReference { get; set; }
        public Guid ItemDiagnosticId { get; set; }
        public DateTime? CreationDate { get; set; }
        public long? Size { get; set; }

        public virtual ItemDiagnostic ItemDiagnostic { get; set; }
    }
}
