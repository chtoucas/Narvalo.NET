// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.Windows.Forms;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    public static class PresenterBinderFactory
    {
        readonly static IMessageBus MessageBus_
            = PlatformServices.Current.MessageBusFactory.Create();

        public static PresenterBinder Create(Control control)
        {
            return Create(control, PlatformServices.Current);
        }

        internal static PresenterBinder Create(
            Control control,
            IPlatformServices platformServices)
        {
            DebugCheck.NotNull(platformServices);

            return new PresenterBinder(
                new[] { control },
                platformServices.PresenterDiscoveryStrategy,
                platformServices.PresenterFactory,
                platformServices.CompositeViewFactory,
                MessageBus_);
        }
    }
}
