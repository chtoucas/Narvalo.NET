// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static class Stubs<T>
    {
        private static readonly Func<T, bool> s_AlwaysFalse = _ => false;

        private static readonly Func<T, bool> s_AlwaysTrue = _ => true;

        private static readonly Func<T, T> s_Identity = _ => _;

        private static readonly Action<T> s_Ignore = _ => { };

        public static Func<T, bool> AlwaysFalse { get { return s_AlwaysFalse; } }

        public static Func<T, bool> AlwaysTrue { get { return s_AlwaysTrue; } }

        public static Func<T, T> Identity { get { return s_Identity; } }

        public static Action<T> Ignore { get { return s_Ignore; } }
    }
}
