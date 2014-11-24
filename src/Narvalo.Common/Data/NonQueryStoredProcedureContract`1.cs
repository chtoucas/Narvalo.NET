// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Data
{
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(NonQueryStoredProcedure<>))]
    abstract class NonQueryStoredProcedureContract<TResult> : NonQueryStoredProcedure<TResult>
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
