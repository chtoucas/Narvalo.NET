// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static class Fun
    {
        public static Func<T> Return<T>(T value)
        {
            return () => value;
        }

        public static Func<T> Flatten<T>(Func<Func<T>> square)
        {
            return () => square.Invoke().Invoke();
            //return square.Bind(_ => _);
        }
    }
}
