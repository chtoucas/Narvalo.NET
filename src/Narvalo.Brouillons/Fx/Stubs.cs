// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static class Stubs
    {
        private static readonly Action s_Noop = () => { };

        public static Action Noop { get { return s_Noop; } }
    }
}
