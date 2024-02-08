using Domain.entities;
using Domain.Extensions;
using Domain.Interfaces;
using Domain.Manager;
using static Domain.Manager.Enums;

namespace soap_net8
{
    public class Service : IService
    {
        private readonly IAuthorService _authorService;
        public Service(IAuthorService authorService)
        {
            _authorService = authorService;
            ResourceExt.resFileManager = ResourceFileManager.Instance;
            ResourceExt.resFileManager.SetResources();
            CryptExt.rijndael = new Rijndael();
        }

        public List<Author> GetAllAuthors()
        {
            List<Author> authors = null;
            string logFullPath = PathExt.CombinePath("LogPath".GetConfig(), $"{Guid.NewGuid().ToString()}.log");
            LogManager logManager = LogManager.GetInstance(enumLogType.SERILOG, enumLogOutputType.FILE,
                Enums.GetValueFromDescription<enumLogLevel>("LogLevel".GetConfig()), logFullPath);
            try
            {
                logManager.WriteInformation($"Start method GetAllAuthors");
                authors = _authorService.GetAllAuthors(logManager);
            }
            catch (Exception e)
            {
                logManager.WriteError($"Error GetAllAuthors. Details: {e.Serialize()}");
                throw e;
            }
            return authors;
        }

        public Author GetAuthorById(long id)
        {
            Author author = null;
            string logFullPath = PathExt.CombinePath("LogPath".GetConfig(), $"{Guid.NewGuid().ToString()}.log");
            LogManager logManager = LogManager.GetInstance(enumLogType.SERILOG, enumLogOutputType.FILE,
                Enums.GetValueFromDescription<enumLogLevel>("LogLevel".GetConfig()), logFullPath);
            try
            {
                logManager.WriteInformation($"Start method GetAuthorById");
                author = _authorService.GetAuthorById(logManager, id);
            }
            catch (Exception e)
            {
                logManager.WriteError($"Error GetAuthorById. Details: {e.Serialize()}");
                throw e;
            }
            return author;
        }

        public List<Author> GetAuthorByName(string name)
        {
            List<Author> authors = null;
            string logFullPath = PathExt.CombinePath("LogPath".GetConfig(), $"{Guid.NewGuid().ToString()}.log");
            LogManager logManager = LogManager.GetInstance(enumLogType.SERILOG, enumLogOutputType.FILE,
                Enums.GetValueFromDescription<enumLogLevel>("LogLevel".GetConfig()), logFullPath);
            try
            {
                logManager.WriteInformation($"Start method GetAuthorByName");
                authors = _authorService.GetAuthorByName(logManager, name);
            }
            catch (Exception e)
            {
                logManager.WriteError($"Error GetAuthorByName. Details: {e.Serialize()}");
                throw e;
            }
            return authors;
        }

        public Author InsertAuthor(Author author)
        {
            string logFullPath = PathExt.CombinePath("LogPath".GetConfig(), $"{Guid.NewGuid().ToString()}.log");
            LogManager logManager = LogManager.GetInstance(enumLogType.SERILOG, enumLogOutputType.FILE,
                Enums.GetValueFromDescription<enumLogLevel>("LogLevel".GetConfig()), logFullPath);
            try
            {
                logManager.WriteInformation($"Start method InsertAuthor");
                author = _authorService.InsertAuthor(logManager, author);
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
