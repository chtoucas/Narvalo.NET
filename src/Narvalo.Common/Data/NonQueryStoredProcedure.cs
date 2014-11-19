// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo;

    public abstract class NonQueryStoredProcedure<TParameters>
    {
        readonly string _connectionString;
        readonly string _name;

        protected NonQueryStoredProcedure(string connectionString, string name)
        {
            Require.NotNullOrEmpty(connectionString, "connectionString");
            Require.NotNullOrEmpty(name, "name");

            _connectionString = connectionString;
            _name = name;
        }

        protected string ConnectionString { get { return _connectionString; } }

        protected string Name { get { return _name; } }

        public int Execute(TParameters parameters)
        {
            int result;

            using (var connection = CreateConnection()) {
                using (var command = CreateCommand(connection)) {
                    PrepareCommand(command, parameters);

                    connection.Open();
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }

        protected virtual void PrepareCommand(SqlCommand command, TParameters parameters) { }

        protected virtual SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "REVIEW: False positive.")]
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "The Code Analysis error is real, but we expect the consumer of this class to use a named SQL procedure.")]
        protected virtual SqlCommand CreateCommand(SqlConnection connection)
        {
            return new SqlCommand(Name, connection) { CommandType = CommandType.StoredProcedure };
        }
    }
}
