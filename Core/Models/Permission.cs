using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Code { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
