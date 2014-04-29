// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Narvalo;
    using Narvalo.Mvp.Binder;

    internal sealed class FormViewHost
    {
        static readonly IDictionary<int, FormViewHost> _formViewHosts
           = new Dictionary<int, FormViewHost>();

        readonly PresenterBinder _presenterBinder;

        public FormViewHost(Form form)
        {
            DebugCheck.NotNull(form);

            _presenterBinder = new PresenterBinder(form);

            form.Load += Form_Load;
            form.Disposed += Form_Disposed;
        }

        void Form_Load(object sender, EventArgs e)
        {
            _presenterBinder.PerformBinding();
        }

        void Form_Disposed(object sender, EventArgs e)
        {
            _presenterBinder.Release();
        }

        void RegisterView_(IView view)
        {
            _presenterBinder.RegisterView(view);
        }

        public static void Register<TControl>(TControl control)
            where TControl : Control, IView
        {
            DebugCheck.NotNull(control);

            var form = control.FindForm();

            if (form == null)
                throw new InvalidOperationException("Controls can only be registered once they have been added to the live control tree. The best place to register them is within the control's Init event.");

            var viewHost = FindViewHost_(control, form);
            viewHost.RegisterView_(control);
        }

        static FormViewHost FindViewHost_(Control control, Form form)
        {
            var formKey = form.GetHashCode();

            if (_formViewHosts.ContainsKey(formKey)) {
                return _formViewHosts[formKey];
            }

            var host = new FormViewHost(form);

            _formViewHosts[formKey] = host;

            return host;
        }
    }
}
