// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Autofac
{
    using System;
    using System.Reflection;
    using global::Autofac;
    using global::Autofac.Builder;
    using global::Autofac.Features.Scanning;
    using Narvalo.Mvp;

    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterPresenters(
                this ContainerBuilder @this,
                params Assembly[] controllerAssemblies)
        {
            Require.Object(@this);

            return @this.RegisterAssemblyTypes(controllerAssemblies)
                .Where(_ => typeof(IPresenter).IsAssignableFrom(_) &&
                    _.Name.EndsWith("Presenter", StringComparison.Ordinal));
        }
    }
}
