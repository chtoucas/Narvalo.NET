// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides Code Contracts abbreviators and helpers.
    /// </summary>
    [DebuggerStepThrough]
    public static class Check
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="testCondition"></param>
        /// <remarks>
        /// To be used instead of
        /// <code>
        ///     Contract.Assert(testCondition);
        ///     Debug.Assert(testCondition);
        /// </code>
        /// </remarks>
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void True(bool testCondition) => Demand.True(testCondition);

        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void False(bool testCondition) => Demand.True(!testCondition);

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

        [DebuggerHidden]
        [ContractVerification(false)]
        public static ControlFlowException Unreachable()
        {
            Contract.Requires(false);

            return new ControlFlowException();
        }

        /// <summary>
        /// Asserts that a point of execution is unreachable.
        /// </summary>
        /// <remarks>
        /// Adapted from <see cref="!:https://blogs.msdn.microsoft.com/francesco/2014/09/12/how-to-use-cccheck-to-prove-no-case-is-forgotten/"/>.
        ///
        /// Unfortunately, CCCheck will still complains with 'CodeContracts: requires unreachable'.
        /// You can safely suppress this warning and, later on, if you reach the "unreachable"
        /// point, CCCheck will still produce a different warning:
        /// 'This requires, always leading to an error, may be reachable. Are you missing an enum case?'
        /// </remarks>
        /// <example>
        /// <code>
        /// switch (myEnum)
        /// {
        ///     case MyEnum.DefinedValue:
        ///         return "DefinedValue";
        ///     default:
        ///         throw Check.Unreachable("Found a missing case in a switch.");
        /// }
        /// </code>
        /// </example>
        /// <param name="rationale">The error message to use if the point of execution is reached.</param>
        /// <returns>A new instance of the <see cref="ControlFlowException"/> class with the specified error message.</returns>
        [DebuggerHidden]
        [ContractVerification(false)]
        public static ControlFlowException Unreachable(string rationale)
        {
            Contract.Requires(false);

            return new ControlFlowException(rationale);
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
        ///         throw Check.Unreachable(new MyException("Found a missing case in a switch."));
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
