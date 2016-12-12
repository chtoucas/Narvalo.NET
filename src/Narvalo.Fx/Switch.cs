// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    public static class Switch
    {
        public static Switch<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        {
            Warrant.NotNull<Switch<TLeft, TRight>>();

            return Switch<TLeft, TRight>.η(value);
        }

        public static Switch<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        {
            Warrant.NotNull<Switch<TLeft, TRight>>();

            return Switch<TLeft, TRight>.η(value);
        }
    }
}
