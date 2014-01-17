namespace Narvalo.Web
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Xml;
    using System.Xml.Schema;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo.Xml;

    public class XmlValidationModule : IHttpModule
    {
        static readonly string HeaderName_ = "X-Narvalo-Validate";

        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Require.NotNull(context, "context");

            context.BeginRequest += OnBeginRequest_;
            context.EndRequest += OnEndRequest_;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(XmlValidationModule));
        }

        void OnBeginRequest_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            bool validate = app.Request.Headers[HeaderName_] != null;
            if (validate) {
                app.Response.Filter = new CaptureStream(app.Response.Filter);
            }
        }

        void OnEndRequest_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            var rendererType = app.Request.Headers[HeaderName_];
            if (rendererType == null) {
                return;
            }
            var renderer = CreateRenderer_(rendererType);

            // REVIEW: Peut-on utiliser context.Response.OutputStream ?
            var captureStream = app.Response.Filter as CaptureStream;
            if (captureStream == null) {
                return;
            }

            captureStream.Rewind();

            IReadOnlyCollection<ValidationEventArgs> errors = null;

            using (var reader = new StreamReader(captureStream.StreamCopy)) {
                // FIXME: Utiliser les bons paramètres.
                var validator = new XmlValidator(new XmlReaderSettings());
                if (!validator.Validate(reader)) {
                    errors = validator.ValidationErrors;
                }
            }

            if (errors != null) {
                renderer.Render(app.Context, errors);
            }
        }

        // FIXME
        static IXmlValidationRenderer CreateRenderer_(string typeName)
        {
            Require.NotNullOrEmpty(typeName, "typeName");

            Type rendererType = Type.GetType(typeName, true /* throwOnError */);
            var renderer = Activator.CreateInstance(rendererType) as IXmlValidationRenderer;

            if (renderer == null) {
                throw Failure.Argument(typeName,
                    "The specified custom renderer type '{0}' must implement the '{1}' interface",
                    typeName,
                    typeof(IXmlValidationRenderer).FullName);
            }

            return renderer;
        }

        class CaptureStream : Stream
        {
            Stream _inner;
            Stream _streamCopy;

            public CaptureStream(Stream inner)
            {
                _streamCopy = new MemoryStream();
                _inner = inner;
            }

            public Stream StreamCopy { get { return _streamCopy; } }
            public override bool CanRead { get { return _inner.CanRead; } }
            public override bool CanSeek { get { return _inner.CanSeek; } }
            public override bool CanWrite { get { return _inner.CanWrite; } }
            public override long Length { get { return _inner.Length; } }

            public override long Position
            {
                get { return _inner.Position; }
                set { _inner.Position = value; }
            }

            public override void Flush()
            {
                _inner.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _inner.Read(buffer, offset, count);
            }

            public void Rewind()
            {
                _streamCopy.Position = 0;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _inner.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                _inner.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                _streamCopy.Write(buffer, offset, count);
                _inner.Write(buffer, offset, count);
            }
        }
    }
}
