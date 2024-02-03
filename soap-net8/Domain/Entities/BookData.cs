
namespace Domain.entities
{
    public class BookData
    {
        public string Isbn { get; set; }
        public string BookClassification { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int TotalSold { get; set; }
    }
}
