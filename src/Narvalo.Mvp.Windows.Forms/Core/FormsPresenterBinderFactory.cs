// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms.Core
{
    using System.Windows.Forms;

    public static class FormsPresenterBinderFactory
    {
        public static FormsPresenterBinder Create(Control control)
        {
            return Create(control, FormsPlatformServices.Current);
        }

        public static FormsPresenterBinder Create(
            Control control,
            IFormsPlatformServices platformServices)
        {
            Require.NotNull(platformServices, "platformServices");

            return new FormsPresenterBinder(
                control,
                platformServices.PresenterDiscoveryStrategy,
                platformServices.PresenterFactory,
                platformServices.CompositeViewFactory,
                platformServices.MessageBus);
        }
    }
}
