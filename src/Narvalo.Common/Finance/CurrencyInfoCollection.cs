// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Gendarme.Rules.Design", "ListsAreStronglyTypedRule",
        Justification = "[Intentionally] The base class implements the strongly typed versions.")]
    public sealed class CurrencyInfoCollection : ReadOnlyCollection<CurrencyInfo>
    {
        public CurrencyInfoCollection(IList<CurrencyInfo> list) : base(list) { }
    }
}
