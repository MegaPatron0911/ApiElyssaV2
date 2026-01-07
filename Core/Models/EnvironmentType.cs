namespace Core.Models
{
    public partial class EnvironmentType
    {
        public EnvironmentType()
        {
            Environments = new HashSet<Environment>();
            DefaultItems = new HashSet<DefaultItem>();
            PropertyEnvironmentDefaults = new HashSet<PropertyEnvironmentDefault>();
        }

        public Guid EnvironmentTypeId { get; set; }
        public string EnvironmentTypeName { get; set; }

        public virtual ICollection<Environment> Environments { get; set; }

        public virtual ICollection<DefaultItem> DefaultItems { get; set; }

        public virtual ICollection<NuwwePropertyDistribution> NuwwePropertyDistributions { get; set; }

        public virtual ICollection<PropertyEnvironmentDefault> PropertyEnvironmentDefaults { get; set; }
    }
}
