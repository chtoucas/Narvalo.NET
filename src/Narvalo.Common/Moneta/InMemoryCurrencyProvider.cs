// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Moneta
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Gendarme.Rules.Naming", "UseCorrectPrefixRule",
        Justification = "[Ignore] The type's name starts with 'In' not 'I'.")]
    public sealed partial class InMemoryCurrencyProvider : ICurrencyProvider
    {
        public HashSet<string> CurrencyCodes
        {
            get { return s_CurrencyCodeSet; }
        }
    }
}
