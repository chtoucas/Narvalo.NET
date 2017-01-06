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
        {
            if (value == 0m) { return 0m; }
            return UpOrDown
               ? DecimalRounding.RoundHalfUp(value)
               : DecimalRounding.RoundHalfDown(value);
        }

        public decimal Round(decimal value, int decimals)
        {
            if (value == 0m) { return 0m; }
            return UpOrDown
                ? DecimalRounding.RoundHalfUp(value, decimals)
                : DecimalRounding.RoundHalfDown(value, decimals);
        }

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
