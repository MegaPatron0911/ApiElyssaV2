namespace Core.Models
{
    public class PlanCompany
    {
        public long PlanCompanyId { get; set; }
        public Guid PlanTypeId { get; set; }
        public Guid CompanyId { get; set; }
        public int QuantityAdditionalUser { get; set; }
        public int QuantityProperties { get; set; }
        public DateTime PlanActivationDate { get; set; }
        public DateTime? PlanModificationDate { get; set; }
        public int PlanCutOff { get; set; }
        public bool PlanStatus { get; set; }
        public string? Notes { get; set; }

        public virtual Company? Company { get; set; }
        public virtual PlanType? PlanType { get; set; }
    }
}
