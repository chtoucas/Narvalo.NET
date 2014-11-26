// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Externs.Log4Net
{
    using System;
    using System.Web;
    using Narvalo;

    public class Log4NetHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            Require.NotNull(context, "context");

            context.BeginRequest += OnBeginRequest_;
            context.EndRequest += OnEndRequest_;
        }

        public void Dispose() { }

        void OnBeginRequest_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            var request = app.Request;
            var properties = log4net.ThreadContext.Properties;

            properties["Domain"] = request.Url.Host;
            properties["RawUrl"] = request.RawUrl;
            properties["UserHostAddress"] 
                = request.UserHostAddress != null ? request.UserHostAddress : String.Empty;
            properties["UrlReferrer"] 
                = request.UrlReferrer != null ? request.UrlReferrer.ToString() : String.Empty;
            properties["UserAgent"] = request.UserAgent;
        }

        void OnEndRequest_(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties.Clear();
        }
    }
}
