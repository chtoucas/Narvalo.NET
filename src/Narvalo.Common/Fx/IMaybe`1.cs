// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    public interface IMaybe<out T>
    {
        bool IsSome { get; }

        T Value { get; }
    }
}
