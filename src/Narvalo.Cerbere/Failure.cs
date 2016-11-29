// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.IO;

    using static System.Diagnostics.Contracts.Contract;

    public static class Failure
    {
        public static ArgumentException Argument(string message, string parameterName)
        {
            Expect.NotNull(message);
            Ensures(Result<ArgumentException>() != null);

            return new ArgumentException(message, parameterName);
        }

        public static ArgumentNullException ArgumentNull(string message, string parameterName)
        {
            Expect.NotNull(message);
            Ensures(Result<ArgumentNullException>() != null);

            return new ArgumentNullException(parameterName, message);
        }

        public static ArgumentOutOfRangeException ArgumentOutOfRange(string message, string parameterName)
        {
            Expect.NotNull(message);
            Ensures(Result<ArgumentOutOfRangeException>() != null);

            return new ArgumentOutOfRangeException(parameterName, message);
        }

        public static InvalidDataException InvalidData()
        {
            Ensures(Result<InvalidDataException>() != null);

            return new InvalidDataException();
        }

        public static InvalidDataException InvalidData(string message)
        {
            Expect.NotNull(message);
            Ensures(Result<InvalidDataException>() != null);

            return new InvalidDataException(message);
        }

        public static InvalidOperationException InvalidOperation()
        {
            Ensures(Result<InvalidOperationException>() != null);

            return new InvalidOperationException();
        }

        public static InvalidOperationException InvalidOperation(string message)
        {
            Expect.NotNull(message);
            Ensures(Result<InvalidOperationException>() != null);

            return new InvalidOperationException(message);
        }

        public static NotImplementedException NotImplemented()
        {
            Ensures(Result<NotImplementedException>() != null);

            return new NotImplementedException();
        }

        public static NotImplementedException NotImplemented(string message)
        {
            Expect.NotNull(message);
            Ensures(Result<NotImplementedException>() != null);

            return new NotImplementedException(message);
        }

        public static NotSupportedException NotSupported()
        {
            Ensures(Result<NotSupportedException>() != null);

            return new NotSupportedException();
        }

        public static NotSupportedException NotSupported(string message)
        {
            Expect.NotNull(message);
            Ensures(Result<NotSupportedException>() != null);

            return new NotSupportedException(message);
        }

        public static ObjectDisposedException ObjectDisposed(Type type)
        {
            Expect.NotNull(type);
            Ensures(Result<ObjectDisposedException>() != null);

            return new ObjectDisposedException(type.FullName);
        }
    }
}
