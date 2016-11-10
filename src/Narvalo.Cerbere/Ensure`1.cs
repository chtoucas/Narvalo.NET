// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    public static class Ensure<T> where T: class
    {
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNull() => Contract.Ensures(Contract.Result<T>() != null);
    }
}
