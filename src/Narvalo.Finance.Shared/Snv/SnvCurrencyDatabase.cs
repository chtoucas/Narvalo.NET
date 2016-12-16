// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Snv
{
    using System.Collections.Generic;

    public sealed class SnvCurrencyDatabase
    {
        public SnvCurrencyDatabase(string fileName, bool withdrawn)
        {
            Require.NotNull(fileName, nameof(fileName));

            FileName = fileName;
        }

        public string FileName { get; }

        public bool Withdrawn { get; }

        public IEnumerable<SnvCurrencyData> GetCurrencies()
        {
            using (var coll = new SnvCurrencyDataCollection(FileName, Withdrawn))
            {
                foreach (var item in coll)
                {
                    yield return item;
                }
            }
        }
    }
}
