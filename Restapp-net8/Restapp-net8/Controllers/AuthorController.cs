using Domain.entities;
using Domain.Extensions;
using Domain.Interfaces;
using Domain.Manager;
using Microsoft.AspNetCore.Mvc;
using static Domain.Manager.Enums;

namespace Restapp_net8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
            ResourceExt.resFileManager = ResourceFileManager.Instance;
            ResourceExt.resFileManager.SetResources();
            CryptExt.rijndael = new Rijndael();
        }

        /// <summary>
        /// GetAllAuthors
        /// </summary>
        /// <returns>List of authors</returns>
        /// <response code="200">return authors</response>
        /// <response code="400">An error occurred</response>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllAuthors()
        {
            string logFullPath = PathExt.CombinePath("LogPath".GetConfig(), $"{Guid.NewGuid().ToString()}.log");
            LogManager logManager = LogManager.GetInstance(enumLogType.SERILOG, enumLogOutputType.FILE,
                Enums.GetValueFromDescription<enumLogLevel>("LogLevel".GetConfig()), logFullPath);
            try
            {
                List<Author> authors = _authorService.GetAllAuthors(logManager);
                if(authors.Count == 0)
                {
                    return NotFound();
                }

                return Ok(authors);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get Authors by name
        /// </summary>
        /// <param name="name">name of author</param>
        /// <returns>List of authors</returns>
        /// <response code="200">return authors</response>
        /// <response code="400">An error occurred</response>
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAuthorsByName(string name)
        {
            string logFullPath = PathExt.CombinePath("LogPath".GetConfig(), $"{Guid.NewGuid().ToString()}.log");
            LogManager logManager = LogManager.GetInstance(enumLogType.SERILOG, enumLogOutputType.FILE,
                Enums.GetValueFromDescription<enumLogLevel>("LogLevel".GetConfig()), logFullPath);
            try
            {
                List<Author> authors = _authorService.GetAuthorByName(logManager, name);
                if (authors.Count == 0)
                {
                    return NotFound();
                }

                return Ok(authors);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get Author by id
        /// </summary>
        /// <param name="id">id of author</param>
        /// <returns>author</returns>
        /// <response code="200">return author</response>
        /// <response code="400">An error occurred</response>
        [HttpGet]
        [Route("getById")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAuthorById(long id)
        {
            string logFullPath = PathExt.CombinePath("LogPath".GetConfig(), $"{Guid.NewGuid().ToString()}.log");
            LogManager logManager = LogManager.GetInstance(enumLogType.SERILOG, enumLogOutputType.FILE,
                Enums.GetValueFromDescription<enumLogLevel>("LogLevel".GetConfig()), logFullPath);
            try
            {
                Author author = _authorService.GetAuthorById(logManager, id);
                if (author is null)
                {
                    return NotFound();
                }

                return Ok(author);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Insert new Author
        /// </summary>
        /// <param name="author">author to insert</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Author
        ///     {
        ///        "Id": 0,
        ///        "Name": "Stephen",
        ///        "Surname": "King",
        ///        "Nationality": "USA",
        ///        "DateOfBirth": 1947-09-21 00:00:00
        ///     }
        ///
        /// </remarks>
        /// <response code="201">author created</response>
        /// <response code="400">An error occurred</response>
        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult InsertAuthor(Author author)
        {
            string logFullPath = PathExt.CombinePath("LogPath".GetConfig(), $"{Guid.NewGuid().ToString()}.log");
            LogManager logManager = LogManager.GetInstance(enumLogType.SERILOG, enumLogOutputType.FILE,
                Enums.GetValueFromDescription<enumLogLevel>("LogLevel".GetConfig()), logFullPath);
            try
            {
                if (!_authorService.InsertAuthor(logManager, author))
                {
                    return BadRequest();
                }

                return Created();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
