using Domain.entities;
using Domain.Extensions;
using Domain.Interfaces;
using Domain.Manager;
using Oracle.ManagedDataAccess.Client;
using Repository.DbAccess;

namespace Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        public List<Author> GetAllAuthors(LogManager logManager)
        {
            List<Author> authors = null;
            try
            {
                logManager.WriteInformation($"Execute query for retrieve authors");
                using (OracleDatabase oracleDatabase = new OracleDatabase())
                {
                    string sql = "QueryAllAuthors".GetConfig();
                    logManager.WriteInformation($"sql: {sql}");
                    List<Dictionary<string, string>> dictList = oracleDatabase.Query(sql, null);
                    logManager.WriteInformation($"Founded: {dictList.Count} authors");
                    if (dictList != null && dictList.Count > 0)
                    {
                        authors = new List<Author>();
                        foreach (Dictionary<string, string> dict in dictList)
                        {
                            Author author = new Author();
                            author.Id = dict["ID"].ToInt64();
                            author.Name = dict["NAME"];
                            author.Surname = dict["SURNAME"];
                            author.Nationality = dict["NATIONALITY"];
                            author.DateOfBirth = dict["DATE_OF_BIRTH"].Parse("dd/MM/yyyy");
                            authors.Add(author);
                        }
                    }
                }
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
                using (OracleDatabase oracleDatabase = new OracleDatabase())
                {
                    string sql = "QueryAuthorsByName".GetConfig();
                    logManager.WriteInformation($"sql: {sql}");

                    List<OracleParameter> oParameter = new List<OracleParameter>()
                    {
                        new OracleParameter { ParameterName = "name", Value = name, OracleDbType = OracleDbType.Varchar2 }
                    };

                    List<Dictionary<string, string>> dictList = oracleDatabase.Query(sql, oParameter);
                    logManager.WriteInformation($"Founded: {dictList.Count} authors");
                    if (dictList != null && dictList.Count > 0)
                    {
                        authors = new List<Author>();
                        foreach (Dictionary<string, string> dict in dictList)
                        {
                            Author author = new Author();
                            author.Id = dict["ID"].ToInt64();
                            author.Name = dict["NAME"];
                            author.Surname = dict["SURNAME"];
                            author.Nationality = dict["NATIONALITY"];
                            author.DateOfBirth = dict["DATE_OF_BIRTH"].Parse("dd/MM/yyyy");
                            authors.Add(author);
                        }
                    }
                }
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
                using (OracleDatabase oracleDatabase = new OracleDatabase())
                {
                    string sql = "QueryAuthorsById".GetConfig();
                    logManager.WriteInformation($"sql: {sql}");

                    List<OracleParameter> oParameter = new List<OracleParameter>()
                    {
                        new OracleParameter { ParameterName = "id", Value = id, OracleDbType = OracleDbType.Int64 }
                    };

                    List<Dictionary<string, string>> dictList = oracleDatabase.Query(sql, oParameter);
                    logManager.WriteInformation($"Founded: {dictList.Count} authors");
                    if (dictList != null && dictList.Count > 0)
                    {
                        foreach (Dictionary<string, string> dict in dictList)
                        {
                            author = new Author();
                            author.Id = dict["ID"].ToInt64();
                            author.Name = dict["NAME"];
                            author.Surname = dict["SURNAME"];
                            author.Nationality = dict["NATIONALITY"];
                            author.DateOfBirth = dict["DATE_OF_BIRTH"].Parse("dd/MM/yyyy");
                            break;
                        }
                    }
                }
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
                using (OracleDatabase oracleDatabase = new OracleDatabase())
                {
                    List<Dictionary<string, string>> dctSeq = oracleDatabase.Query("SeqAuthor".GetConfig(), null);

                    string sequenceId = string.Empty;

                    if (dctSeq != null & dctSeq.Count > 0)
                        sequenceId = dctSeq[0].Values.First();

                    logManager.WriteInformation($"sequenceId: {sequenceId}");

                    if (sequenceId.IsNullOrEmpty())
                    {
                        throw new Exception($"Error when call sequence from author");
                    }

                    string sql = "QueryInsertAuthor".GetConfig();
                    logManager.WriteInformation($"sql: {sql}");

                    List<OracleParameter> oParameter = new List<OracleParameter>()
                    {
                        new OracleParameter { ParameterName = "id", Value = sequenceId.ToInt64(), OracleDbType = OracleDbType.Int64 },
                        new OracleParameter { ParameterName = "name", Value = author.Name, OracleDbType = OracleDbType.Varchar2 },
                        new OracleParameter { ParameterName = "surname", Value = author.Surname, OracleDbType = OracleDbType.Varchar2 },
                        new OracleParameter { ParameterName = "nationality", Value = author.Nationality, OracleDbType = OracleDbType.Varchar2 },
                        new OracleParameter { ParameterName = "dateOfBirth", Value = author.DateOfBirth, OracleDbType = OracleDbType.Date },
                    };

                    int result = oracleDatabase.Execute(sql, oParameter);
                    logManager.WriteInformation($"Inserted {result} row");
                    if (result == 0)
                    {
                        throw new Exception($"Error when insert author in table");
                    }

                    author = GetAuthorById(logManager, sequenceId.ToInt64());
                }
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
