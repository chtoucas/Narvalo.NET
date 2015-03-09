// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static class Stubs<T>
    {
        private static readonly Action<T> s_Ignore = _ => { };

        public static Action<T> Ignore { get { return s_Ignore; } }
    }
}
