// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System;
    using System.Windows.Forms;
    using Narvalo;

    internal class FormHost
    {
        readonly FormPresenterBinder _presenterBinder;

        public FormHost(Form form)
        {
            _presenterBinder = new FormPresenterBinder(form);

            form.Load += (sender, e) => _presenterBinder.PerformBinding();
            form.Disposed += (sender, e) => _presenterBinder.Release();
        }

        public static void RegisterControl<T>(T control)
             where T : Control, IView
        {
            DebugCheck.NotNull(control);

            var form = control.FindForm();

            if (form == null) {
                throw new InvalidOperationException(
                    "Controls can only be registered once they have been added to the live control tree.");
            }

            var host = form.GetOrAddHost();

            host.RegisterView(control);
        }

        public static void RegisterForm(Form form)
        {
            DebugCheck.NotNull(form);

            form.GetOrAddHost();
        }

        public void RegisterView(IView view)
        {
            _presenterBinder.RegisterView(view);
        }
    }
}
