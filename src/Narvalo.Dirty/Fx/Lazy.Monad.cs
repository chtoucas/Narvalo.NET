// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class Lazy
    {
        //internal static Lazy<T> η<T>(T value)
        //{
        //    return new Lazy<T>(() => value);
        //}

        internal static Lazy<T> μ<T>(Lazy<Lazy<T>> square)
        {
            return square.Value;
        }
    }
}
