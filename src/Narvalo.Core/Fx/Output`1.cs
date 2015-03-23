// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    using Narvalo.Internal;

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
    public abstract partial class Output<T>
    {
        private readonly bool _isSuccess;

#if CONTRACTS_FULL // [Ignore] Contract Class and Object Invariants.
        protected Output(bool isSuccess) { _isSuccess = isSuccess; }
#else
        protected Output(bool isSuccess) { _isSuccess = isSuccess;  }
#endif

        /// <summary>
        /// Gets a value identicating whether the output is successful. 
        /// </summary>
        /// <remarks>Most of the time, you don't need to access this property.
        /// You are better off using the rich vocabulary that this class offers.</remarks>
        /// <value><c>true</c> if the output is successful; otherwise <c>false</c>.</value>
        public bool IsSuccess { get { return _isSuccess; } }

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

            // This is not really necessary since the cast to Success_ would fail anyway,
            // but doing so allows us to throw a more meaningful exception and effectively
            // hide the implementation details; the default exception would say something
            // like "Unable to cast a Failure_ type to a Success_ type".
            if (!value.IsSuccess)
            {
                throw new InvalidCastException(Strings_Core.Output_CannotCastFailureToValue);
            }

            return ((Success_)value).Value;
        }

        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Ignore] An explicit conversion operator must be static.")]
        public static explicit operator ExceptionDispatchInfo(Output<T> value)
        {
            Require.NotNull(value, "value");
            Contract.Ensures(Contract.Result<ExceptionDispatchInfo>() != null);

            // This is not really necessary since the cast to Failure_ would fail anyway,
            // but doing so allows us to throw a more meaningful exception and effectively
            // hide the implementation details; the default exception would say something
            // like "Unable to cast a Success_ type to a Failure_ type".
            if (value.IsSuccess)
            {
                throw new InvalidCastException(Strings_Core.Output_CannotCastSuccessToException);
            }

            return ((Failure_)value).ExceptionInfo;
        }

        public abstract void Apply(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure);

        public abstract Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector);

        public abstract TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TResult> caseFailure);

        public abstract void OnSuccess(Action<T> action);

        public abstract void OnFailure(Action<ExceptionDispatchInfo> action);

        public abstract Maybe<T> ValueOrNone();

        public abstract T ValueOrThrow();

        #region Overrides for a bunch of auto-generated (extension) methods (see Output.g.cs).

        public abstract void Apply(Action<T> action);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select",
            Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        public abstract Output<TResult> Select<TResult>(Func<T, TResult> selector);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Then",
            Justification = "[Intentionally] 'Then' is a VB keyword (If...Then...Else), but this is harmless here.")]
        public abstract Output<TResult> Then<TResult>(Output<TResult> other);

        #endregion

        /// <summary>
        /// Obtains the underlying value if any; otherwise the default value of the type T.
        /// </summary>
        /// <returns>The underlying value if any; otherwise the default value of the type T.</returns>
        public T ValueOrDefault()
        {
            return Match(Stubs<T>.Identity, Stubs<T>.AlwaysDefault);
        }

        /// <summary>
        /// Returns the underlying value if any; otherwise <paramref name="other"/>.
        /// </summary>
        /// <param name="other">A default value to be used if if there is no underlying value.</param>
        /// <returns>The underlying value if any; otherwise <paramref name="other"/>.</returns>
        public T ValueOrElse(T other)
        {
            return Match(Stubs<T>.Identity, () => other);
        }

        public T ValueOrElse(Func<T> valueFactory)
        {
            Contract.Requires(valueFactory != null);

            return Match(Stubs<T>.Identity, valueFactory);
        }

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
            Justification = "[Ignore] Weird. This method is called many times.")]
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

            var output = square as Output<Output<T>>.Success_;

            if (output != null)
            {
                return output.Value;
            }
            else
            {
                return η((square as Output<Output<T>>.Failure_).ExceptionInfo);
            }
        }
    }

    /// <content>
    /// Implements the "success" part of the <see cref="Output{T}"/> type.
    /// </content>
    public partial class Output<T>
    {
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

            public override void Apply(Action<T> action)
            {
                Require.NotNull(action, "action");

                action.Invoke(Value);
            }

            public override void Apply(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure)
            {
                Require.NotNull(caseSuccess, "caseSuccess");

                caseSuccess.Invoke(Value);
            }

            public override Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector)
            {
                Require.NotNull(selector, "selector");

                return selector.Invoke(Value);
            }

            public override TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TResult> caseFailure)
            {
                Require.NotNull(caseSuccess, "caseSuccess");

                return caseSuccess.Invoke(Value);
            }

            public override void OnFailure(Action<ExceptionDispatchInfo> action) { }

            public override void OnSuccess(Action<T> action)
            {
                Apply(action);
            }

            public override Output<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                Require.NotNull(selector, "selector");

                return Output<TResult>.η(selector.Invoke(Value));
            }

            public override Output<TResult> Then<TResult>(Output<TResult> other)
            {
                return other;
            }

            public override Maybe<T> ValueOrNone()
            {
                return Maybe.Of(Value);
            }

            public override T ValueOrThrow()
            {
                return Value;
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Success({0})", Value);
            }
        }

        /// <content>
        /// Implements the <see cref="IEquatable{Success}"/> interface.
        /// </content>
        private partial class Success_
        {
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
        }
    }

    /// <content>
    /// Implements the "failure" part of the <see cref="Output{T}"/> type.
    /// </content>
    public partial class Output<T>
    {
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

            public override void Apply(Action<T> action) { }

            public override void Apply(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure)
            {
                Require.NotNull(caseFailure, "caseFailure");

                caseFailure.Invoke(ExceptionInfo);
            }

            public override Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector)
            {
                return Output<TResult>.η(ExceptionInfo);
            }

            public override TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TResult> caseFailure)
            {
                Require.NotNull(caseFailure, "caseFailure");

                return caseFailure.Invoke();
            }

            public override void OnFailure(Action<ExceptionDispatchInfo> action)
            {
                Require.NotNull(action, "action");

                action.Invoke(ExceptionInfo);
            }

            public override void OnSuccess(Action<T> action) { }

            public override Output<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                return Output<TResult>.η(ExceptionInfo);
            }

            public override Output<TResult> Then<TResult>(Output<TResult> other)
            {
                return Output<TResult>.η(ExceptionInfo);
            }

            public override Maybe<T> ValueOrNone()
            {
                return Maybe<T>.None;
            }

            public override T ValueOrThrow()
            {
                ExceptionInfo.Throw();

                return default(T);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Failure({0})", _exceptionInfo);
            }
        }

        /// <content>
        /// Implements the <see cref="IEquatable{Failure}"/> interface.
        /// </content>
        private partial class Failure_
        {
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
        }
    }

#if CONTRACTS_FULL // [Ignore] Contract Class and Object Invariants.

    [ContractClass(typeof(OutputContract<>))]
    public partial class Output<T>
    {
        private partial class Failure_
        {
            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_exceptionInfo != null);
            }
        }
    }

    [ContractClassFor(typeof(Output<>))]
    internal abstract class OutputContract<T> : Output<T>
    {
        protected OutputContract() : base(true) { }

        public override void Apply(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure)
        {
            Contract.Requires(caseSuccess != null);
            Contract.Requires(caseFailure != null);
        }

        public override Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector)
        {
            Contract.Requires(selector != null);

            return default(Output<TResult>);
        }

        public override TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TResult> caseFailure)
        {
            Contract.Requires(caseSuccess != null);
            Contract.Requires(caseFailure != null);

            return default(TResult);
        }

        public override void OnFailure(Action<ExceptionDispatchInfo> action)
        {
            Contract.Requires(action != null);
        }

        public override void OnSuccess(Action<T> action)
        {
            Contract.Requires(action != null);
        }

        public override Output<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Contract.Requires(selector != null);

            return default(Output<TResult>);
        }

        public override Maybe<T> ValueOrNone()
        {
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return default(Maybe<T>);
        }
    }

#endif
}
