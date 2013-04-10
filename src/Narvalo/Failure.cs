namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Narvalo.Resources;

    /// <summary>
    /// Utility class for creating and unwrapping <see cref="Exception"/> instances.
    /// </summary>
    public static class Failure
    {
        /// <summary>
        /// Creates an <see cref="ArgumentException"/> with the provided properties.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static ArgumentException Argument(string messageFormat, params object[] messageArgs)
        {
            return new ArgumentException(Format_(messageFormat, messageArgs));
        }

        /// <summary>
        /// Creates an <see cref="ArgumentException"/> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static ArgumentException Argument(
            string parameterName,
            string messageFormat,
            params object[] messageArgs)
        {
            return new ArgumentException(Format_(messageFormat, messageArgs), parameterName);
        }

        /// <summary>
        /// Creates an <see cref="ArgumentNullException"/> with the provided properties.
        /// </summary>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "The purpose of this API is to return an error for properties")]
        public static ArgumentNullException PropertyNull()
        {
            return new ArgumentNullException("value");
        }

        /// <summary>
        /// Creates an <see cref="ArgumentNullException"/> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static ArgumentNullException ArgumentNull(string parameterName)
        {
            return new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Creates an <see cref="ArgumentNullException"/> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static ArgumentNullException ArgumentNull(
            string parameterName,
            string messageFormat,
            params object[] messageArgs)
        {
            return new ArgumentNullException(parameterName, Format_(messageFormat, messageArgs));
        }

        /// <summary>
        /// Creates an <see cref="ArgumentException"/> with a default message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static ArgumentException ArgumentEmpty(string parameterName)
        {
            return Failure.Argument(parameterName, SR.Failure_IsEmpty, parameterName);
        }

        /// <summary>
        /// Creates an <see cref="ArgumentOutOfRangeException"/> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <param name="actualValue">The value of the argument that causes this exception.</param>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
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

        /// <summary>
        /// Creates an <see cref="KeyNotFoundException"/> with a message saying that the key was not found.
        /// </summary>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static KeyNotFoundException KeyNotFound()
        {
            return new KeyNotFoundException();
        }

        /// <summary>
        /// Creates an <see cref="KeyNotFoundException"/> with a message saying that the key was not found.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static KeyNotFoundException KeyNotFound(string messageFormat, params object[] messageArgs)
        {
            return new KeyNotFoundException(Format_(messageFormat, messageArgs));
        }

        public static ObjectDisposedException ObjectDisposed(Type type)
        {
            Requires.NotNull(type, "type");

            return new ObjectDisposedException(type.FullName);
        }

        /// <summary>
        /// Creates an <see cref="ObjectDisposedException"/> initialized according to guidelines.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static ObjectDisposedException ObjectDisposed(
            string messageFormat,
            params object[] messageArgs)
        {
            // Pass in null, not disposedObject.GetType().FullName as per the above guideline
            return new ObjectDisposedException(null, Format_(messageFormat, messageArgs));
        }

        /// <summary>
        /// Creates an <see cref="OperationCanceledException"/> initialized with the provided parameters.
        /// </summary>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static OperationCanceledException OperationCanceled()
        {
            return new OperationCanceledException();
        }

        /// <summary>
        /// Creates an <see cref="OperationCanceledException"/> initialized with the provided parameters.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static OperationCanceledException OperationCanceled(
            string messageFormat,
            params object[] messageArgs)
        {
            return new OperationCanceledException(Format_(messageFormat, messageArgs));
        }

        /// <summary>
        /// Creates an <see cref="InvalidEnumArgumentException"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <param name="invalidValue">The value of the argument that failed.</param>
        /// <param name="enumClass">A <see cref="Type"/> that represents the enumeration class with the valid values.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static InvalidEnumArgumentException InvalidEnumArgument(
            string parameterName,
            int invalidValue,
            Type enumClass)
        {
            return new InvalidEnumArgumentException(parameterName, invalidValue, enumClass);
        }

        /// <summary>
        /// Creates an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static InvalidOperationException InvalidOperation(
            string messageFormat,
            params object[] messageArgs)
        {
            return new InvalidOperationException(Format_(messageFormat, messageArgs));
        }

        /// <summary>
        /// Creates an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="innerException">Inner exception</param>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static InvalidOperationException InvalidOperation(
            Exception innerException,
            string messageFormat,
            params object[] messageArgs)
        {
            return new InvalidOperationException(Format_(messageFormat, messageArgs), innerException);
        }

        /// <summary>
        /// Creates an <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        public static NotSupportedException NotSupported(string messageFormat, params object[] messageArgs)
        {
            return new NotSupportedException(Format_(messageFormat, messageArgs));
        }

        /// <summary>
        /// Formats the specified resource string using <see cref="M:CultureInfo.CurrentCulture"/>.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The formatted string.</returns>
        static string Format_(string format, params object[] args)
        {
            return String.Format(CultureInfo.CurrentCulture, format, args);
        }
    }
}
