namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;

    public static class Failure // LocalizedException
    {
        public static ArgumentException Argument(
            string parameterName,
            string messageFormat,
            params object[] messageArgs)
        {
            return new ArgumentException(Format_(messageFormat, messageArgs), parameterName);
        }

        public static ArgumentNullException ArgumentNull(
            string parameterName,
            string messageFormat,
            params object[] messageArgs)
        {
            return new ArgumentNullException(parameterName, Format_(messageFormat, messageArgs));
        }

        public static ArgumentOutOfRangeException ArgumentOutOfRange(
            string parameterName,
            object actualValue,
            string messageFormat,
            params object[] messageArgs)
        {
            return new ArgumentOutOfRangeException(
                parameterName,
                actualValue,
                Format_(messageFormat, messageArgs));
        }

        public static ConfigurationErrorsException ConfigurationErrors(
            string messageFormat,
            params object[] messageArgs)
        {
            return new ConfigurationErrorsException(Format_(messageFormat, messageArgs));
        }

        public static KeyNotFoundException KeyNotFound(string messageFormat, params object[] messageArgs)
        {
            return new KeyNotFoundException(Format_(messageFormat, messageArgs));
        }

        public static ObjectDisposedException ObjectDisposed(
            Type type,
            string messageFormat,
            params object[] messageArgs)
        {
            Require.NotNull(type, "type");

            return new ObjectDisposedException(type.FullName, Format_(messageFormat, messageArgs));
        }

        public static OperationCanceledException OperationCanceled(
            string messageFormat,
            params object[] messageArgs)
        {
            return new OperationCanceledException(Format_(messageFormat, messageArgs));
        }

        public static InvalidOperationException InvalidOperation(
            string messageFormat,
            params object[] messageArgs)
        {
            return new InvalidOperationException(Format_(messageFormat, messageArgs));
        }

        public static InvalidOperationException InvalidOperation(
            Exception innerException,
            string messageFormat,
            params object[] messageArgs)
        {
            return new InvalidOperationException(Format_(messageFormat, messageArgs), innerException);
        }

        public static NotSupportedException NotSupported(string messageFormat, params object[] messageArgs)
        {
            return new NotSupportedException(Format_(messageFormat, messageArgs));
        }

        static string Format_(string format, params object[] args)
        {
            return String.Format(CultureInfo.CurrentCulture, format, args);
        }
    }
}
