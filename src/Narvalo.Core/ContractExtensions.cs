// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics.Contracts;

    public static class ContractExtensions
    {
        public static T AssumeNotNull<T>(this T obj) where T : class
        {
            Contract.Ensures(Contract.Result<T>() == obj);
            Contract.Ensures(Contract.Result<T>() != null);
            Contract.Assume(obj != null);

            return obj;
        }
    }
}
