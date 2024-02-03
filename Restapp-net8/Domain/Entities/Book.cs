
namespace Domain.entities
{
    public class Book
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public int TotalPage { get; set; }
        public BookData BookData { get; set; }
        public List<BookPrice> Prices { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
