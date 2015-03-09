// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Externs.Autofac
{
    using System;
    using System.Reflection;
    using System.Web;

    using global::Autofac;
    using global::Autofac.Builder;
    using global::Autofac.Features.Scanning;
    using Narvalo.Mvp;

    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterHandlers(
                this ContainerBuilder @this,
                params Assembly[] handlerAssemblies)
        {
            Require.Object(@this);

            return @this.RegisterAssemblyTypes(handlerAssemblies)
                .Where(_ => typeof(IHttpHandler).IsAssignableFrom(_)
                    && _.Name.EndsWith("Handler", StringComparison.Ordinal));
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterPresenters(
                this ContainerBuilder @this,
                params Assembly[] presenterAssemblies)
        {
            Require.Object(@this);

            return @this.RegisterAssemblyTypes(presenterAssemblies)
                .Where(_ => typeof(IPresenter).IsAssignableFrom(_)
                    && _.Name.EndsWith("Presenter", StringComparison.Ordinal));
        }
    }
}
