namespace Narvalo.Internal
{
    using System;
    using System.IO;

    static class StreamUtility
    {
        public static byte[] Slurp(Stream stream, int bufferLength)
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
