// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    public static partial class Maybe
    {
        public static Maybe<T> Create<T>(T? value) where T : struct
        {
            return value.HasValue ? Maybe<T>.η(value.Value) : Maybe<T>.None;
        }
    }
}
