// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace DocuMaker.Internal
{
    using System;
    using System.Diagnostics.Contracts;
    using Autofac.Builder;
    using Autofac.Extras.DynamicProxy2;
    using Serilog;
    using Serilog.Events;

    internal static class RegistrationExtensions
    {
        static bool IsDebuggingEnabled
        {
            get { return Log.IsEnabled(LogEventLevel.Debug); }
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle>
            InterfaceDebuggedBy<TLimit, TActivatorData, TSingleRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> @this,
            Type interceptorType)
        {
            Contract.Requires(@this != null);

            if (!IsDebuggingEnabled) {
                return @this;
            }

            return @this.EnableInterfaceInterceptors()
                .InterceptedBy(interceptorType);
        }
    }
}
