// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System.Globalization;

    public sealed partial class OpenGraphLocale
    {
        public OpenGraphLocale(CultureInfo culture)
        {
            Require.NotNull(culture, nameof(culture));

            Culture = culture;
        }

        public CultureInfo Culture { get; }

        public override string ToString() => Culture.ToString().Replace('-', '_');
    }
}
