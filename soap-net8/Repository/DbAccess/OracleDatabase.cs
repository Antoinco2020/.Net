using Domain.Extensions;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Data;
using System.Text;

namespace Repository.DbAccess
{
    /// <summary>
    /// Class that encapsulates a Oracle Database connections 
    /// and CRUD operations
    /// </summary>
    public class OracleDatabase : IDisposable
    {
        private OracleConnection _connection;

        /// <summary>
        /// Default constructor which uses the "DefaultConnection" connectionString
        /// </summary>
        public OracleDatabase()
            : this("ConnectionString")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string name
        /// </summary>
        /// <param name="connectionStringName"></param>
        public OracleDatabase(string connectionString)
        {
            _connection = new OracleConnection(connectionString.IsNullOrEmpty() ? "OracleConnection".Decrypt() : connectionString.Decrypt());
        }

        /// <summary>
        /// Executes a non-query Oracle statement
        /// </summary>
        /// <param name="commandText">The Oracle query to execute</param>
        /// <param name="parameters">Optional parameters to pass to the query</param>
        /// <returns>The count of records affected by the Oracle statement</returns>
        public int Execute(string commandText, IEnumerable parameters)
        {
            int result = 0;

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();

                using (OracleTransaction transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var command = CreateCommand(commandText, parameters);

                    command.Transaction = transaction;

                    try
                    {
                        result = command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            finally
            {
                _connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Executes a Oracle query that returns a single scalar value as the result.
        /// </summary>
        /// <param name="commandText">The Oracle query to execute</param>
        /// <param name="parameters">Optional parameters to pass to the query</param>
        /// <returns></returns>
        public object QueryValue(string commandText, IEnumerable parameters)
        {
            object result;

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                result = command.ExecuteScalar();
            }
            finally
            {
                EnsureConnectionClosed();
            }

            return result;
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        /// <summary>
        /// Executes a SQL query that returns a list of rows as the result.
        /// </summary>
        /// <param name="commandText">The Oracle query to execute</param>
        /// <param name="parameters">Parameters to pass to the Oracle query</param>
        /// <returns>A list of a Dictionary of Key, values pairs representing the 
        /// ColumnName and corresponding value</returns>
        public List<Dictionary<string, string>> Query(string commandText, IEnumerable parameters)
        {
            List<Dictionary<string, string>> rows;
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                using (var reader = command.ExecuteReader())
                {
                    rows = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            string columnValue = "";
                            //System.Text.Encoding.Default.GetString((Byte[])reader.GetValue(i));
                            if (reader.GetValue(i).GetType().Name == "Byte[]")
                            {
                                columnValue = reader.IsDBNull(i) ? null : ByteArrayToString((Byte[])reader.GetValue(i));
                            }
                            else
                            {
                                columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();
                            }
                            row.Add(columnName, columnValue);
                        }
                        rows.Add(row);
                    }
                }
            }
            finally
            {
                EnsureConnectionClosed();
            }

            return rows;
        }

        /// <summary>
        /// Executes a SQL query that returns a list of rows as the result.
        /// </summary>
        /// <param name="commandText">The Oracle query to execute</param>
        /// <param name="parameters">Parameters to pass to the Oracle query</param>
        /// <returns>A list of a Dictionary of Key, values pairs representing the 
        /// ColumnName and corresponding value</returns>
        public List<Dictionary<string, string>> QueryWithoutCloseConnection(string commandText, IEnumerable parameters)
        {
            List<Dictionary<string, string>> rows;
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                using (var reader = command.ExecuteReader())
                {
                    rows = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            string columnValue = "";
                            //System.Text.Encoding.Default.GetString((Byte[])reader.GetValue(i));
                            if (reader.GetValue(i).GetType().Name == "Byte[]")
                            {
                                columnValue = reader.IsDBNull(i) ? null : ByteArrayToString((Byte[])reader.GetValue(i));
                            }
                            else
                            {
                                columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();
                            }
                            row.Add(columnName, columnValue);
                        }
                        rows.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return rows;
        }

        /// <summary>
        /// Opens a connection if not open
        /// </summary>
        private void EnsureConnectionOpen()
        {
            var retries = 3;
            if (_connection.State == ConnectionState.Open)
            {
                return;
            }
            while (retries >= 0 && _connection.State != ConnectionState.Open)
            {
                _connection.Open();
                retries--;
                Thread.Sleep(30);
            }
        }

        /// <summary>
        /// Closes a connection if open
        /// </summary>
        private void EnsureConnectionClosed()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Creates a OracleCommand with the given parameters
        /// </summary>
        /// <param name="commandText">The Oracle query to execute</param>
        /// <param name="parameters">Parameters to pass to the Oracle query</param>
        /// <returns></returns>
        private OracleCommand CreateCommand(string commandText, IEnumerable parameters)
        {
            var command = _connection.CreateCommand();
            command.BindByName = true;
            command.CommandText = commandText;
            AddParameters(command, parameters);

            return command;
        }

        /// <summary>
        /// Adds the parameters to a Oracle command
        /// </summary>
        /// <param name="command">The Oracle query to execute</param>
        /// <param name="parameters">Parameters to pass to the Oracle query</param>
        private static void AddParameters(OracleCommand command, IEnumerable parameters)
        {
            if (parameters == null) return;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// Helper method to return query a string value 
        /// </summary>
        /// <param name="commandText">The Oracle query to execute</param>
        /// <param name="parameters">Parameters to pass to the Oracle query</param>
        /// <returns>The string value resulting from the query</returns>
        public string GetStrValue(string commandText, IEnumerable parameters)
        {
            var value = QueryValue(commandText, parameters) as string;
            return value;
        }

        public bool ExecuteProc(string ProcedureName, List<string> InputParameterName, List<string> InputParameterValue, List<string> InputParameteOracleType, List<string> OutputParameterName, List<string> OutputParameterValue, List<string> OutputParameteOracleType, out List<string> ResultsOutputValue)
        {
            bool result = true;

            if (string.IsNullOrEmpty(ProcedureName))
            {
                throw new ArgumentException("Procedure name cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();

                OracleCommand cmd;

                cmd = new OracleCommand(ProcedureName, _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // ************************************************************************
                //              PARAMETRI  IN 
                // ************************************************************************
                for (int i = 0; i < InputParameterName.Count; i++)
                {
                    cmd.Parameters.Add(InputParameterName[i], WrapperOracleType(InputParameteOracleType[i])).Value = InputParameterValue[i];
                }

                // ************************************************************************
                //              PARAMETRI  OUT 
                // ************************************************************************
                for (int i = 0; i < OutputParameterName.Count; i++)
                {
                    if (OutputParameteOracleType[i].ToString() == "stringa")
                    {
                        // imposta 200 caratteri:
                        cmd.Parameters.Add(OutputParameterName[i].ToString(), WrapperOracleType(OutputParameteOracleType[i]), 200);
                    }
                    else
                    {
                        cmd.Parameters.Add(OutputParameterName[i].ToString(), WrapperOracleType(OutputParameteOracleType[i]));
                    }

                    cmd.Parameters[OutputParameterName[i]].Direction = ParameterDirection.Output;
                }

                int intRetProc = cmd.ExecuteNonQuery();
                // stored procedures return always -1 

                List<string> ValoriOutProc = new List<string>();
                for (int i = 0; i < OutputParameterName.Count; i++)
                {
                    var output = cmd.Parameters[OutputParameterName[i]].Value;
                    var type = output.GetType();
                    Oracle.ManagedDataAccess.Types.OracleClob oracleClob = null;
                    if (type.Name.ToUpper().Equals("ORACLECLOB"))
                    {
                        oracleClob = (Oracle.ManagedDataAccess.Types.OracleClob)output;
                        ValoriOutProc.Add(oracleClob.Value);
                    }
                    else
                    {
                        ValoriOutProc.Add(cmd.Parameters[OutputParameterName[i]].Value.ToString());
                    }



                }

                ResultsOutputValue = ValoriOutProc;

                result = true;
            }
            catch (Exception ex)
            {
                ResultsOutputValue = new List<string>();
                ResultsOutputValue.Add(ex.Message);
                result = false;
            }
            finally
            {
                _connection.Close();
            }

            return result;
        }

        private OracleDbType WrapperOracleType(string tipoDato)
        {
            OracleDbType oracleType;
            switch (tipoDato.ToLower())
            {
                case "stringa":
                    oracleType = OracleDbType.Varchar2;
                    break;
                case "intero":
                    oracleType = OracleDbType.Int32;
                    break;
                case "numerico":
                    oracleType = OracleDbType.Long;
                    break;
                case "data":
                    oracleType = OracleDbType.Date;
                    break;
                case "clob":
                    oracleType = OracleDbType.Clob;
                    break;

                default:
                    oracleType = OracleDbType.Varchar2;
                    break;
            }
            return oracleType;
        }

        public void Dispose()
        {
            if (_connection == null) return;

            _connection.Dispose();
            _connection = null;
        }
    }
}
