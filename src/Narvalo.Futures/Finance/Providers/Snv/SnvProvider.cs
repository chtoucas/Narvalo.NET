// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Providers.Snv
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public sealed class SnvProvider
    {
        private SnvProvider(string fileName, bool historical)
        {
            Require.NotNull(fileName, nameof(fileName));

            FileName = fileName;
            Historical = historical;
        }

        public string FileName { get; }

        public bool Historical { get; }

        public static SnvProvider GetCurrentDatabase(string fileName)
        {
            return new SnvProvider(fileName, false);
        }

        public static SnvProvider GetHistoricalDatabase(string fileName)
        {
            return new SnvProvider(fileName, true);
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
