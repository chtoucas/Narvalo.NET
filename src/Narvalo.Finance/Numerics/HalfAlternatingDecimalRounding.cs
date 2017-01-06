// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class HalfAlternatingDecimalRounding : IDecimalRounding, IDisposable
    {
        private bool _disposed = false;
        private IEnumerator<bool> _iterator;

        public HalfAlternatingDecimalRounding()
        {
            _iterator = new BooleanSequence().GetEnumerator();
        }

        private bool UpOrDown
        {
            get
            {
                _iterator.MoveNext();
                return _iterator.Current;
            }
        }

        public void Dispose() => Dispose(true);

        public decimal Round(decimal value)
            => UpOrDown
            ? DecimalRounding.Round(value, NumberRounding.HalfUp)
            : DecimalRounding.Round(value, NumberRounding.HalfDown);

        public decimal Round(decimal value, int decimals)
            => UpOrDown
            ? DecimalRounding.Round(value, decimals, NumberRounding.HalfUp)
            : DecimalRounding.Round(value, decimals, NumberRounding.HalfDown);

        private void Dispose(bool disposing)
        {
            if (_disposed) { return; }

            if (disposing)
            {
                if (_iterator != null)
                {
                    _iterator.Dispose();
                    _iterator = null;
                }
            }

            _disposed = true;
        }

        private class BooleanSequence : IEnumerable<bool>
        {
            public IEnumerator<bool> GetEnumerator()
            {
                while (true)
                {
                    yield return true;
                    yield return false;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
