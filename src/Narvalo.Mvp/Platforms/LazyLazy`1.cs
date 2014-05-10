// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using System;
    using System.ComponentModel;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LazyLazy<TValue>
    {
        readonly Lazy<TValue> _lazyValue;

        Func<TValue> _valueFactory;

        public LazyLazy(Func<TValue> valueFactory)
        {
            DebugCheck.NotNull(valueFactory);

            _valueFactory = valueFactory;
            // WARNING: Do not change the following line for:
            // _lazyValue = new Lazy<TValue>(_valueFactory);
            // as it will fail to capture the variable "_valueFactory".
            _lazyValue = new Lazy<TValue>(() => _valueFactory());
        }

        public TValue Value { get { return _lazyValue.Value; } }

        public bool CanSet { get { return !_lazyValue.IsValueCreated; } }

        public void InnerSet(TValue value)
        {
            InnerSet(() => value);
        }

        public void InnerSet(Func<TValue> valueFactory)
        {
            DebugCheck.NotNull(valueFactory);

            if (!CanSet) {
                throw new InvalidOperationException(
                    "Once accessed, you can no longer change the underlying value factory.");
            }

            // REVIEW: We break thread-safety here, but does it matter for our use case?
            _valueFactory = valueFactory;
        }
    }
}
