// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Narvalo;

    internal static class FormHost
    {
        static readonly IDictionary<int, FormBinder> _cache
           = new Dictionary<int, FormBinder>();

        public static void Register<TControl>(TControl control)
            where TControl : Control, IView
        {
            DebugCheck.NotNull(control);

            var form = control.FindForm();

            if (form == null) {
                throw new InvalidOperationException(
                    "Controls can only be registered once they have been added to the live control tree.");
            }

            var host = GetOrAddHost_(form);
            host.RegisterView(control);
        }

        static FormBinder GetOrAddHost_(Form form)
        {
            DebugCheck.NotNull(form);

            var cacheKey = form.GetHashCode();

            if (_cache.ContainsKey(cacheKey)) {
                return _cache[cacheKey];
            }

            var host = new FormBinder(form);

            _cache[cacheKey] = host;

            return host;
        }
    }

}
