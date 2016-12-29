// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Linq;

    public sealed class Weights
    {
        private readonly decimal[] _arr;

        private Weights(decimal[] arr)
        {
            Demand.NotNull(arr);

            _arr = arr;
        }

        public int Length => _arr.Length;

        public decimal this[int i] {
            get
            {
                if (i < 0 || i >= _arr.Length)
                {
                    throw new IndexOutOfRangeException();
                }

                return _arr[i];
            }
        }

        public static Weights From(int[] arr)
        {
            if (arr.Sum() != 100) { throw new ArgumentException(); }

            var weights = new decimal[arr.Length];
            for (var i = 0; i < arr.Length; i++)
            {
                weights[i] *= 0.01M;
            }

            return new Weights(weights);
        }

        public static Weights From(decimal[] ratios)
        {
            if (ratios.Sum() != 1M) { throw new ArgumentException(); }

            return new Weights(ratios);
        }
    }
}
