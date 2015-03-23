// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public class OpenGraphImage
    {
        private readonly string _mimeType;
        private readonly Uri _url;

        public OpenGraphImage(Uri url, string mimeType)
        {
            Require.NotNull(url, "url");
            Require.NotNull(mimeType, "mimeType");

            _url = url;
            _mimeType = mimeType;
        }

        public Uri Url
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                return _url;
            }
        }

        public string MimeType
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return _mimeType;
            }
        }

        public int Height { get; set; }

        public int Width { get; set; }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_mimeType != null);
            Contract.Invariant(_url != null);
        }

#endif
    }
}
