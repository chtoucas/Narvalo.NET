// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    public static class Identity
    {
        public static Identity<T> Create<T>(T value)
        {
            return Identity<T>.η(value);
        }
    }
}
