
namespace Domain.entities
{
    public class BookPrice
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime PriceStartDate { get; set; }
        public DateTime PriceEndDate { get; set; }
    }
}
