// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System.Globalization;

    public sealed partial class OpenGraphLocale
    {
        private readonly CultureInfo _culture;

        public OpenGraphLocale(CultureInfo culture)
        {
            Require.NotNull(culture, nameof(culture));

            _culture = culture;
        }

        public CultureInfo Culture
        {
            get
            {
                Warrant.NotNull<CultureInfo>();

                return _culture;
            }
        }

        public override string ToString()
        {
            return Culture.ToString().Replace('-', '_');
        }
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Web.Semantic
{
    using System.Diagnostics.Contracts;

    public sealed partial class OpenGraphLocale
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_culture != null);
        }
    }
}

#endif
