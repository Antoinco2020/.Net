
namespace Domain.entities
{
    public class Author
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Book> Books { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
