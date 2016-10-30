// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    public sealed partial class DefaultCurrencyFactory : CurrencyFactory
    {
        public DefaultCurrencyFactory() { }

        protected override bool IsValid(string code) => s_CodeSet.Contains(code);
    }
}
