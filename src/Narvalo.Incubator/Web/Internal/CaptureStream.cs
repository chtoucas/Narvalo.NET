namespace Narvalo.Web.Internal
{
    using System.IO;

    class CaptureStream : Stream
    {
        private Stream _inner;
        private Stream _streamCopy;

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
