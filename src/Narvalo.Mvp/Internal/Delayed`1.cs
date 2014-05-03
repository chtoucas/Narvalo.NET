// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;

    [Obsolete]
    internal class Delayed<TValue>
    {
        readonly Lazy<TValue> _lazyValue;

        Func<TValue> _valueFactory;

        public Delayed(Func<TValue> valueFactory)
        {
            DebugCheck.NotNull(valueFactory);

            _valueFactory = valueFactory;
            // WARNING: Do not change the following line for:
            // _lazyValue = new Lazy<TValue>(_valueFactory);
            // as it will fail to capture the variable "_valueFactory".
            _lazyValue = new Lazy<TValue>(() => _valueFactory());
        }

        public TValue Value { get { return _lazyValue.Value; } }

        public bool CanReset { get { return !_lazyValue.IsValueCreated; } }

        public void Reset(TValue value)
        {
            Reset(() => value);
        }

        public void Reset(Func<TValue> valueFactory)
        {
            DebugCheck.NotNull(valueFactory);

            if (!CanReset) {
                throw new InvalidOperationException(
                    "Once accessed, you can no longer change the underlying value factory.");
            }

            // REVIEW: We break thread-safety here, but does it matter for our use case?
            _valueFactory = valueFactory;
        }
    }
}
