namespace Narvalo.Web.Social
{
    using System;
    using System.Web;

    public class FacebookContext
    {
        const long AnonymousId_ = 0;

        //static readonly object ContextKey = new Object();
        static readonly FacebookContext AnonymousContext_ = new FacebookContext();

        readonly string _sessionKey;
        readonly long _userId;

        public FacebookContext()
            : this(AnonymousId_, null /* sessionKey */) { ; }

        public FacebookContext(FacebookCookie cookie)
            : this(cookie.UserId, cookie.SessionKey) { ; }

        public FacebookContext(long userId, string sessionKey)
        {
            _userId = userId;
            _sessionKey = sessionKey;
        }

        //public static FacebookContext Current
        //{
        //    get
        //    {
        //        // FIXME: ne pas utiliser HttpContext.Current
        //        // See http://odetocode.com/articles/112.aspx
        //        if (HttpContext.Current == null) {
        //            return AnonymousContext;
        //        }

        //        return GetCurrentContext(new HttpContextWrapper(HttpContext.Current));
        //    }
        //}

        public bool IsConnected
        {
            get
            {
                return _userId != AnonymousId_ && !String.IsNullOrEmpty(_sessionKey);
            }
        }

        public string SessionKey { get { return _sessionKey; } }
        public long UserId { get { return _userId; } }

        public static FacebookContext ResolveContext(HttpContextBase httpContext, string appId, string appSecret)
        {
            var cookie = FacebookCookie.Create(httpContext, appId, appSecret);

            return cookie.HasValue ? new FacebookContext(cookie.Value) : AnonymousContext_;
        }

        //static FacebookContext GetCurrentContext(HttpContextBase httpContext)
        //{
        //    var context = httpContext.Items[ContextKey] as FacebookContext;
        //    if (context == null) {
        //        var section = WebConfigurationManager<FacebookSection>.GetSection("narvalo.web/facebook");
        //        context = ResolveContext(httpContext, section);
        //        httpContext.Items[ContextKey] = context;
        //    }
        //    return context;
        //}
    }
}
