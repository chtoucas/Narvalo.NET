// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /*!
     * The trivial monad. Useless in the context of C#.
     */

    public sealed partial class Identity<T> : IEquatable<Identity<T>>, IEquatable<T>
    {
        readonly T _value;

        Identity(T value)
        {
            _value = value;
        }

        public T Value { get { return _value; } }
    }
}
