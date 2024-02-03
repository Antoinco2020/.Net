using Domain.entities;
using System.ServiceModel;

namespace soap_net8
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        List<Author> GetAllAuthors();
        
        [OperationContract]
        List<Author> GetAuthorByName(string name);
        [OperationContract]
        Author GetAuthorById(long id);

        [OperationContract]
        Author InsertAuthor(Author author);
    }
}
