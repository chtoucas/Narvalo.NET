// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using System;

    public sealed class Setter<TSource, T> where TSource : class
    {
        private readonly TSource _source;
        private readonly Action<T> _set;

        public Setter(TSource source, Action<T> set)
        {
            Require.NotNull(source, "source");
            Require.NotNull(set, "set");

            _source = source;
            _set = set;
        }

        public TSource Is(T value)
        {
            _set(value);

            return _source;
        }
    }
}
