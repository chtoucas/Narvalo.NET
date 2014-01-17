using Castle.DynamicProxy;

namespace Narvalo.Castle
{
    using System;
    using System.Data.SqlClient;

    public class SqlExceptionInterceptor : IInterceptor
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
