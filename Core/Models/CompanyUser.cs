namespace Core.Models
{
    public class CompanyUser
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string Tin { get; set; }
        public string Address { get; set; }

        public string Email { get; set; }
    }
}
