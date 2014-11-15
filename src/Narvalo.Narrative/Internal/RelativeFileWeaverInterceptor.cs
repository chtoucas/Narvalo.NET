// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Internal
{
    using Castle.DynamicProxy;
    using Narvalo;
    using Narvalo.IO;
    using Serilog;

    internal sealed class RelativeFileWeaverInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            DebugCheck.NotNull(invocation);

            var info = invocation.Arguments[0] as RelativeFile;

            Log.Debug("Processing {Name}", info.RelativeName);

            invocation.Proceed();
        }
    }
}
