using Castle.DynamicProxy;

namespace Narvalo.Castle.DynamicProxy
{
    using System;
    using System.Data.SqlClient;

    public class SqlExceptionInterceptor : IInterceptor
    {
        readonly Action<SqlException> _onException;

        public SqlExceptionInterceptor(Action<SqlException> onException)
        {
            Requires.NotNull(onException, "onException");

            _onException = onException;
        }

        #region IInterceptor

        public void Intercept(IInvocation invocation)
        {
            Requires.NotNull(invocation, "invocation");

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
