// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Represents the output of a computation which may throw exceptions.
    /// An instance of the <see cref="Output{T}"/> class contains either a <c>T</c>
    /// value or the exception state at the point it was thrown.
    /// </summary>
    /// <remarks>
    /// <para>WARNING: We do not catch exceptions throw by any supplied delegate...</para>
    /// <para>This class is not meant to replace the standard exception mechanism.</para>
    /// </remarks>
    /// <typeparam name="T">The underlying type of the value.</typeparam>
    /// <seealso cref="Either{T1, T2}"/>
    /// <seealso cref="Switch{T1, T2}"/>
    /// <seealso cref="VoidOrBreak"/>
    /// <seealso cref="VoidOrError"/>
    public abstract partial class Output<T>
    {
        private readonly bool _isSuccess;

#if CONTRACTS_FULL // Custom ctor visibility for the contract class only.
        protected Output(bool isSuccess)
#else
        private Output(bool isSuccess)
#endif
        { _isSuccess = isSuccess; }

        /// <summary>
        /// Gets a value indicating whether the output is successful. 
        /// </summary>
        /// <remarks>Most of the time, you don't need to access this property.
        /// You are better off using the rich vocabulary that this class offers.</remarks>
        /// <value><c>true</c> if the output is successful; otherwise <c>false</c>.</value>
        public bool IsSuccess { get { return _isSuccess; } }

        #region Explicit casting operators

        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Ignore] An explicit conversion operator must be static.")]
        public static explicit operator Output<T>(T value)
        {
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            return η(value);
        }

        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Ignore] An explicit conversion operator must be static.")]
        public static explicit operator Output<T>(ExceptionDispatchInfo exceptionInfo)
        {
            Contract.Requires(exceptionInfo != null);
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            return η(exceptionInfo);
        }

        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Ignore] An explicit conversion operator must be static.")]
        public static explicit operator T(Output<T> value)
        {
            Require.NotNull(value, "value");

            // We check the value of the property IsSuccess even if this is not really necessary 
            // since a direct cast would have worked too:
            //  return ((Success_)value).Value;
            // but doing so allows us to throw a more meaningful exception and to effectively
            // hide the implementation details; the default exception would say something
            // like "Unable to cast a Failure_ type to a Success_ type".
            if (!value.IsSuccess)
            {
                throw new InvalidCastException(Strings_Core.Output_CannotCastFailureToValue);
            }

            var success = value as Success_;
            Contract.Assume(success != null, "'value' is not of Success_ type");

            return success.Value;
        }

        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Ignore] An explicit conversion operator must be static.")]
        public static explicit operator ExceptionDispatchInfo(Output<T> value)
        {
            Require.NotNull(value, "value");
            Contract.Ensures(Contract.Result<ExceptionDispatchInfo>() != null);

            // We check the value of the property IsSuccess even if this is not really necessary 
            // since a direct cast would have worked too:
            //  return ((Failure_)value).Value;
            // but doing so allows us to throw a more meaningful exception and to effectively
            // hide the implementation details; the default exception would say something
            // like "Unable to cast a Success_ type to a Failure_ type".
            if (value.IsSuccess)
            {
                throw new InvalidCastException(Strings_Core.Output_CannotCastSuccessToException);
            }

            var failure = value as Failure_;
            Contract.Assume(failure != null, "'value' is not of Failure_ type");

            return failure.ExceptionInfo;
        }

        #endregion

        #region Abstract methods

        // Core monad Bind method.
        public abstract Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector);

        // Overrides the 'Select' auto-generated (extension) method (see Output.g.cs).
        // Since Select is a building block, we override it in Failure_ and Success_.
        // Otherwise we would have to call ToValue() or ToExceptionInfo() which imply a casting.
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select",
            Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        public abstract Output<TResult> Select<TResult>(Func<T, TResult> selector);

        #endregion

        public void Invoke(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure)
        {
            Require.NotNull(caseSuccess, "caseSuccess");
            Require.NotNull(caseFailure, "caseFailure");

            if (IsSuccess)
            {
                caseSuccess.Invoke(ToValue());
            }
            else
            {
                caseFailure.Invoke(ToExceptionDispatchInfo());
            }
        }

        public TResult Map<TResult>(Func<T, TResult> caseSuccess, Func<TResult> caseFailure)
        {
            Require.NotNull(caseSuccess, "caseSuccess");
            Require.NotNull(caseFailure, "caseFailure");

            return IsSuccess
                ? caseSuccess.Invoke(ToValue())
                : caseFailure.Invoke();
        }

        public void OnSuccess(Action<T> action)
        {
            Contract.Requires(action != null);

            if (IsSuccess)
            {
                Invoke(action);
            }
        }

        public void OnFailure(Action<ExceptionDispatchInfo> action)
        {
            Require.NotNull(action, "action");

            if (!IsSuccess)
            {
                action.Invoke(ToExceptionDispatchInfo());
            }
        }

        /// <summary>
        /// Obtains the underlying value if any; otherwise the default value of the type T.
        /// </summary>
        /// <returns>The underlying value if any; otherwise the default value of the type T.</returns>
        public T ValueOrDefault()
        {
            return IsSuccess ? ToValue() : default(T);
        }

        /// <summary>
        /// Returns the underlying value if any; otherwise <paramref name="other"/>.
        /// </summary>
        /// <param name="other">A default value to be used if if there is no underlying value.</param>
        /// <returns>The underlying value if any; otherwise <paramref name="other"/>.</returns>
        public T ValueOrElse(T other)
        {
            return IsSuccess ? ToValue() : other;
        }

        public T ValueOrElse(Func<T> valueFactory)
        {
            Require.NotNull(valueFactory, "valueFactory");

            return IsSuccess ? ToValue() : valueFactory.Invoke();
        }

        public Maybe<T> ValueOrNone()
        {
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            if (IsSuccess)
            {
                return Maybe.Of(ToValue());
            }
            else
            {
                return Maybe<T>.None;
            }
        }

        public T ValueOrThrow()
        {
            if (IsSuccess)
            {
                return ToValue();
            }
            else
            {
                ToExceptionDispatchInfo().Throw();

                return default(T);
            }
        }

        #region Overrides a bunch of auto-generated (extension) methods (see Output.g.cs).

        public void Invoke(Action<T> action)
        {
            Require.NotNull(action, "action");

            if (IsSuccess)
            {
                action.Invoke(ToValue());
            }
        }

        public Output<TResult> Then<TResult>(Output<TResult> other)
        {
            return IsSuccess
                ? other
                : Output<TResult>.η(ToExceptionDispatchInfo());
        }

        #endregion

        #region Core Monad methods

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [SuppressMessage("Gendarme.Rules.Naming", "UseCorrectCasingRule",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Output<T> η(T value)
        {
            return new Success_(value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [SuppressMessage("Gendarme.Rules.Naming", "UseCorrectCasingRule",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUncalledPrivateCodeRule",
            Justification = "[Ignore] Weird. This method does have plenty of callers inside the assembly.")]
        internal static Output<T> η(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, "exceptionInfo");

            return new Failure_(exceptionInfo);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [SuppressMessage("Gendarme.Rules.Naming", "UseCorrectCasingRule",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Output<T> μ(Output<Output<T>> square)
        {
            Require.NotNull(square, "square");

            if (square.IsSuccess)
            {
                return square.ToValue();
            }
            else
            {
                return η(square.ToExceptionDispatchInfo());
            }
        }

        #endregion

        /// <summary>
        /// Obtains the enclosed value.
        /// </summary>
        /// <remarks>
        /// Any access to this method must be protected by checking before that <see cref="IsSuccess"/> 
        /// is <see langword="true"/>.
        /// </remarks>
        internal T ToValue()
        {
            Contract.Requires(IsSuccess);

            var success = this as Success_;
            Contract.Assume(success != null, "'this' is not of Success_ type");

            return success.Value;
        }

        /// <summary>
        /// Obtains the enclosed exception state.
        /// </summary>
        /// <remarks>
        /// Any access to this method must be protected by checking before that <see cref="IsSuccess"/> 
        /// is <see langword="false"/>.
        /// </remarks>
        internal ExceptionDispatchInfo ToExceptionDispatchInfo()
        {
            Contract.Requires(!IsSuccess);

            var failure = this as Failure_;
            Contract.Assume(failure != null, "'this' is not of Failure_ type");

            return failure.ExceptionInfo;
        }

        /// <summary>
        /// Represents the "success" part of the <see cref="Output{T}"/> type.
        /// </summary>
        private sealed partial class Success_ : Output<T>, IEquatable<Success_>
        {
            private readonly T _value;

            public Success_(T value)
                : base(true)
            {
                _value = value;
            }

            internal T Value { get { return _value; } }

            public override Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector)
            {
                Require.NotNull(selector, "selector");

                return selector.Invoke(Value);
            }

            public override Output<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                Require.NotNull(selector, "selector");

                return Output<TResult>.η(selector.Invoke(Value));
            }

            public bool Equals(Success_ other)
            {
                if (other == this)
                {
                    return true;
                }

                if (other == null)
                {
                    return false;
                }

                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Success_);
            }

            public override int GetHashCode()
            {
                return Value == null ? 0 : EqualityComparer<T>.Default.GetHashCode(Value);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Success({0})", Value);
            }
        }

        /// <summary>
        /// Represents the "failure" part of the <see cref="Output{T}"/> type.
        /// </summary>
        private sealed partial class Failure_ : Output<T>, IEquatable<Failure_>
        {
            private readonly ExceptionDispatchInfo _exceptionInfo;

            public Failure_(ExceptionDispatchInfo exceptionInfo)
                : base(false)
            {
                Contract.Requires(exceptionInfo != null);

                _exceptionInfo = exceptionInfo;
            }

            internal ExceptionDispatchInfo ExceptionInfo
            {
                get
                {
                    Contract.Ensures(Contract.Result<ExceptionDispatchInfo>() != null);

                    return _exceptionInfo;
                }
            }

            public override Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector)
            {
                return Output<TResult>.η(ExceptionInfo);
            }

            public override Output<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                return Output<TResult>.η(ExceptionInfo);
            }

            public bool Equals(Failure_ other)
            {
                if (other == this)
                {
                    return true;
                }

                if (other == null)
                {
                    return false;
                }

                return EqualityComparer<ExceptionDispatchInfo>.Default.Equals(_exceptionInfo, other._exceptionInfo);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Failure_);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<ExceptionDispatchInfo>.Default.GetHashCode(_exceptionInfo);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Failure({0})", _exceptionInfo);
            }
        }
    }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

    // In real world, only Success_ and Failure_ can inherit from Output.
    // Adding the following object invariants on Output<T>:
    //  (IsSuccess && (this as Success_) != null) || (this as Failure_) != null
    // should make unecessary any call to Contract.Assume but I have not been able to make this work.
    [ContractClass(typeof(OutputContract<>))]
    public partial class Output<T>
    {
        private partial class Success_
        {
            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(IsSuccess);
            }
        }

        private partial class Failure_
        {
            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(!IsSuccess);
                Contract.Invariant(_exceptionInfo != null);
            }
        }
    }

    [ContractClassFor(typeof(Output<>))]
    internal abstract class OutputContract<T> : Output<T>
    {
        protected OutputContract() : base(true) { }

        public override Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector)
        {
            Contract.Requires(selector != null);

            return default(Output<TResult>);
        }

        public override Output<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Contract.Requires(selector != null);

            return default(Output<TResult>);
        }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return String.Empty;
        }
    }

#endif
}
