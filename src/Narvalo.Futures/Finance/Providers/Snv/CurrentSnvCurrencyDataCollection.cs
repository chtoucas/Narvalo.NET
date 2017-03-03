// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Providers.Snv
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    using static Narvalo.Finance.Providers.Snv.SnvDataHelpers;

    public sealed class CurrentSnvCurrencyDataCollection : SnvCurrencyDataCollection
    {
        public CurrentSnvCurrencyDataCollection(string input) : base(input, false) { }

        public CurrentSnvCurrencyDataCollection(Stream input) : base(input, false) { }

        public CurrentSnvCurrencyDataCollection(TextReader input) : base(input, false) { }

        public CurrentSnvCurrencyDataCollection(XmlReader input) : base(input, false) { }

        protected override IEnumerator<SnvCurrencyData> Traverse(XElement root)
        {
            ThrowIfDisposed();

            var coll = root.ElementOrThrow(SnvXmlNames.List).Elements(SnvXmlNames.Item);

            foreach (var item in coll)
            {
                var codeElement = item.Element(SnvXmlNames.AlphabeticCode);
                if (codeElement == null)
                {
                    var countryName = item.ElementOrThrow(SnvXmlNames.CountryName).Value(CleanupCountryName);
                    Debug.WriteLine("Found a currency without a universal currency: " + countryName);

                    continue;
                }

                var code = codeElement.Value;
                var numericCode = item.ElementOrThrow(SnvXmlNames.NumericCode).Value(ParseNumericCode);

                var cdata = SnvCurrencyData.CreateCurrency(code, numericCode);

                var nameElement = item.ElementOrThrow(SnvXmlNames.EnglishName);
                cdata.EnglishName = nameElement.Value(CleanupCurrencyName);
                cdata.IsFund = nameElement.AttributeOrElse(SnvXmlNames.IsFund, ParseIsFund, false);

                cdata.EnglishCountryName = item.ElementOrThrow(SnvXmlNames.CountryName).Value(CleanupCountryName);
                cdata.MinorUnits = item.ElementOrThrow(SnvXmlNames.MinorUnits).Value(ParseMinorUnits);

                yield return cdata;
            }
        }
    }
}
