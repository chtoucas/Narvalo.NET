// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    ///// <summary>
    ///// Provides helper methods to check for conditions on parameters.
    ///// </summary>
    ///// <remarks>
    ///// <para>The methods WON'T be recognized by FxCop as parameter validators
    ///// against <see langword="null"/> value.</para>
    ///// <para>The methods MUST appear after all Code Contracts.</para>
    ///// <para>If a condition does not hold, an unrecoverable exception is thrown
    ///// in debug builds.</para>
    ///// <para>This class MUST NOT be used in place of proper validation routines of public
    ///// arguments but is only useful in very specialized use cases. Be wise.
    ///// Personally, I can only see one situation where these helpers make sense:
    ///// for protected overridden methods in a sealed class when the base method
    ///// declares a contract (otherwise you should use <see cref="Promise"/>),
    ///// when you know for certain that all callers will satisfy the condition
    ///// and most certainly when you own all base classes. As you can see, that
    ///// makes a lot of prerequisites...
    ///// </para>
    ///// </remarks>
    // To be used instead of
    //      Contract.Assert(testCondition);
    //      Debug.Assert(testCondition);
    [DebuggerStepThrough]
    public static class Check
    {
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void True(bool testCondition) => Demand.True(testCondition);

        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void False(bool testCondition) => Demand.True(!testCondition);

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
