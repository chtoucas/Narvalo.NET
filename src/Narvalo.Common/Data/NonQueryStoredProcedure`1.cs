// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Internal;

    [ContractClass(typeof(NonQueryStoredProcedureContract<>))]
    public abstract class NonQueryStoredProcedure<TParameters>
    {
        private readonly string _connectionString;
        private readonly string _name;

        protected NonQueryStoredProcedure(string connectionString, string name)
        {
            Require.NotNullOrEmpty(connectionString, "connectionString");
            Require.NotNullOrEmpty(name, "name");

            _connectionString = connectionString;
            _name = name;
        }

        protected string ConnectionString
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return _connectionString;
            }
        }

        protected string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return _name;
            }
        }

        public int Execute(TParameters values)
        {
            Contract.Requires(values != null);

            int result;

            using (var connection = CreateConnection_())
            {
                using (var command = CreateCommand_(connection))
                {
                    AddParameters(command.Parameters.AssumeNotNull(), values);

                    connection.Open();
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }

        protected abstract void AddParameters(SqlParameterCollection parameters, TParameters values);

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "The Code Analysis error is real, but we expect the consumer of this class to use a named SQL procedure.")]
        private SqlCommand CreateCommand_(SqlConnection connection)
        {
            Contract.Requires(connection != null);
            Contract.Ensures(Contract.Result<SqlCommand>() != null);

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

        private SqlConnection CreateConnection_()
        {
            Contract.Ensures(Contract.Result<SqlConnection>() != null);

            return new SqlConnection(ConnectionString);
        }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "[CodeContracts] Object Invariants.")]
        private void ObjectInvariants()
        {
            Contract.Invariant(_connectionString != null);
            Contract.Invariant(_connectionString.Length != 0);
            Contract.Invariant(_name != null);
            Contract.Invariant(_name.Length != 0);
        }
    }

    [ContractClassFor(typeof(NonQueryStoredProcedure<>))]
    internal abstract class NonQueryStoredProcedureContract<TResult> : NonQueryStoredProcedure<TResult>
    {
        protected NonQueryStoredProcedureContract(string connectionString, string name)
            : base(connectionString, name) { }

        protected override void AddParameters(SqlParameterCollection parameters, TResult values)
        {
            Contract.Requires(parameters != null);
            Contract.Requires(values != null);
        }
    }
}
