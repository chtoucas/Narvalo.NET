// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Snv
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    using static Narvalo.Finance.Snv.SnvDataHelpers;

    public sealed class SnvCurrencyDataCollection : IEnumerable<SnvCurrencyData>, IDisposable
    {
        private bool _disposed = false;
        private XmlReader _reader;

        public SnvCurrencyDataCollection(string input, bool withdrawn)
        {
            Demand.NotNullOrEmpty(input);

            _reader = SnvXmlReader.Of(input);
            Withdrawn = withdrawn;
        }

        public SnvCurrencyDataCollection(Stream input, bool withdrawn)
        {
            Demand.NotNull(input);

            _reader = SnvXmlReader.Of(input);
            Withdrawn = withdrawn;
        }

        public SnvCurrencyDataCollection(TextReader input, bool withdrawn)
        {
            Demand.NotNull(input);

            _reader = SnvXmlReader.Of(input);
            Withdrawn = withdrawn;
        }

        public SnvCurrencyDataCollection(XmlReader input, bool withdrawn)
        {
            Demand.NotNull(input);

            _reader = SnvXmlReader.Of(input);
            Withdrawn = withdrawn;
        }

        public event EventHandler<PubDateEventArgs> PubDateLoaded;

        public bool Withdrawn { get; }

        public void Dispose() => Dispose(true);

        public IEnumerator<SnvCurrencyData> GetEnumerator()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(typeof(SnvCurrencyDataCollection).FullName);
            }

            var root = XElement.Load(_reader, LoadOptions.None);

            var pubDate = root.AttributeOrThrow(SnvXmlNames.PubDate, ParsePubDate);
            OnPubDateLoaded(new PubDateEventArgs(pubDate));

            return Withdrawn ? TraverseLegacy(root) : Traverse(root);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<SnvCurrencyData> Traverse(XElement root)
        {
            var coll = root.ElementOrThrow(SnvXmlNames.List).Elements(SnvXmlNames.Item);

            foreach (var item in coll)
            {
                var codeElement = item.Element(SnvXmlNames.AlphabeticCode);
                if (codeElement == null)
                {
                    Debug.WriteLine("Found a currency without alphabetic code.");

                    continue;
                }

                var code = codeElement.Value;
                var numericCode = item.ElementOrThrow(SnvXmlNames.NumericCode).Value(ParseNumericCode);

                var cdata = SnvCurrencyData.Create(code, numericCode);

                var nameElement = item.ElementOrThrow(SnvXmlNames.EnglishName);
                cdata.EnglishName = nameElement.Value(CleanupCurrencyName);
                cdata.IsFund = nameElement.AttributeOrElse(SnvXmlNames.IsFund, ParseIsFund, false);

                cdata.EnglishCountryName = item.ElementOrThrow(SnvXmlNames.CountryName).Value(CleanupCountryName);
                cdata.MinorUnits = item.ElementOrThrow(SnvXmlNames.MinorUnits).Value(ParseMinorUnits);

                yield return cdata;
            }
        }

        private IEnumerator<SnvCurrencyData> TraverseLegacy(XElement root)
        {
            var coll = root.ElementOrThrow(SnvXmlNames.LegacyList).Elements(SnvXmlNames.LegacyItem);

            foreach (var item in coll)
            {
                var code = item.ElementOrThrow(SnvXmlNames.AlphabeticCode).Value;
                short? numericCode = item.Element(SnvXmlNames.NumericCode)?.Value(TryParseNumericCode);

                var cdata = SnvCurrencyData.CreateLegacy(code, numericCode);

                var nameElement = item.ElementOrThrow(SnvXmlNames.EnglishName);
                cdata.EnglishName = nameElement.Value(CleanupCurrencyName);
                cdata.IsFund = nameElement.AttributeOrElse(SnvXmlNames.IsFund, ParseIsFund, false);

                cdata.EnglishCountryName = item.ElementOrThrow(SnvXmlNames.CountryName).Value(CleanupCountryName);

                yield return cdata;
            }
        }

        private void OnPubDateLoaded(PubDateEventArgs e) => PubDateLoaded?.Invoke(this, e);

        private void Dispose(bool disposing)
        {
            if (_disposed) { return; }

            if (disposing)
            {
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }
            }

            _disposed = true;
        }
    }
}
