// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System;

    public partial class OpenGraphImage
    {
        public OpenGraphImage(Uri url, string mimeType)
        {
            Require.NotNull(url, nameof(url));
            Require.NotNull(mimeType, nameof(mimeType));

            Url = url;
            MimeType = mimeType;
        }

        public Uri Url { get; }

        public string MimeType { get; }

        public int Height { get; set; }

        public int Width { get; set; }
    }
}
