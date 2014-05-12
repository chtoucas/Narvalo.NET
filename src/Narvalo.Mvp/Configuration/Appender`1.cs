// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Configuration
{
    using System;

    public sealed class Appender<TSource, T> where TSource : class
    {
        readonly TSource _source;
        readonly Action<T> _append;

        public Appender(TSource source, Action<T> append)
        {
            Require.NotNull(source, "source");
            Require.NotNull(append, "append");

            _source = source;
            _append = append;
        }

        public TSource With(params T[] values)
        {
            foreach (var value in values) {
                _append(value);
            }

            return _source;
        }
    }
}
