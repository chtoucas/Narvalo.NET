// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms.Core
{
    using System.Windows.Forms;

    public static class FormsPresenterBinderFactory
    {
        public static FormsPresenterBinder Create(Form form)
        {
            return Create(form, FormsPlatformServices.Current);
        }

        public static FormsPresenterBinder Create(
            Form form,
            IFormsPlatformServices container)
        {
            Require.NotNull(container, "container");

            return new FormsPresenterBinder(
                form,
                container.PresenterDiscoveryStrategy,
                container.PresenterFactory,
                container.CompositeViewFactory,
                container.MessageBus);
        }
    }
}
