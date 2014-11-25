// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using Narvalo;

    [ContractClass(typeof(NonQueryStoredProcedureContract<>))]
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

        public int Execute(TParameters values)
        {
            if (values == null) {
                throw new ArgumentException(Strings_Common.NonQueryStoredProcedure_ValuesIsNull, "values");
            }

            Contract.EndContractBlock();

            int result;

            using (var connection = CreateConnection_()) {
                using (var command = CreateCommand_(connection)) {
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
        SqlCommand CreateCommand_(SqlConnection connection)
        {
            Enforce.NotNull(connection, "connection");
            Contract.Ensures(Contract.Result<SqlCommand>() != null);

            SqlCommand tmpCmd = null;
            SqlCommand cmd = null;

            try {
                tmpCmd = new SqlCommand(Name, connection);
                tmpCmd.CommandType = CommandType.StoredProcedure;

                cmd = tmpCmd;
                tmpCmd = null;
            }
            finally {
                if (tmpCmd != null) {
                    tmpCmd.Dispose();
                }
            }

            return cmd;
        }

        SqlConnection CreateConnection_()
        {
            Contract.Ensures(Contract.Result<SqlConnection>() != null);

            return new SqlConnection(ConnectionString);
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
