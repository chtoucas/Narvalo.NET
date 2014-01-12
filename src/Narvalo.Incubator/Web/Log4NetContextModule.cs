namespace Narvalo.Web
{
    using System;
    using System.Web;

    public class Log4NetContextModule : IHttpModule
    {
        #region IHttpModule

        public void Init(HttpApplication context)
        {
            if (context == null) {
                throw new ArgumentNullException("context");
            }

            context.BeginRequest += OnBeginRequest;
            context.EndRequest += OnEndRequest;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        protected void OnBeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current == null) {
                // peut arriver quand on utilise trySkipIisCustomErrors
                return;
            }

            var req = HttpContext.Current.Request;
            var properties = log4net.ThreadContext.Properties;

            properties["domain"] = req.Url.Host;
            properties["rawUrl"] = req.RawUrl;
            properties["referrer"] = req.UrlReferrer != null ? req.UrlReferrer.ToString() : String.Empty;
            properties["ua"] = req.UserAgent;
        }

        protected void OnEndRequest(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties.Clear();
        }
    }
}
