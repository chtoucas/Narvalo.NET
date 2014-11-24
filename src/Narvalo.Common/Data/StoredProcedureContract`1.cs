// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(StoredProcedure<>))]
    abstract class StoredProcedureContract<TResult> : StoredProcedure<TResult>
    {
        protected StoredProcedureContract(string connectionString, string name)
            : base(connectionString, name) { }

        protected override TResult Execute(SqlDataReader reader)
        {
            Contract.Requires(reader != null);

            return default(TResult);
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            Contract.Requires(parameters != null);
        }
    }
}
