// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System;

    public class OpenGraphImage
    {
        private readonly string _mimeType;
        private readonly Uri _url;

        public OpenGraphImage(Uri url, string mimeType)
        {
            Require.NotNull(url, nameof(url));
            Require.NotNull(mimeType, nameof(mimeType));

            _url = url;
            _mimeType = mimeType;
        }

        public Uri Url
        {
            get
            {
                Warrant.NotNull<Uri>();

                return _url;
            }
        }

        public string MimeType
        {
            get
            {
                Warrant.NotNull<string>();

                return _mimeType;
            }
        }

        public int Height { get; set; }

        public int Width { get; set; }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_mimeType != null);
            Contract.Invariant(_url != null);
        }

#endif
    }
}
