using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class NuwwePropertyDistribution
    {
        public string CodPropertyDistribution { get; set; }
        public Guid? EnvironmentTypeId { get; set; }
        public string Name { get; set; }
        public Guid? CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [MaxLength(5)]
        public string TipoRango { get; set; }

        public virtual EnvironmentType EnvironmentType { get; set; }
        public virtual Company Company { get; set; }
    }
}
