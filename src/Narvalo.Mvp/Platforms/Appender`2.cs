// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using System;

    public sealed partial class Appender<TSource, T> where TSource : class
    {
        private readonly TSource _source;
        private readonly Action<T> _append;

        public Appender(TSource source, Action<T> append)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(append, nameof(append));

            _source = source;
            _append = append;
        }

        public TSource With(params T[] values)
        {
            Require.NotNull(values, nameof(values));

            foreach (var value in values)
            {
                if (value == null) { continue; }

                _append(value);
            }

            return _source;
        }
    }
}
