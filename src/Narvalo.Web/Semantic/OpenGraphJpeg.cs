namespace Narvalo.Web.Semantic
{
    using System;

    public sealed class OpenGraphJpeg : OpenGraphImage
    {
        public OpenGraphJpeg(Uri url) : base(url, "image/jpeg") { }
    }
}
