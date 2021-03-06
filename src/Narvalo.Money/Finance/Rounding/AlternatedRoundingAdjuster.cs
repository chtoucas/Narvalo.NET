﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Rounding
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class AlternatedRoundingAdjuster : IRoundingAdjuster, IDisposable
    {
        private bool _disposed = false;
        private IEnumerator<bool> _iterator;

        public AlternatedRoundingAdjuster()
        {
            _iterator = new BooleanSequence().GetEnumerator();
        }

        private bool UpOrDown
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(typeof(AlternatedRoundingAdjuster).FullName);
                }

                _iterator.MoveNext();
                return _iterator.Current;
            }
        }

        public void Dispose() => Dispose(true);

        public decimal Round(decimal value)
            => UpOrDown
            ? RoundingAdjusters.HalfUp(value)
            : RoundingAdjusters.HalfDown(value);

        public decimal Round(decimal value, int decimalPlaces)
            => UpOrDown
            ? RoundingAdjusters.HalfUp(value, decimalPlaces)
            : RoundingAdjusters.HalfDown(value, decimalPlaces);

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
