// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    public abstract partial class StoredProcedure<TResult>
    {
        private CommandBehavior _commandBehavior
            = CommandBehavior.CloseConnection | CommandBehavior.SingleResult;

        protected StoredProcedure(string connectionString, string name)
        {
            Require.NotNullOrEmpty(connectionString, nameof(connectionString));
            Require.NotNullOrEmpty(name, nameof(name));

            ConnectionString = connectionString;
            Name = name;
        }

        protected CommandBehavior CommandBehavior
        {
            get { return _commandBehavior; }
            set { _commandBehavior = value; }
        }

        protected string ConnectionString { get; }

        protected string Name { get; }

        public TResult Execute()
        {
            TResult retval;

            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = CreateCommand(connection))
                {
                    var parameters = command.Parameters;

                    PrepareParameters(parameters);

                    connection.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior))
                    {
                        retval = Execute(reader);
                    }
                }
            }

            return retval;
        }

        protected abstract TResult Execute(SqlDataReader reader);

        protected abstract void PrepareParameters(SqlParameterCollection parameters);

        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities", Justification = "[Intentionally] The Code Analysis error is real, but we expect the consumer of this class to use a named SQL procedure.")]
        private SqlCommand CreateCommand(SqlConnection connection)
        {
            Debug.Assert(connection != null);

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
