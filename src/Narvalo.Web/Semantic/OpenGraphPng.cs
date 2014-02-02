namespace Narvalo.Web.Semantic
{
    using System;

    public sealed class OpenGraphPng : OpenGraphImage
    {
        public OpenGraphPng(Uri url) : base(url, "image/png") { }
    }
}
