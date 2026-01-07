using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public partial class ItemDiagnostic
    {
        public ItemDiagnostic()
        {
            Photos = new HashSet<Photo>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ItemDiagnosticId { get; set; }

        public Guid? MaterialId { get; set; }
        public Guid ItemId { get; set; }
        public string? Description { get; set; }
        public int Amount { get; set; }

        /// <summary>
        /// Could be:
        ///1: Malo
        ///2: Regular
        ///3: Bueno
        ///4: Nuevo
        /// </summary>
        /// 
        public int? Rating { get; set; }
        public virtual Item Item { get; set; }
        public virtual Material? Material { get; set; }

        public Guid EnvironmentDiagnosticId { get; set; }
        public virtual EnvironmentDiagnostic EnvironmentDiagnostic { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public string? Subject { get; set; }
        public string? Anottation { get; set; }
        public bool IsANew { get; set; }
        /// <summary>
        /// Could be:
        /// 0: No es una novedad
        /// 1: Es una novedad
        /// </summary>
        public bool ToRepair { get; set; }

        public int? Order { get; set; }
        public ItemDiagnostic ItemDiagnosticParent { get; set; }
        public Guid? ItemDiagnosticIdSelfReference { get; set; }
        public string? ItemName { get; set; }
        public DateTime? LastUpdate { get; set; }
        public virtual ICollection<ItemDiagnostic> ItemDiagnostics { get; set; }
    }
}
