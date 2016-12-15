// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Collections.Generic;

    public sealed partial class DefaultCurrencyFactory : CurrencyFactory
    {
        private volatile static HashSet<string> s_CodeSet;

        public DefaultCurrencyFactory() { }

        protected override bool Validate(string code) => CodeSet.Contains(code);
    }
}
