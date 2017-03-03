// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;

    public abstract partial class NonQueryStoredProcedure<TParameters>
    {
        protected NonQueryStoredProcedure(string connectionString, string name)
        {
            Require.NotNullOrEmpty(connectionString, nameof(connectionString));
            Require.NotNullOrEmpty(name, nameof(name));

            ConnectionString = connectionString;
            Name = name;
        }

        protected string ConnectionString { get; }

        protected string Name { get; }

        public int Execute(TParameters values)
        {
            int retval;

            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = CreateCommand(connection))
                {
                    var parameters = command.Parameters;

                    AddParameters(parameters, values);

                    connection.Open();
                    retval = command.ExecuteNonQuery();
                }
            }

            return retval;
        }

        protected abstract void AddParameters(SqlParameterCollection parameters, TParameters values);

        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities",
            Justification = "[Intentionally] The Code Analysis error is real, but we expect the consumer of this class to use a named SQL procedure.")]
        private SqlCommand CreateCommand(SqlConnection connection)
        {
            Demand.NotNull(connection);

            SqlCommand tmpCmd = null;
            SqlCommand cmd = null;

            try
            {
                tmpCmd = new SqlCommand(Name, connection);
                tmpCmd.CommandType = CommandType.StoredProcedure;

                cmd = tmpCmd;
                tmpCmd = null;
            }
            finally
            {
                if (tmpCmd != null)
                {
                    tmpCmd.Dispose();
                }
            }

            return cmd;
        }
    }
}
