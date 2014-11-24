// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using Narvalo;

    public abstract class StoredProcedure<TResult>
    {
        readonly string _connectionString;
        readonly string _name;

        CommandBehavior _commandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleResult;

        protected StoredProcedure(string connectionString, string name)
        {
            Require.NotNullOrEmpty(connectionString, "connectionString");
            Require.NotNullOrEmpty(name, "name");

            _connectionString = connectionString;
            _name = name;
        }

        protected CommandBehavior CommandBehavior
        {
            get { return _commandBehavior; }
            set { _commandBehavior = value; }
        }

        protected string ConnectionString { get { return _connectionString; } }

        protected string Name { get { return _name; } }

        public TResult Execute()
        {
            TResult result;

            using (var connection = CreateConnection()) {
                using (var command = CreateCommand(connection)) {
                    PrepareCommand(command);

                    // FIXME: If connection is null???
                    connection.Open();

                    using (var reader = ExecuteCommand_(command)) {
                        result = Execute(reader);
                    }
                }
            }

            return result;
        }

        protected abstract TResult Execute(SqlDataReader reader);

        protected virtual void PrepareCommand(SqlCommand command) { }

        // REVIEW: Why virtual? Looks like a bad idea.
        protected virtual SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        // REVIEW: Why virtual? Looks like a bad idea. If not necessary, update ExecuteCommand_ to use Require.NotNull.
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "[REVIEW] False positive.")]
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "The Code Analysis error is real, but we expect the consumer of this class to use a named SQL procedure.")]
        protected virtual SqlCommand CreateCommand(SqlConnection connection)
        {
            return new SqlCommand(Name, connection) { CommandType = CommandType.StoredProcedure };
        }

        SqlDataReader ExecuteCommand_(SqlCommand command)
        {
            Require.NotNull(command, "command");

            return command.ExecuteReader(CommandBehavior);
        }

#if CONTRACTS_FULL
        [ContractInvariantMethod]
        void ObjectInvariants()
        {
            Contract.Invariant(_connectionString != null);
            Contract.Invariant(_connectionString.Length != 0);
            Contract.Invariant(_name != null);
            Contract.Invariant(_name.Length != 0);
        }
#endif
    }
}