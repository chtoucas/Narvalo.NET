// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Xml;

    public class CachingXmlResolver : XmlResolver
    {
        private const int BUFFER_LENGTH = 32768;

        private static IDictionary<Uri, byte[]> s_Cache = new Dictionary<Uri, byte[]>();
        private static object s_Lock = new Object();

        private readonly XmlResolver _inner;

        public CachingXmlResolver(XmlResolver inner)
            : base()
        {
            Require.NotNull(inner, "inner");

            _inner = inner;
        }

        public override ICredentials Credentials
        {
            set { _inner.Credentials = value; }
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            byte[] content;

            if (s_Cache.ContainsKey(absoluteUri))
            {
                content = s_Cache[absoluteUri];
            }
            else
            {
                lock (s_Lock)
                {
                    if (s_Cache.ContainsKey(absoluteUri))
                    {
                        content = s_Cache[absoluteUri];
                    }
                    else
                    {
                        content = GetEntityCore(absoluteUri, role, ofObjectToReturn);
                        s_Cache.Add(absoluteUri, content);
                    }
                }
            }

            return new MemoryStream(content);
        }

        protected virtual byte[] GetEntityCore(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            byte[] content;
            using (var stream = (Stream)_inner.GetEntity(absoluteUri, role, ofObjectToReturn))
            {
                content = Slurp_(stream, BUFFER_LENGTH);
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

        private static byte[] Slurp_(Stream stream, int bufferLength)
        {
            byte[] buffer = new byte[bufferLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
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
            byte[] retval = new byte[read];
            Array.Copy(buffer, retval, read);
            return retval;
        }
    }
}
