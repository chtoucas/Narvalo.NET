// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System;

    public sealed class OpenGraphJpeg : OpenGraphImage
    {
        public OpenGraphJpeg(Uri url)
            : base(url, "image/jpeg")
        {
            Expect.NotNull(url);
        }
    }
}
