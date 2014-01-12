namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;

    public static class ExceptionFactory
    {
        public static ArgumentException Argument(string messageFormat, params object[] messageArgs)
        {
            return new ArgumentException(Format_(messageFormat, messageArgs));
        }

        public static ArgumentException Argument(
            string parameterName,
            string messageFormat,
            params object[] messageArgs)
        {
            return new ArgumentException(Format_(messageFormat, messageArgs), parameterName);
        }

        //[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        //    Justification = "The purpose of this API is to return an error for extension methods.")]
        //public static ArgumentNullException ObjectNull()
        //{
        //    return new ArgumentNullException("this");
        //}

        //[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        //    Justification = "The purpose of this API is to return an error for properties.")]
        //public static ArgumentNullException PropertyNull()
        //{
        //    return new ArgumentNullException("value");
        //}

        //public static ArgumentNullException ArgumentNull(string parameterName)
        //{
        //    return new ArgumentNullException(parameterName);
        //}

        public static ArgumentNullException ArgumentNull(
            string parameterName,
            string messageFormat,
            params object[] messageArgs)
        {
            return new ArgumentNullException(parameterName, Format_(messageFormat, messageArgs));
        }

        public static ArgumentException ArgumentEmpty(string parameterName)
        {
            return ExceptionFactory.Argument(parameterName, SR.Failure_IsEmpty, parameterName);
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

        public static KeyNotFoundException KeyNotFound()
        {
            return new KeyNotFoundException();
        }

        public static KeyNotFoundException KeyNotFound(string messageFormat, params object[] messageArgs)
        {
            return new KeyNotFoundException(Format_(messageFormat, messageArgs));
        }

        public static ObjectDisposedException ObjectDisposed(Type type)
        {
            Requires.NotNull(type, "type");

            return new ObjectDisposedException(type.FullName);
        }

        public static ObjectDisposedException ObjectDisposed(
            string messageFormat,
            params object[] messageArgs)
        {
            // Pass in null, not disposedObject.GetType().FullName as per the above guideline
            return new ObjectDisposedException(null, Format_(messageFormat, messageArgs));
        }

        public static OperationCanceledException OperationCanceled()
        {
            return new OperationCanceledException();
        }

        public static OperationCanceledException OperationCanceled(
            string messageFormat,
            params object[] messageArgs)
        {
            return new OperationCanceledException(Format_(messageFormat, messageArgs));
        }

        public static InvalidEnumArgumentException InvalidEnumArgument(
            string parameterName,
            int invalidValue,
            Type enumClass)
        {
            return new InvalidEnumArgumentException(parameterName, invalidValue, enumClass);
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

        #region > Membres privés <

        static string Format_(string format, params object[] args)
        {
            return String.Format(CultureInfo.CurrentCulture, format, args);
        }

        #endregion
    }
}
