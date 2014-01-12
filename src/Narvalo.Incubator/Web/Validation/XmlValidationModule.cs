namespace Narvalo.Web.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Xml.Schema;
    using Narvalo.Web.Internal;

    public class XmlValidationModule : IHttpModule
    {
        private static readonly string HeaderName = "X-Narvalo-Validate";

        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

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
            var application = sender as HttpApplication;
            var context = application.Context;

            bool validate = context.Request.Headers[HeaderName] != null;
            if (validate) {
                context.Response.Filter = new CaptureStream(context.Response.Filter);
            }
        }

        protected void OnEndRequest(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;
            var context = application.Context;

            var rendererType = context.Request.Headers[HeaderName];
            if (rendererType == null) {
                return;
            }
            var renderer = CreateRenderer(rendererType);

            // TODO: peut-on utiliser context.Response.OutputStream ?
            var captureStream = context.Response.Filter as CaptureStream;
            if (captureStream == null) {
                return;
            }

            captureStream.Rewind();

            IList<ValidationEventArgs> errors = new List<ValidationEventArgs>();
            // FIXME
            //using (var reader = new StreamReader(captureStream.StreamCopy)) {
            //    using (var validator = new XmlValidator(reader)) {
            //        errors = validator.Validate();
            //    }
            //}

            if (errors.Count == 0) {
                return;
            }

            renderer.Render(context, errors);
        }

        // FIXME
        public static IValidationRenderer CreateRenderer(string typeName)
        {
            Requires.NotNullOrEmpty(typeName, "typeName");

            Type rendererType = Type.GetType(typeName, true /* throwOnError */);
            var renderer = Activator.CreateInstance(rendererType) as IValidationRenderer;

            if (renderer == null) {
                throw ExceptionFactory.Argument(typeName,
                    "The specified custom renderer type '{0}' must implement the '{1}' interface",
                    typeName,
                    typeof(IValidationRenderer).FullName);
            }

            return renderer;
        }
    }
}
