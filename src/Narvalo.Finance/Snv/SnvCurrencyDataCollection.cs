// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Snv
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    using static Narvalo.Finance.Snv.SnvDataHelpers;

    public abstract class SnvCurrencyDataCollection : IEnumerable<SnvCurrencyData>, IDisposable
    {
        private bool _disposed = false;
        private XmlReader _reader;

        protected SnvCurrencyDataCollection(string input, bool historical)
        {
            Require.NotNullOrEmpty(input, nameof(input));

            _reader = SnvXmlReader.Of(input);
            Historical = historical;
        }

        protected SnvCurrencyDataCollection(Stream input, bool historical)
        {
            Require.NotNull(input, nameof(input));

            _reader = SnvXmlReader.Of(input);
            Historical = historical;
        }

        protected SnvCurrencyDataCollection(TextReader input, bool historical)
        {
            Require.NotNull(input, nameof(input));

            _reader = SnvXmlReader.Of(input);
            Historical = historical;
        }

        protected SnvCurrencyDataCollection(XmlReader input, bool historical)
        {
            Require.NotNull(input, nameof(input));

            _reader = SnvXmlReader.Of(input);
            Historical = historical;
        }

        public event EventHandler<PubDateEventArgs> PubDateLoaded;

        public bool Historical { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<SnvCurrencyData> GetEnumerator()
        {
            ThrowIfDisposed();

            var root = XElement.Load(_reader, LoadOptions.None);

            var pubDate = root.AttributeOrThrow(SnvXmlNames.PubDate, ParsePubDate);
            OnPubDateLoaded(new PubDateEventArgs(pubDate));

            return Traverse(root);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected abstract IEnumerator<SnvCurrencyData> Traverse(XElement root);

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(typeof(SnvCurrencyDataCollection).FullName);
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
