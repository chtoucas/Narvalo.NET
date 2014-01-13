namespace Narvalo.Log4Net
{
    using System;
    using System.Web;
    using Narvalo;

    public class Log4NetHttpModule : IHttpModule
    {
        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

            context.BeginRequest += OnBeginRequest_;
            context.EndRequest += OnEndRequest_;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

         void OnBeginRequest_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            var req = app.Request;
            var properties = log4net.ThreadContext.Properties;

            properties["domain"] = req.Url.Host;
            properties["rawUrl"] = req.RawUrl;
            properties["referrer"] = req.UrlReferrer != null ? req.UrlReferrer.ToString() : String.Empty;
            properties["ua"] = req.UserAgent;
        }

         void OnEndRequest_(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties.Clear();
        }
    }
}
