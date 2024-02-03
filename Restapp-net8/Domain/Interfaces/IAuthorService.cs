using Domain.entities;
using Domain.Manager;

namespace Domain.Interfaces
{
    public interface IAuthorService
    {
        List<Author> GetAllAuthors(LogManager logManager);
        List<Author> GetAuthorByName(LogManager logManager, string name);
        Author GetAuthorById(LogManager logManager, long id);
        bool InsertAuthor(LogManager logManager, Author author);
    }
}
