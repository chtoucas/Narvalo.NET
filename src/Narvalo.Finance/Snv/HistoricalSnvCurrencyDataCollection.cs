// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Snv
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    using static Narvalo.Finance.Snv.SnvDataHelpers;

    public sealed class HistoricalSnvCurrencyDataCollection : SnvCurrencyDataCollection
    {
        public HistoricalSnvCurrencyDataCollection(string input)
            : base(input, true)
        {
            Expect.NotNullOrEmpty(input);
        }

        public HistoricalSnvCurrencyDataCollection(Stream input)
            : base(input, true)
        {
            Expect.NotNull(input);
        }

        public HistoricalSnvCurrencyDataCollection(TextReader input)
            : base(input, true)
        {
            Expect.NotNull(input);
        }

        public HistoricalSnvCurrencyDataCollection(XmlReader input)
            : base(input, true)
        {
            Expect.NotNull(input);
        }

        protected override IEnumerator<SnvCurrencyData> Traverse(XElement root)
        {
            ThrowIfDisposed();

            var coll = root.ElementOrThrow(SnvXmlNames.LegacyList).Elements(SnvXmlNames.LegacyItem);

            foreach (var item in coll)
            {
                var code = item.ElementOrThrow(SnvXmlNames.AlphabeticCode).Value;
                short? numericCode = item.Element(SnvXmlNames.NumericCode)?.Value(TryParseNumericCode);

                var cdata = SnvCurrencyData.CreateWithdrawnCurrency(code, numericCode);

                var nameElement = item.ElementOrThrow(SnvXmlNames.EnglishName);
                cdata.EnglishName = nameElement.Value(CleanupCurrencyName);
                cdata.IsFund = nameElement.AttributeOrElse(SnvXmlNames.IsFund, ParseIsFund, false);

                cdata.EnglishCountryName = item.ElementOrThrow(SnvXmlNames.CountryName).Value(CleanupCountryName);

                if (!numericCode.HasValue)
                {
                    Debug.WriteLine("Found a legacy currency without a numeric code: " + cdata.EnglishCountryName);
                }

                yield return cdata;
            }
        }
    }
}
