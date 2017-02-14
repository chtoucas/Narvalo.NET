// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    // Friendly version of Either<TError, Unit>.
    // WARNING: We make this class a "monad" on TError.
    // Normally, TError should represent a **light** error. For exceptions, see VoidOrError.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract partial class VoidOr<TError>
        : Internal.IAlternative<TError>, Internal.IOptional<TError>
    {
        private VoidOr() { }

        public abstract bool IsVoid { get; }

        public bool IsError => !IsVoid;

        internal abstract TError Error { get; }

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsVoid ? "Void" : "Error";

        [DebuggerTypeProxy(typeof(VoidOr<>.Void_.DebugView))]
        private sealed partial class Void_ : VoidOr<TError>
        {
            public Void_() { }

            public override bool IsVoid => true;

            internal override TError Error
            {
                get { throw new InvalidOperationException("XXX"); }
            }

            public override string ToString()
            {
                Warrant.NotNull<string>();
                return "Void";
            }

            /// <summary>
            /// Represents a debugger type proxy for <see cref="VoidOr{TError}.Void_"/>.
            /// </summary>
            [ContractVerification(false)] // Debugger-only code.
            [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
            private sealed class DebugView { }
        }

        [DebuggerTypeProxy(typeof(VoidOr<>.Error_.DebugView))]
        private sealed partial class Error_ : VoidOr<TError>
        {
            private readonly TError _error;

            public Error_(TError error)
            {
                Demand.NotNullUnconstrained(error);
                _error = error;
            }

            public override bool IsVoid => false;

            internal override TError Error
            {
                get { Warrant.NotNullUnconstrained<TError>(); return _error; }
            }

            public override string ToString()
            {
                Warrant.NotNull<string>();
                return Format.Current("Error({0})", Error);
            }

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

    // Conversion operators.
    public partial class VoidOr<TError>
    {
        public abstract TError ToError();

        public static explicit operator TError(VoidOr<TError> value)
            => value == null ? default(TError) : value.ToError();

        public static explicit operator VoidOr<TError>(TError error)
            => VoidOr.FromError(error);

        private partial class Void_
        {
            public override TError ToError()
            {
                throw new InvalidCastException("XXX");
            }
        }

        private partial class Error_
        {
            public override TError ToError() => Error;
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

        private partial class Void_
        {
            public override VoidOr<TResult> Bind<TResult>(Func<TError, VoidOr<TResult>> selector)
                => VoidOr<TResult>.Void;
        }

        private partial class Error_
        {
            public override VoidOr<TResult> Bind<TResult>(Func<TError, VoidOr<TResult>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Error);
            }
        }
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

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "OrElse", Justification = "[Intentionally] Standard name for the monoid method.")]
        public abstract VoidOr<TError> OrElse(VoidOr<TError> other);

        private partial class Void_
        {
            public override VoidOr<TError> OrElse(VoidOr<TError> other) => other;
        }

        private partial class Error_
        {
            public override VoidOr<TError> OrElse(VoidOr<TError> other) => this;
        }
    }

    // Implements the Internal.IAlternative<TError> interface.
    public partial class VoidOr<TError>
    {
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public abstract TResult Match<TResult>(Func<TError, TResult> caseError, Func<TResult> caseSuccess);

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public abstract TResult Match<TResult>(Func<TError, TResult> caseError, TResult caseSuccess);

        public abstract TResult Coalesce<TResult>(
            Func<TError, bool> predicate,
            Func<TError, TResult> selector,
            Func<TResult> otherwise);

        public abstract TResult Coalesce<TResult>(Func<TError, bool> predicate, TResult thenResult, TResult elseResult);

        public abstract void Do(Func<TError, bool> predicate, Action<TError> action, Action otherwise);

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public abstract void Do(Action<TError> onError, Action onSuccess);

        private partial class Void_ : VoidOr<TError>
        {
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

            public override TResult Coalesce<TResult>(Func<TError, bool> predicate, TResult thenResult, TResult elseResult)
                => elseResult;

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
        }

        private partial class Error_
        {
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

            public override TResult Coalesce<TResult>(Func<TError, bool> predicate, TResult thenResult, TResult elseResult)
            {
                Require.NotNull(predicate, nameof(predicate));

                return predicate.Invoke(Error) ? thenResult : elseResult;
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
        }
    }

    // Implements the Internal.IOptional<TError> interface.
    public partial class VoidOr<TError>
    {
        public abstract void When(Func<TError, bool> predicate, Action<TError> action);

        public abstract void OnSuccess(Action action);

        public abstract void OnError(Action<TError> action);

        private partial class Void_ : VoidOr<TError>
        {
            public override void When(Func<TError, bool> predicate, Action<TError> action) { }

            public override void OnSuccess(Action action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke();
            }

            public override void OnError(Action<TError> action) { }
        }

        private partial class Error_
        {
            public override void When(Func<TError, bool> predicate, Action<TError> action)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate.Invoke(Error)) { action.Invoke(Error); }
            }

            public override void OnSuccess(Action action) { }

            public override void OnError(Action<TError> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(Error);
            }
        }
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
