﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Reflection;
    using System.Web;
    using Autofac;
    using Autofac.Builder;
    using Autofac.Features.Scanning;

    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterHandlers(this ContainerBuilder @this, params Assembly[] handlerAssemblies)
        {
            Require.Object(@this);

            return @this.RegisterAssemblyTypes(handlerAssemblies)
                .Where(_ => typeof(IHttpHandler).IsAssignableFrom(_)
                    && _.Name.EndsWith("Handler", StringComparison.Ordinal));
        }
    }
}
