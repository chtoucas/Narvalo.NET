// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Externs.Castle
{
    using System;
    using System.Xml;

    using global::Castle.DynamicProxy;

    public sealed class XmlExceptionInterceptor : IInterceptor
    {
        private readonly Action<XmlException> _onException;

        public XmlExceptionInterceptor(Action<XmlException> onException)
        {
            Require.NotNull(onException, "onException");

            _onException = onException;
        }

        public void Intercept(IInvocation invocation)
        {
            Require.NotNull(invocation, "invocation");

            try
            {
                invocation.Proceed();
            }
            catch (XmlException ex)
            {
                _onException(ex);
                throw;
            }
        }
    }
}
