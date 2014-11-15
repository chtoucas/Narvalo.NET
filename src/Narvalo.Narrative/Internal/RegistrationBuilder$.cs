// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Internal
{
    using System;
    using Autofac.Builder;
    using Autofac.Extras.DynamicProxy2;
    using Narvalo;
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
            DebugCheck.NotNull(@this);

            if (!IsDebuggingEnabled) {
                return @this;
            }

            return @this.EnableInterfaceInterceptors()
                .InterceptedBy(interceptorType);
        }
    }
}
