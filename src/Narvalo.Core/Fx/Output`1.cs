﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;

    using Narvalo.Internal;

    // WARNING: We do not catch exception.
    public abstract partial class Output<T>
    {
        private Output() { }

        public abstract void Apply(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure);

        public abstract Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector);

        public abstract TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TResult> caseFailure);

        public abstract T ValueOrThrow();

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
            Justification = "Standard naming convention from mathematics. Only used internally.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Output<T> η(T value)
        {
            return Output<T>.Success.η(value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics. Only used internally.")]
        internal static Output<T> μ(Output<Output<T>> square)
        {
            Require.NotNull(square, "square");
            Assume.Invariant(square);

            var output = square as Output<Output<T>>.Success;

            if (output != null)
            {
                return output.Value;
            }
            else
            {
                return Failure.η((square as Output<Output<T>>.Failure).ExceptionInfo);
            }
        }
    }

    /// <content>
    /// Implements the success part of the <see cref="Output{T}"/> type.
    /// </content>
    public partial class Output<T>
    {
        /// <summary>
        /// Represents the success part of the <see cref="Output{T}"/> type.
        /// </summary>
        internal sealed partial class Success : Output<T>, IEquatable<Success>
        {
            private readonly T _value;

            private Success(T value)
            {
                _value = value;
            }

            internal T Value { get { return _value; } }

            public override Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector)
            {
                Require.NotNull(selector, "selector");

                return selector.Invoke(Value);
            }

            public override void Apply(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure)
            {
                Require.NotNull(caseSuccess, "caseSuccess");

                caseSuccess.Invoke(Value);
            }

            public override TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TResult> caseFailure)
            {
                Require.NotNull(caseSuccess, "caseSuccess");

                return caseSuccess.Invoke(Value);
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
        internal partial class Success
        {
            public bool Equals(Success other)
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
                return Equals(obj as Success);
            }

            public override int GetHashCode()
            {
                return Value == null ? 0 : EqualityComparer<T>.Default.GetHashCode(Value);
            }
        }
    }

    /// <content>
    /// Implements the failure part of the <see cref="Output{T}"/> type.
    /// </content>
    public partial class Output<T>
    {
        /// <summary>
        /// Represents the failure part of the <see cref="Output{T}"/> type.
        /// </summary>
        internal sealed partial class Failure : Output<T>, IEquatable<Failure>
        {
            private readonly ExceptionDispatchInfo _exceptionInfo;

            private Failure(ExceptionDispatchInfo exceptionInfo)
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
                return Output<TResult>.Failure.η(_exceptionInfo);
            }

            public override void Apply(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure)
            {
                Require.NotNull(caseFailure, "caseFailure");

                caseFailure.Invoke(ExceptionInfo);
            }

            public override TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TResult> caseFailure)
            {
                Require.NotNull(caseFailure, "caseFailure");

                return caseFailure.Invoke();
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

            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
                Justification = "Standard naming convention from mathematics. Only used internally.")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static Output<T> η(ExceptionDispatchInfo exceptionInfo)
            {
                Require.NotNull(exceptionInfo, "exceptionInfo");

                return new Failure(exceptionInfo);
            }
        }

        /// <content>
        /// Implements the <see cref="IEquatable{Failure}"/> interface.
        /// </content>
        internal partial class Failure
        {
            public bool Equals(Failure other)
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
                return Equals(obj as Failure);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<ExceptionDispatchInfo>.Default.GetHashCode(_exceptionInfo);
            }
        }
    }


#if CONTRACTS_FULL

    public abstract partial class Output<T>
    {
        internal partial class Failure
        {
            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_exceptionInfo != null);
            }
        }
    }

#endif
}