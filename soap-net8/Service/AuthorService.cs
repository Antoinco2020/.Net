using Domain.entities;
using Domain.Extensions;
using Domain.Interfaces;
using Domain.Manager;

namespace Service
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public List<Author> GetAllAuthors(LogManager logManager)
        {
            List<Author> authors = null;
            try
            {
                authors = _authorRepository.GetAllAuthors(logManager);
            }
            catch (Exception e)
            {
                logManager.WriteError($"Error GetAllAuthors. Details: {e.Serialize()}");
                throw e;
            }
            return authors;
        }

        public List<Author> GetAuthorByName(LogManager logManager,string name)
        {
            List<Author> authors = null;
            try
            {
                authors = _authorRepository.GetAuthorByName(logManager, name);
            }
            catch (Exception e)
            {
                logManager.WriteError($"Error GetAuthorByName. Details: {e.Serialize()}");
                throw e;
            }
            return authors;
        }

        public Author GetAuthorById(LogManager logManager, long id)
        {
            Author author = null;
            try
            {
                author = _authorRepository.GetAuthorById(logManager, id);
            }
            catch (Exception e)
            {
                logManager.WriteError($"Error GetAuthorById. Details: {e.Serialize()}");
                throw e;
            }
            return author;
        }

        public Author InsertAuthor(LogManager logManager, Author author)
        {
            try
            {
                author = _authorRepository.InsertAuthor(logManager, author);
            }
            catch (Exception e)
            {
                logManager.WriteError($"Error InsertAuthor. Details: {e.Serialize()}");
                throw e;
            }
            return author;
        }
    }
}
