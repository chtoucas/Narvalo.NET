// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Castle
{
    using System;
    using System.Data.SqlClient;
    using global::Castle.DynamicProxy;

    public sealed class SqlExceptionInterceptor : IInterceptor
    {
        readonly Action<SqlException> _onException;

        public SqlExceptionInterceptor(Action<SqlException> onException)
        {
            Require.NotNull(onException, "onException");

            _onException = onException;
        }

        #region IInterceptor

        public void Intercept(IInvocation invocation)
        {
            Require.NotNull(invocation, "invocation");

            try {
                invocation.Proceed();
            }
            catch (SqlException ex) {
                _onException(ex);
                throw;
            }
        }

        #endregion
    }
}
