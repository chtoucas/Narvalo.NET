// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms.Core
{
    using System.Windows.Forms;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    public static class FormsPresenterBinderFactory
    {
        public static PresenterBinder Create(Control control)
        {
            return Create(control, FormsPlatformServices.Current);
        }

        public static PresenterBinder Create(
            Control control,
            IPlatformServices platformServices)
        {
            Require.NotNull(platformServices, "platformServices");

            return new PresenterBinder(
                new[] { control },
                platformServices.PresenterDiscoveryStrategy,
                platformServices.PresenterFactory,
                platformServices.CompositeViewFactory,
                platformServices.MessageBusFactory);
        }
    }
}
