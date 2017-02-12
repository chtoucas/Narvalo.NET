// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    // Friendly version of Either<TError, Unit>.
    public abstract partial class VoidOr<TError> : Internal.IAlternative<TError>
    {
        private VoidOr() { }

        public abstract bool IsError { get; }

        internal abstract TError Error { get; }

        public bool IsVoid => !IsError;

        [DebuggerDisplay("Void")]
        private sealed partial class Void_ : VoidOr<TError>
        {
            public Void_() { }

            public override bool IsError => false;

            internal override TError Error
            {
                get { throw new InvalidOperationException("XXX"); }
            }

            public override VoidOr<TResult> Bind<TResult>(Func<TError, VoidOr<TResult>> selector)
                => VoidOr<TResult>.Void;

            public override VoidOr<TError> OrElse(VoidOr<TError> other) => other;

            #region Internal.IAlternative<TError> interface.

            public override TResult Match<TResult>(Func<TError, TResult> caseError, Func<TResult> caseSuccess)
            {
                Require.NotNull(caseSuccess, nameof(caseSuccess));

                return caseSuccess.Invoke();
            }

            public override TResult Match<TResult>(Func<TError, TResult> caseError, TResult caseSuccess)
                => caseSuccess;

            public override TResult Coalesce<TResult>(
                Func<TError, bool> predicate,
                Func<TError, TResult> selector,
                Func<TResult> otherwise)
            {
                Require.NotNull(otherwise, nameof(otherwise));

                return otherwise.Invoke();
            }

            public override TResult Coalesce<TResult>(Func<TError, bool> predicate, TResult then, TResult other)
                => other;

            public override void Do(Func<TError, bool> predicate, Action<TError> action, Action otherwise)
            {
                Require.NotNull(otherwise, nameof(otherwise));

                otherwise.Invoke();
            }

            public override void Do(Action<TError> onError, Action onSuccess)
            {
                Require.NotNull(onSuccess, nameof(onSuccess));

                onSuccess.Invoke();
            }

            public override void OnError(Action<TError> action) { }

            public override void OnSuccess(Action action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke();
            }

            #endregion

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return "Void";
            }
        }

        [DebuggerDisplay("Error")]
        [DebuggerTypeProxy(typeof(VoidOr<>.Error_.DebugView))]
        private sealed partial class Error_ : VoidOr<TError>
        {
            private readonly TError _error;

            public Error_(TError error)
            {
                Demand.NotNullUnconstrained(error);

                _error = error;
            }

            public override bool IsError => true;

            internal override TError Error
            {
                get { Warrant.NotNullUnconstrained<TError>(); return _error; }
            }

            public override VoidOr<TResult> Bind<TResult>(Func<TError, VoidOr<TResult>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Error);
            }

            public override VoidOr<TError> OrElse(VoidOr<TError> other) => this;

            #region Internal.IAlternative<TError> interface.

            public override TResult Match<TResult>(Func<TError, TResult> caseError, Func<TResult> caseSuccess)
            {
                Require.NotNull(caseError, nameof(caseError));

                return caseError.Invoke(Error);
            }

            public override TResult Match<TResult>(Func<TError, TResult> caseError, TResult caseSuccess)
            {
                Require.NotNull(caseError, nameof(caseError));

                return caseError.Invoke(Error);
            }

            public override TResult Coalesce<TResult>(
                Func<TError, bool> predicate,
                Func<TError, TResult> selector,
                Func<TResult> otherwise)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(selector, nameof(selector));
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
                Require.NotNull(otherwise, nameof(otherwise));

                if (predicate.Invoke(Error))
                {
                    action.Invoke(Error);
                }
                else
                {
                    otherwise.Invoke();
                }
            }

            public override void Do(Action<TError> onError, Action onSuccess)
            {
                Require.NotNull(onError, nameof(onError));

                onError.Invoke(Error);
            }

            public override void OnError(Action<TError> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(Error);
            }

            public override void OnSuccess(Action action) { }

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return "Error(" + Error.ToString() + ")";
            }

            #endregion

            /// <summary>
            /// Represents a debugger type proxy for <see cref="VoidOr{TError}.Error_"/>.
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

                public TError Error => _inner.Error;
            }
        }
    }

    // Provides the core Monad methods.
    public partial class VoidOr<TError>
    {
        public abstract VoidOr<TResult> Bind<TResult>(Func<TError, VoidOr<TResult>> selector);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static VoidOr<TError> η(TError value)
        {
            Require.NotNullUnconstrained(value, nameof(value));
            Warrant.NotNull<VoidOr<TError>>();

            return new VoidOr<TError>.Error_(value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static VoidOr<TError> μ(VoidOr<VoidOr<TError>> square)
            => square.IsError ? square.Error : VoidOr<TError>.Void;
    }

    // Provides the core MonadOr methods.
    public partial class VoidOr<TError>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly VoidOr<TError> s_Void = new VoidOr<TError>.Void_();

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static VoidOr<TError> Void
        {
            get { Warrant.NotNull<VoidOr<TError>>(); return s_Void; }
        }

        public abstract VoidOr<TError> OrElse(VoidOr<TError> other);
    }

    // Implements the Internal.IAlternative<TError> interface.
    public partial class VoidOr<TError>
    {
        public abstract TResult Match<TResult>(Func<TError, TResult> caseError, Func<TResult> caseSuccess);

        public abstract TResult Match<TResult>(Func<TError, TResult> caseError, TResult caseSuccess);

        public abstract TResult Coalesce<TResult>(
            Func<TError, bool> predicate,
            Func<TError, TResult> selector,
            Func<TResult> otherwise);

        public abstract TResult Coalesce<TResult>(Func<TError, bool> predicate, TResult then, TResult other);

        public abstract void Do(Func<TError, bool> predicate, Action<TError> action, Action otherwise);

        public abstract void Do(Action<TError> onError, Action onSuccess);

        public abstract void OnError(Action<TError> action);

        public abstract void OnSuccess(Action action);
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Fx
{
    using System.Diagnostics.Contracts;

    public partial class VoidOr<TError>
    {
        private sealed partial class Void_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(IsSuccess);
            }
        }

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
