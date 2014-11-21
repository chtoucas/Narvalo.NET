// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics.Contracts;

    public static class CodeContract
    {
        public static T AssumeNotNull<T>(this T obj) where T : class
        {
            Contract.Ensures(Contract.Result<T>() == obj);
            Contract.Ensures(Contract.Result<T>() != null);
            Contract.Assume(obj != null);

            return obj;
        }

        /// <summary>
        /// According to its documentation, CCCheck only assumes and asserts
        /// the object invariance for the "this" object. This method allows
        /// to state explicitly the object invariance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void AssumeInvariant<T>(T obj) where T : class { }
    }
}
