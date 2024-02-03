using Domain.entities;
using Domain.Manager;

namespace Domain.Interfaces
{
    public interface IAuthorRepository
    {
        List<Author> GetAllAuthors(LogManager logManager);
        List<Author> GetAuthorByName(LogManager logManager, string name);
        Author GetAuthorById(LogManager logManager, long id);
        Author InsertAuthor(LogManager logManager, Author author);
    }
}
