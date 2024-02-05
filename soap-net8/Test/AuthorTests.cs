using System.Globalization;
using WsLibrary;

namespace Test
{
    [TestClass]
    public class AuthorTests
    {
        private ServiceClient client = new ServiceClient();

        [TestMethod]
        public void D_GetAllAuthor()
        {
            try
            {
                Author[] authors = client.GetAllAuthors();
                Assert.IsNotNull(authors);
            }
            catch (Exception  e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void C_GetAuthorByName()
        {
            try
            {
                Author[] authors = client.GetAuthorByName("Stephen");
                Assert.IsNotNull(authors);

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
        [TestMethod]
        public void B_GetAuthorById()
        {
            try
            {
                Author author = client.GetAuthorById(1);
                Assert.IsNotNull(author);

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void A_InsertAuthor()
        {
            try
            {
                string urlPart = "insert";
                Author author = new Author();
                author.Name = "Stephen";
                author.Surname = "King";
                author.DateOfBirth = DateTime.ParseExact("21/09/1947", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                author.Nationality = "USA";
                author = client.InsertAuthor(author);
                Assert.IsTrue(author.Id > 0);
                
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
        
    }
}