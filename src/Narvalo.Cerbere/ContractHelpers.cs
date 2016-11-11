// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides Code Contracts abbreviators and helpers.
    /// </summary>
    public static class ContractHelpers
    {
        /// <summary>
        /// This method allows to state explicitly the object invariance.
        /// </summary>
        /// <remarks>To be recognized by CCCheck this method must be named exactly "AssumeInvariant".</remarks>
        /// <typeparam name="T">The underlying type of the object.</typeparam>
        /// <param name="obj">The invariant object.</param>
        [Pure]
        [DebuggerHidden]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void AssumeInvariant<T>(T obj) { }

        /// <summary>
        /// Asserts that a point of execution is unreachable.
        /// </summary>
        /// <remarks>Adapted from <see cref="!:https://blogs.msdn.microsoft.com/francesco/2014/09/12/how-to-use-cccheck-to-prove-no-case-is-forgotten/"/>.</remarks>
        /// <example>
        /// <code>
        /// switch (myEnum)
        /// {
        ///     case MyEnum.DefinedValue:
        ///         return "DefinedValue";
        ///     default:
        ///         throw Acknowledge.Unreachable("Found a missing case in the switch.");
        /// }
        /// </code>
        /// </example>
        /// <param name="reason">The error message to use if the point of execution is reached.</param>
        /// <returns>A new instance of the <see cref="NotSupportedException"/> class with the specified error message.</returns>
        [DebuggerHidden]
        [ContractVerification(false)]
        public static NotSupportedException Unreachable(string reason)
        {
            Contract.Requires(false);

            return new NotSupportedException(reason);
        }

        /// <summary>
        /// Asserts that a point of execution is unreachable.
        /// </summary>
        /// <remarks>Adapted from <see cref="!:https://blogs.msdn.microsoft.com/francesco/2014/09/12/how-to-use-cccheck-to-prove-no-case-is-forgotten/"/>.</remarks>
        /// <example>
        /// <code>
        /// switch (myEnum)
        /// {
        ///     case MyEnum.DefinedValue:
        ///         return "DefinedValue";
        ///     default:
        ///         throw Acknowledge.Unreachable(new MyException("Found a missing case in the switch."));
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="TException">The type of <paramref name="exception"/>.</typeparam>
        /// <param name="exception">The exception the caller wish to throw back from here.</param>
        /// <returns>The untouched input <paramref name="exception"/>.</returns>
        [DebuggerHidden]
        [ContractVerification(false)]
        public static TException Unreachable<TException>(TException exception)
            where TException : Exception
        {
            Contract.Requires(false);

            return exception;
        }
    }
}
