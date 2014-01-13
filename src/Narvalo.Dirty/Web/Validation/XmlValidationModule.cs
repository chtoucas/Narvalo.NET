namespace Narvalo.Web.Validation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Xml;
    using System.Xml.Schema;
    using Narvalo.Xml;

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

            IList<ValidationEventArgs> errors = null;

            using (var reader = new StreamReader(captureStream.StreamCopy)) {
                // FIXME: Créer les bons paramètres.
                var validator = new XmlValidator(new XmlReaderSettings());
                if (!validator.Validate(reader)) {
                    errors = validator.ValidationErrors;
                }
            }

            if (errors == null) {
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

        class CaptureStream : Stream
        {
            Stream _inner;
            Stream _streamCopy;

            public CaptureStream(Stream inner)
            {
                _streamCopy = new MemoryStream();
                _inner = inner;
            }

            public Stream StreamCopy
            {
                get { return _streamCopy; }
            }

            public override bool CanRead
            {
                get { return _inner.CanRead; }
            }

            public override bool CanSeek
            {
                get { return _inner.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return _inner.CanWrite; }
            }

            public override void Flush()
            {
                _inner.Flush();
            }

            public override long Length
            {
                get { return _inner.Length; }
            }

            public override long Position
            {
                get
                {
                    return _inner.Position;
                }
                set
                {
                    _inner.Position = value;
                }
            }

            public void Rewind()
            {
                _streamCopy.Position = 0;
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _inner.Read(buffer, offset, count);
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
