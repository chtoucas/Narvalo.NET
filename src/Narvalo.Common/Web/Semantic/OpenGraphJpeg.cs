namespace Narvalo.Web.Semantic
{
    using System;

    public class OpenGraphJpeg : OpenGraphImage
    {
        public OpenGraphJpeg(Uri url) : base(url, "image/jpeg") { }
    }
}
