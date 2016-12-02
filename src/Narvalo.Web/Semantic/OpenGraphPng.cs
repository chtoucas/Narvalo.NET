// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System;
    using System.Diagnostics.Contracts;

    public sealed class OpenGraphPng : OpenGraphImage
    {
        public OpenGraphPng(Uri url)
            : base(url, "image/png")
        {
            Expect.NotNull(url);
        }
    }
}
