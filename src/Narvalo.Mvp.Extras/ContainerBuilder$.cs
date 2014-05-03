// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Reflection;
    using Autofac;
    using Autofac.Builder;
    using Autofac.Features.Scanning;
    using Narvalo.Mvp.CommandLine;

    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterCommands(
                this ContainerBuilder builder,
                params Assembly[] controllerAssemblies)
        {
            return builder.RegisterAssemblyTypes(controllerAssemblies)
                .Where(t => typeof(ICommand).IsAssignableFrom(t) &&
                    t.Name.EndsWith("Command", StringComparison.Ordinal));
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterPresenters(
                this ContainerBuilder builder,
                params Assembly[] controllerAssemblies)
        {
            return builder.RegisterAssemblyTypes(controllerAssemblies)
                .Where(t => typeof(IPresenter).IsAssignableFrom(t) &&
                    t.Name.EndsWith("Presenter", StringComparison.Ordinal));
        }
    }
}
