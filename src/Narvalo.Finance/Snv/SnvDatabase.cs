// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Snv
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public sealed class SnvDatabase
    {
        private SnvDatabase(string fileName, bool historical)
        {
            Require.NotNull(fileName, nameof(fileName));

            FileName = fileName;
            Historical = historical;
        }

        public string FileName { get; }

        public bool Historical { get; }

        public static SnvDatabase GetCurrentDatabase(string fileName)
        {
            Expect.NotNull(fileName);

            return new SnvDatabase(fileName, false);
        }

        public static SnvDatabase GetHistoricalDatabase(string fileName)
        {
            Expect.NotNull(fileName);

            return new SnvDatabase(fileName, true);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "[Intentionally] The method returns an enumeration.")]
        public IEnumerable<SnvCurrencyData> GetCurrencies()
        {
            using (var coll = GetCollection())
            {
                foreach (var item in coll)
                {
                    yield return item;
                }
            }
        }

        private SnvCurrencyDataCollection GetCollection()
        {
            if (Historical)
            {
                return new HistoricalSnvCurrencyDataCollection(FileName);
            }
            else
            {
                return new CurrentSnvCurrencyDataCollection(FileName);
            }
        }
    }
}
