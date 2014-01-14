namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public static class Failure
    {
        public static ArgumentException Argument(
            string parameterName,
            string messageFormat,
            params object[] messageArgs)
        {
            return new ArgumentException(Format.CurrentCulture(messageFormat, messageArgs), parameterName);
        }

        public static ArgumentNullException ArgumentNull(
            string parameterName,
            string messageFormat,
            params object[] messageArgs)
        {
            return new ArgumentNullException(parameterName, Format.CurrentCulture(messageFormat, messageArgs));
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
                Format.CurrentCulture(messageFormat, messageArgs));
        }

        public static ConfigurationErrorsException ConfigurationErrors(
            string messageFormat,
            params object[] messageArgs)
        {
            return new ConfigurationErrorsException(Format.CurrentCulture(messageFormat, messageArgs));
        }

        public static KeyNotFoundException KeyNotFound(string messageFormat, params object[] messageArgs)
        {
            return new KeyNotFoundException(Format.CurrentCulture(messageFormat, messageArgs));
        }

        public static ObjectDisposedException ObjectDisposed(
            Type type,
            string messageFormat,
            params object[] messageArgs)
        {
            return new ObjectDisposedException(type.FullName, Format.CurrentCulture(messageFormat, messageArgs));
        }

        public static OperationCanceledException OperationCanceled(
            string messageFormat,
            params object[] messageArgs)
        {
            return new OperationCanceledException(Format.CurrentCulture(messageFormat, messageArgs));
        }

        //public static InvalidEnumArgumentException InvalidEnumArgument(
        //    string parameterName,
        //    int invalidValue,
        //    Type enumClass)
        //{
        //    return new InvalidEnumArgumentException(parameterName, invalidValue, enumClass);
        //}

        public static InvalidOperationException InvalidOperation(
            string messageFormat,
            params object[] messageArgs)
        {
            return new InvalidOperationException(Format.CurrentCulture(messageFormat, messageArgs));
        }

        public static InvalidOperationException InvalidOperation(
            Exception innerException,
            string messageFormat,
            params object[] messageArgs)
        {
            return new InvalidOperationException(Format.CurrentCulture(messageFormat, messageArgs), innerException);
        }

        public static NotSupportedException NotSupported(string messageFormat, params object[] messageArgs)
        {
            return new NotSupportedException(Format.CurrentCulture(messageFormat, messageArgs));
        }
    }
}
