// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;
    using Castle.DynamicProxy;
    using Serilog;

    public sealed class WeavingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Require.NotNull(invocation, "invocation");

            var file = invocation.Arguments[0] as FileInfo;

            Log.Information("Weaving {Name}", file.FullName);

            invocation.Proceed();
        }
    }
}
