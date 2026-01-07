using Core.Enums;

namespace Core.Models
{
    public partial class Inventory
    {
        public Inventory()
        {
            EnvironmentDiagnostics = new HashSet<EnvironmentDiagnostic>();
            CustomerSuccessRatings = new HashSet<CustomerSuccessRating>();
        }

        public Guid InventoryId { get; set; }
        public Guid PropertyId { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? StakeHolderSignatureId { get; set; }
        public Guid? OwnerSignatureId { get; set; }

        /// <summary>
        /// Could be:
        /// 1: Captación
        /// 2: Entrega
        /// </summary>
        public DateTime? AgentSignatureDate { get; set; }
        public bool IsRemoteSigned { get; set; }
        public decimal RentalPrice { get; set; }
        public string? Currency { get; set; }
        public InventoryType InventoryType { get; set; }
        public long EstateAgentInCompanyId { get; set; }
        public bool IsSigned { get; set; }
        public string? PfdUrl { get; set; }
        public string? Approval_Code { get; set; }
        public DateTime? SignatureDate { get; set; }
        public DateTime? OwnerSignatureDate { get; set; }
        public string? PdfUrlRef { get; set; }
        public bool IsActive { get; set; }

        public virtual StakeHolderSignature? StakeHolderSignature { get; set; }
        public virtual EstateAgentInCompany EstateAgentInCompany { get; set; }
        public virtual Property Property { get; set; } = null!;
        public virtual ICollection<EnvironmentDiagnostic> EnvironmentDiagnostics { get; set; }
        public virtual OwnerSignature? OwnerSignature { get; set; }
        public virtual ICollection<CustomerSuccessRating> CustomerSuccessRatings { get; set; }
    }
}
