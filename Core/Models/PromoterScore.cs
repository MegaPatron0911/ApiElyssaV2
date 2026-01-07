namespace Core.Models
{
    public class PromoterScore
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public double Score { get; set; }
        public string Comment { get; set; }
    }
}
