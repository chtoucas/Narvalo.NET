// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System;

    using Narvalo;

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

        public Uri Url { get { return _url; } }

        public string MimeType { get { return _mimeType; } }

        public int Height { get; set; }

        public int Width { get; set; }
    }
}
