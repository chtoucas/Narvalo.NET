// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Legacy
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed class CurrencyInfoCollection : ReadOnlyCollection<CurrencyInfo>
    {
        public CurrencyInfoCollection(IList<CurrencyInfo> list) : base(list) { }
    }
}
