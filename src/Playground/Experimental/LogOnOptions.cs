namespace Narvalo.Playground
{
    using System;
    using System.Collections.Specialized;
    using Narvalo;
    using Narvalo.Web;

    public class LogOnOptions
    {
        public static readonly string CreatePersistentCookieKey = "persistent";
        public static readonly string TargetUrlKey = "target";

        bool _createPersistentCookie;
        Uri _targetUrl;

        public bool CreatePersistentCookie
        {
            get { return _createPersistentCookie; }
            set { _createPersistentCookie = value; }
        }

        public Uri TargetUrl
        {
            get { return _targetUrl; }
            set
            {
                if (_targetUrl.IsAbsoluteUri) {
                    throw ExceptionFactory.Argument("XXX", "targetUrl");
                }
                _targetUrl = value;
            }
        }

        public string ToQueryString()
        {
            var nvc = new NameValueCollection();

            if (_createPersistentCookie) {
                nvc.Add(CreatePersistentCookieKey, "1");
            }
            if (_targetUrl != null) {
                nvc.Add(TargetUrlKey, TargetUrl.ToString());
            }

            return nvc.ToQueryString();
        }
    }
}
