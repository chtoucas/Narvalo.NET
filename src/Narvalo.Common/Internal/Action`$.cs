// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    static partial class ActionExtensions
    {
        //// When

        public static Action When(this Action @this, bool predicate)
        {
            return predicate ? @this : Stubs.Noop;
        }

        public static Action<T> When<T>(this Action<T> @this, bool predicate)
        {
            return predicate ? @this : Stubs<T>.Ignore;
        }

        //// Unless

        public static Action Unless(this Action @this, bool predicate)
        {
            return When(@this, !predicate);
        }

        public static Action<T> Unless<T>(this Action<T> @this, bool predicate)
        {
            return When(@this, !predicate);
        }
    }
}
