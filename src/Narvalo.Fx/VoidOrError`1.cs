// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    // Friendly version of Either<TError, Unit>.
    public partial class VoidOrError<TError> : Internal.IMatcher<TError>
    {
        private VoidOrError() { }

        private VoidOrError(bool isError)
        {
            IsError = isError;
        }

        public bool IsError { get; }

        public bool IsVoid => !IsError;

        public virtual TError Error
        {
            get
            {
                throw new InvalidOperationException("XXX");
            }
        }

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return "Void";
        }

        [DebuggerDisplay("Error")]
        [DebuggerTypeProxy(typeof(VoidOrError<>.Error_.DebugView))]
        private sealed partial class Error_ : VoidOrError<TError>
        {
            private readonly TError _error;

            public Error_(TError error)
                : base(true)
            {
                Demand.NotNullUnconstrained(error);

                _error = error;
            }

            public override TError Error
            {
                get { Warrant.NotNullUnconstrained<TError>(); return _error; }
            }

            public override TResult Match<TResult>(Func<TError, TResult> caseError, Func<TResult> caseVoid)
            {
                Require.NotNull(caseError, nameof(caseError));

                return caseError.Invoke(Error);
            }

            public override TResult Match<TResult>(Func<TError, TResult> caseError, TResult caseVoid)
            {
                Require.NotNull(caseError, nameof(caseError));

                return caseError.Invoke(Error);
            }

            public override TResult Coalesce<TResult>(
                Func<TError, bool> predicate,
                Func<TError, TResult> selector,
                Func<TResult> otherwise)
            {
                Require.NotNull(otherwise, nameof(otherwise));

                return predicate.Invoke(Error) ? selector.Invoke(Error) : otherwise.Invoke();
            }

            public override TResult Coalesce<TResult>(Func<TError, bool> predicate, TResult then, TResult other)
            {
                Require.NotNull(predicate, nameof(predicate));

                return predicate.Invoke(Error) ? then : other;
            }

            public override void Do(Func<TError, bool> predicate, Action<TError> action, Action otherwise)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate.Invoke(Error))
                {
                    action.Invoke(Error);
                }
                else
                {
                    otherwise.Invoke();
                }
            }

            public override void Do(Action<TError> caseError, Action caseVoid)
            {
                Require.NotNull(caseError, nameof(caseError));

                caseError.Invoke(Error);
            }

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return "Error(" + Error.ToString() + ")";
            }

            /// <summary>
            /// Represents a debugger type proxy for <see cref="VoidOrError{TError}.Error_"/>.
            /// </summary>
            [ContractVerification(false)] // Debugger-only code.
            [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
            private sealed class DebugView
            {
                private readonly Error_ _inner;

                public DebugView(Error_ inner)
                {
                    _inner = inner;
                }

                public TError Message => _inner.Error;
            }
        }
    }

    // Provides the core Monad methods.
    public partial class VoidOrError<TError>
    {
        public VoidOrError<TResult> Bind<TResult>(Func<TError, VoidOrError<TResult>> selectorM)
        {
            Require.NotNull(selectorM, nameof(selectorM));

            return IsError ? selectorM.Invoke(Error) : VoidOrError<TResult>.Void;
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static VoidOrError<TError> η(TError value)
        {
            Require.NotNullUnconstrained(value, nameof(value));
            Warrant.NotNull<VoidOrError<TError>>();

            return new VoidOrError<TError>.Error_(value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static VoidOrError<TError> μ(VoidOrError<VoidOrError<TError>> square)
            => square.IsError ? square.Error : VoidOrError<TError>.Void;
    }

    // Provides the core MonadOr methods.
    public partial class VoidOrError<TError>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly VoidOrError<TError> s_Void = new VoidOrError<TError>();

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static VoidOrError<TError> Void
        {
            get { Warrant.NotNull<VoidOrError<TError>>(); return s_Void; }
        }

        public VoidOrError<TError> OrElse(VoidOrError<TError> other) => IsVoid ? other : this;
    }

    // Implements the Internal.IMatcher<TError> interface.
    public partial class VoidOrError<TError>
    {
        public virtual TResult Match<TResult>(Func<TError, TResult> caseError, Func<TResult> caseVoid)
        {
            Require.NotNull(caseVoid, nameof(caseVoid));

            return caseVoid.Invoke();
        }

        public virtual TResult Match<TResult>(Func<TError, TResult> caseError, TResult caseVoid)
            => caseVoid;

        public virtual TResult Coalesce<TResult>(
            Func<TError, bool> predicate,
            Func<TError, TResult> selector,
            Func<TResult> otherwise)
        {
            Require.NotNull(otherwise, nameof(otherwise));

            return otherwise.Invoke();
        }

        public virtual TResult Coalesce<TResult>(Func<TError, bool> predicate, TResult then, TResult other)
            => other;

        public virtual void Do(Func<TError, bool> predicate, Action<TError> action, Action otherwise)
        {
            Require.NotNull(otherwise, nameof(otherwise));

            otherwise.Invoke();
        }

        public virtual void Do(Action<TError> caseError, Action caseVoid)
        {
            Require.NotNull(caseVoid, nameof(caseVoid));

            caseVoid.Invoke();
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Fx
{
    using System.Diagnostics.Contracts;

    public partial class VoidOrError<TError>
    {
        private sealed partial class Error_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_error != null);
                Contract.Invariant(IsError);
            }
        }
    }
}

#endif
