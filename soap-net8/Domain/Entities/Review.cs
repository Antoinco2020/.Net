
namespace Domain.entities
{
    public class Review
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int Vote { get; set; }
        public string Description { get; set; }
    }
}
