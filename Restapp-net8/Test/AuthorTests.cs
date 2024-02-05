using Domain.entities;
using Domain.Extensions;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Test
{
    [TestClass]
    public class AuthorTests
    {
        private const string BASE_URL = "http://localhost:5151/api/author/";

        [TestMethod]
        public void D_GetAllAuthor()
        {
            try
            {
                string urlPart = "all";
                string content = RestCall(BASE_URL, urlPart, null, null, Enums.HttpMethod.GET).Result;
                List<Author> authors = content.Deserialize<List<Author>>();
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
                string urlPart = "get";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("name", "Stephen");
                string content = RestCall(BASE_URL, urlPart, parameters, null, Enums.HttpMethod.GET).Result;
                List<Author> authors = content.Deserialize<List<Author>>();
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
                string urlPart = "getById";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("id", "1");
                string content = RestCall(BASE_URL, urlPart, parameters, null, Enums.HttpMethod.GET).Result;
                Author author = content.Deserialize<Author>();
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
                string content = RestCall(BASE_URL, urlPart, null, author.Serialize(), Enums.HttpMethod.POST).Result;
                author = content.Deserialize<Author>();
                Assert.IsTrue(author.Id > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        private async Task<string> RestCall(string baseUrl, string urlPart, Dictionary<string, string> parameters, string body, Test.Enums.HttpMethod method)
        {
            string content = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:55587/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = null;


                    if (parameters != null)
                    {
                        urlPart += $"?";
                        foreach (KeyValuePair<string, string> entry in parameters)
                        {
                            urlPart += $"{entry.Key}={entry.Value}&";
                        }
                        urlPart = urlPart.Substring(0, urlPart.Length - 1);
                    }


                    switch (method)
                    {
                        case Enums.HttpMethod.GET:
                            response = await client.GetAsync(urlPart);
                            break;
                        case Enums.HttpMethod.POST:
                            response = await client.PostAsJsonAsync(urlPart, body);
                            break;
                        case Enums.HttpMethod.PUT:
                            response = await client.PutAsJsonAsync(urlPart, body);
                            break;
                        case Enums.HttpMethod.DELETE:
                            response = await client.DeleteAsync(urlPart);
                            break;
                    }

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception("Internal server Error");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return content;
        }
    }
}