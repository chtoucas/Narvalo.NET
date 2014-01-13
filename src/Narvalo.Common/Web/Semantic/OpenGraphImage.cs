namespace Narvalo.Web.Semantic
{
    using System;
    using Narvalo;

    public class OpenGraphImage
    {
        readonly string _mimeType;
        readonly Uri _url;

        public OpenGraphImage(Uri url, string mimeType)
        {
            Requires.NotNull(url, "url");
            Requires.NotNull(mimeType, "mimeType");

            _url = url;
            _mimeType = mimeType;
        }

        public Uri Url { get { return _url; } }
        public string MimeType { get { return _mimeType; } }

        public int Height { get; set; }
        public int Width { get; set; }
    }
}
