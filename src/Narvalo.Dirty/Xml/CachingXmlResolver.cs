namespace Narvalo.Xml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Xml;

    public class CachingXmlResolver : XmlResolver
    {
        const int BufferLength_ = 32768;

        static IDictionary<Uri, byte[]> Cache_ = new Dictionary<Uri, byte[]>();
        static object Lock_ = new Object();

        readonly XmlResolver _inner;

        public CachingXmlResolver(XmlResolver inner)
            : base()
        {
            Requires.NotNull(inner, "inner");

            _inner = inner;
        }

        public override ICredentials Credentials
        {
            set { _inner.Credentials = value; }
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            byte[] content;

            if (Cache_.ContainsKey(absoluteUri)) {
                content = Cache_[absoluteUri];
            }
            else {
                lock (Lock_) {
                    if (Cache_.ContainsKey(absoluteUri)) {
                        content = Cache_[absoluteUri];
                    }
                    else {
                        content = GetEntityCore(absoluteUri, role, ofObjectToReturn);
                        Cache_.Add(absoluteUri, content);
                    }
                }
            }

            return new MemoryStream(content);
        }

        protected virtual byte[] GetEntityCore(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            byte[] content;
            using (var stream = (Stream)_inner.GetEntity(absoluteUri, role, ofObjectToReturn)) {
                content = Slurp_(stream, BufferLength_);
            }
            return content;
        }

        //Stream GetStream_(Uri absoluteUri)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(absoluteUri);
        //    request.Proxy = new WebProxy(proxy.Server, proxy.Port);
        //    request.Proxy.Credentials = new NetworkCredential(proxy.Username, proxy.Password, proxy.Domain);

        //    HttpWebResponse @this = (HttpWebResponse)request.GetResponse();
        //    return @this.GetResponseStream();
        //}

        static byte[] Slurp_(Stream stream, int bufferLength)
        {
            byte[] buffer = new byte[bufferLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0) {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length) {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1) {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[2 * buffer.Length];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }

            // Buffer is now too big. Shrink it.
            byte[] result = new byte[read];
            Array.Copy(buffer, result, read);
            return result;
        }
    }
}
