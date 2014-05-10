// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms.Internal
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Narvalo;

    internal sealed class FormHost
    {
        // REVIEW
        static readonly IDictionary<int, FormHost> _hosts
           = new Dictionary<int, FormHost>();

        readonly FormsPresenterBinder _presenterBinder;

        public FormHost(Form form)
        {
            DebugCheck.NotNull(form);

            _presenterBinder = FormsPresenterBinderFactory.Create(form);

            // NB: Since we create the host during the CreateControl event, 
            // the binding operation must occur afterwards.
            form.Load += (sender, e) => _presenterBinder.PerformBinding();

            form.Disposed += (sender, e) =>
            {
                var hostKey = sender.GetHashCode();
                _hosts.Remove(hostKey);

                _presenterBinder.Release();
            };
        }

        public static FormHost Register(Form form)
        {
            DebugCheck.NotNull(form);

            var hostKey = form.GetHashCode();

            if (_hosts.ContainsKey(hostKey)) {
                return _hosts[hostKey];
            }
            else {
                var host = new FormHost(form);
                _hosts[hostKey] = host;
                return host;
            }
        }

        public void RegisterView(IView view)
        {
            _presenterBinder.RegisterView(view);
        }
    }
}
