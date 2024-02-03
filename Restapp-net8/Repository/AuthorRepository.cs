using Domain.entities;
using Domain.Extensions;
using Domain.Interfaces;
using Domain.Manager;
using Repository.DbAccess;
using System.Xml.Linq;

namespace Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _db;

        public AuthorRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Author> GetAllAuthors(LogManager logManager)
        {
            List<Author> authors = null;
            try
            {
                logManager.WriteInformation($"Retrieve authors");
                authors = _db.Authors.ToList();
                logManager.WriteInformation($"Founded: {authors.Count} authors");
            }
            catch (Exception e)
            {
                logManager.WriteError($"Error GetAllAuthors. Details: {e.Serialize()}");
                throw e;
            }
            return authors;
        }

        public List<Author> GetAuthorByName(LogManager logManager, string name)
        {
            List<Author> authors = null;
            try
            {
                logManager.WriteInformation($"Retrieve author by name {name}");
                authors = _db.Authors.Where(a => a.Name.Equals(name)).ToList();
                logManager.WriteInformation($"Founded: {authors.Count} authors");
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
                logManager.WriteInformation($"Retrieve author by id {id}");
                author = _db.Authors.Find(id);
                if(author is null)
                {
                    throw new Exception($"Author not founded by id");
                }
                logManager.WriteInformation($"Author founded");
            }
            catch (Exception e)
            {
                logManager.WriteError($"Error GetAuthorById. Details: {e.Serialize()}");
                throw e;
            }
            return author;
        }

        public bool InsertAuthor(LogManager logManager, Author author)
        {
            bool retValue = false;
            try
            {
                logManager.WriteInformation($"Insert author {author.Serialize()}");
                _db.Authors.Add(author);
                _db.SaveChanges();
                logManager.WriteInformation($"Author insert correctly");
                retValue = true;
            }
            catch (Exception e)
            {
                logManager.WriteError($"Error InsertAuthor. Details: {e.Serialize()}");
                throw e;
            }
            return retValue;
        }
    }
}
