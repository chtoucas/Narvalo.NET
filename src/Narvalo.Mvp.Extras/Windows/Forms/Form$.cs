// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Narvalo;

    internal static class FormExtensions
    {
        static readonly IDictionary<int, FormHost> _hosts
           = new Dictionary<int, FormHost>();

        public static void RemoveHost(this Form @this)
        {
            DebugCheck.NotNull(@this);

            var hostKey = @this.GetHashCode();

            _hosts.Remove(hostKey);
        }

        public static FormHost GetOrAddHost(this Form @this)
        {
            DebugCheck.NotNull(@this);

            var hostKey = @this.GetHashCode();

            if (_hosts.ContainsKey(hostKey)) {
                return _hosts[hostKey];
            }
            else {
                var host = new FormHost(@this);
                _hosts[hostKey] = host;

                @this.Disposed += (sender, e) => _hosts.Remove(hostKey);

                return host;
            }
        }
    }
}
