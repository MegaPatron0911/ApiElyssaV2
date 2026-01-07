namespace Core.Models
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public long EstateAgentInCompanyId { get; set; }
        public string IpAddress { get; set; }
        public string DeviceInfo { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual EstateAgentInCompany EstateAgentInCompany { get; set; }

        private DateTime _updateAt;
        public DateTime UpdateAt
        {
            get => DateTime.SpecifyKind(_updateAt, DateTimeKind.Utc);
            set => _updateAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => DateTime.SpecifyKind(_createdAt, DateTimeKind.Utc);
            set => _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private DateTime _expiresAt;
        public DateTime ExpiresAt
        {
            get => DateTime.SpecifyKind(_expiresAt, DateTimeKind.Utc);
            set => _expiresAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
}
