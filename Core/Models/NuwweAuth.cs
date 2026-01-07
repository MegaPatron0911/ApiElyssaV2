namespace Core.Models
{
    public class NuwweAuth
    {
        public int Id { get; set; }
        public Guid CompanyId { get; set; }       
        public string Token { get; set; }           
        public string? RefreshToken { get; set; }    
        public string? DomainUrl { get; set; }
        public string NameIdNuwwe { get; set; }     
        public DateTime CreatedAt { get; set; }     
        public DateTime? UpdatedAt { get; set; }

        public virtual Company Company { get; set; }
    }
}
