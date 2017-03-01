// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class Outcome
    {
        public static Outcome<T, TError> Of<T, TError>(T value) => Outcome<T, TError>.η(value);

        public static Outcome<T, TError> FromError<T, TError>(TError value) => Outcome<T, TError>.η(value);
    }

    // Friendly version of Either<T, TError>. NB: In Haskell, the error is the left type parameter.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract partial class Outcome<T, TError> : Internal.IEither<T, TError>, Internal.Iterable<T>
    {
        private Outcome() { }

        public abstract bool IsSuccess { get; }

        public bool IsError => !IsSuccess;

        internal abstract T Value { get; }

        internal abstract TError Error { get; }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsSuccess ? "Success" : "Error";

        public abstract T ValueOrDefault();

        public abstract T ValueOrElse(T other);

        public abstract T ValueOrElse(Func<T> valueFactory);

        public abstract Maybe<T> ValueOrNone();

        public abstract T ValueOrThrow(Exception exception);

        public abstract T ValueOrThrow(Func<Exception> exceptionFactory);

        [DebuggerTypeProxy(typeof(Outcome<,>.Success_.DebugView))]
        private sealed partial class Success_ : Outcome<T, TError>
        {
            private readonly T _value;

            public Success_(T value)
            {
                _value = value;
            }

            public override bool IsSuccess => true;

            internal override T Value => _value;

            internal override TError Error { get { throw new InvalidOperationException("XXX"); } }

            public override T ValueOrDefault() => Value;

            public override T ValueOrElse(T other) => Value;

            public override T ValueOrElse(Func<T> valueFactory) => Value;

            public override Maybe<T> ValueOrNone() => Maybe.Of(Value);

            public override T ValueOrThrow(Exception exception) => Value;

            public override T ValueOrThrow(Func<Exception> exceptionFactory) => Value;

            public override string ToString() => Format.Current("Success({0})", Value);

            /// <summary>
            /// Represents a debugger type proxy for <see cref="Outcome{T, TError}.Success_"/>.
            /// </summary>
            [Narvalo.ExcludeFromCodeCoverage]
            private sealed class DebugView
            {
                private readonly Outcome<T, TError> _inner;

                public DebugView(Outcome<T, TError> inner)
                {
                    _inner = inner;
                }

                public T Value => _inner.Value;
            }
        }

        [DebuggerTypeProxy(typeof(Outcome<,>.Error_.DebugView))]
        private sealed partial class Error_ : Outcome<T, TError>
        {
            private readonly TError _error;

            public Error_(TError error)
            {
                _error = error;
            }

            public override bool IsSuccess => false;

            internal override T Value { get { throw new InvalidOperationException("XXX"); } }

            internal override TError Error => _error;

            public override T ValueOrDefault() => default(T);

            public override T ValueOrElse(T other) => other;

            public override T ValueOrElse(Func<T> valueFactory)
            {
                Require.NotNull(valueFactory, nameof(valueFactory));

                return valueFactory.Invoke();
            }

            public override Maybe<T> ValueOrNone() => Maybe<T>.None;

            public override T ValueOrThrow(Exception exception)
            {
                Require.NotNull(exception, nameof(exception));

                throw exception;
            }

            public override T ValueOrThrow(Func<Exception> exceptionFactory)
            {
                Require.NotNull(exceptionFactory, nameof(exceptionFactory));

                throw exceptionFactory.Invoke();
            }

            public override string ToString() => Format.Current("Error({0})", Error);

            /// <summary>
            /// Represents a debugger type proxy for <see cref="Outcome{T, TError}.Error_"/>.
            /// </summary>
            [ExcludeFromCodeCoverage]
            private sealed class DebugView
            {
                private readonly Outcome<T, TError> _inner;

                public DebugView(Outcome<T, TError> inner)
                {
                    _inner = inner;
                }

                public TError Error => _inner.Error;
            }
        }
    }

    // Conversion operators.
    public partial class Outcome<T, TError>
    {
        public abstract T ToValue();

        public abstract TError ToError();

        public abstract Either<T, TError> ToEither();

        public static explicit operator T(Outcome<T, TError> value)
            => value == null ? default(T) : value.ToValue();

        public static explicit operator TError(Outcome<T, TError> value)
            => value == null ? default(TError) : value.ToError();

        public static explicit operator Outcome<T, TError>(T value)
            => Outcome.Of<T, TError>(value);

        public static explicit operator Outcome<T, TError>(TError error)
            => Outcome.FromError<T, TError>(error);

        private partial class Success_
        {
            public override T ToValue() => Value;

            public override TError ToError()
            {
                throw new InvalidCastException("XXX");
            }

            public override Either<T, TError> ToEither() => Either.OfLeft<T, TError>(Value);
        }

        private partial class Error_
        {
            public override T ToValue()
            {
                throw new InvalidCastException("XXX");
            }

            public override TError ToError() => Error;

            public override Either<T, TError> ToEither() => Either.OfRight<T, TError>(Error);
        }
    }

    // Provides the core Monad methods.
    public partial class Outcome<T, TError>
    {
        public abstract Outcome<TOutcome, TError> Bind<TOutcome>(Func<T, Outcome<TOutcome, TError>> selector);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Outcome<T, TError> η(T value) => new Success_(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Outcome<T, TError> η(TError value) => new Error_(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Outcome<T, TError> μ(Outcome<Outcome<T, TError>, TError> square)
        {
            Require.NotNull(square, nameof(square));

            return square.IsSuccess ? square.Value : Outcome.FromError<T, TError>(square.Error);
        }

        private partial class Success_
        {
            public override Outcome<TOutcome, TError> Bind<TOutcome>(Func<T, Outcome<TOutcome, TError>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Value);
            }
        }

        private partial class Error_
        {
            public override Outcome<TOutcome, TError> Bind<TOutcome>(Func<T, Outcome<TOutcome, TError>> selector)
                => Outcome.FromError<TOutcome, TError>(Error);
        }
    }

    // Implements the Internal.IEither<T, TError> interface.
    public partial class Outcome<T, TError>
    {
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public abstract TOutcome Match<TOutcome>(Func<T, TOutcome> caseSuccess, Func<TError, TOutcome> caseError);

        // Alias for WhenSuccess().
        // NB: We keep this one public as it overrides the auto-generated method.
        public void When(Func<T, bool> predicate, Action<T> action)
            => WhenSuccess(predicate, action);

        // Alias for WhenError(). Publicly hidden.
        void Internal.ISecondaryContainer<TError>.When(
            Func<TError, bool> predicate,
            Action<TError> action)
            => WhenError(predicate, action);

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public abstract void Do(Action<T> onSuccess, Action<TError> onError);

        // Alias for OnSuccess(). Publicly hidden.
        void Internal.IContainer<T>.Do(Action<T> onSuccess) => OnSuccess(onSuccess);

        // Alias for OnError(). Publicly hidden.
        void Internal.ISecondaryContainer<TError>.Do(Action<TError> onError) => OnError(onError);

        public abstract void WhenSuccess(Func<T, bool> predicate, Action<T> action);

        public abstract void WhenError(Func<TError, bool> predicate, Action<TError> action);

        public abstract void OnSuccess(Action<T> action);

        public abstract void OnError(Action<TError> action);

        private partial class Success_
        {
            public override TOutcome Match<TOutcome>(Func<T, TOutcome> caseSuccess, Func<TError, TOutcome> caseError)
            {
                Require.NotNull(caseSuccess, nameof(caseSuccess));

                return caseSuccess.Invoke(Value);
            }

            public override void WhenSuccess(Func<T, bool> predicate, Action<T> action)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate.Invoke(Value)) { action.Invoke(Value); }
            }

            public override void WhenError(Func<TError, bool> predicate, Action<TError> action) { }

            public override void Do(Action<T> onSuccess, Action<TError> onError)
            {
                Require.NotNull(onSuccess, nameof(onSuccess));

                onSuccess.Invoke(Value);
            }

            public override void OnSuccess(Action<T> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(Value);
            }

            public override void OnError(Action<TError> action) { }
        }

        private partial class Error_
        {
            public override TOutcome Match<TOutcome>(Func<T, TOutcome> caseSuccess, Func<TError, TOutcome> caseError)
            {
                Require.NotNull(caseError, nameof(caseError));

                return caseError.Invoke(Error);
            }

            public override void WhenSuccess(Func<T, bool> predicate, Action<T> action) { }

            public override void WhenError(Func<TError, bool> predicate, Action<TError> action)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate.Invoke(Error)) { action.Invoke(Error); }
            }

            public override void Do(Action<T> onSuccess, Action<TError> onError)
            {
                Require.NotNull(onError, nameof(onError));

                onError.Invoke(Error);
            }

            public override void OnSuccess(Action<T> action) { }

            public override void OnError(Action<TError> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(Error);
            }
        }
    }

    // Implements the Internal.Iterable<T> interface.
    public partial class Outcome<T, TError>
    {
        public abstract IEnumerable<T> ToEnumerable();

        public IEnumerator<T> GetEnumerator() => ToEnumerable().GetEnumerator();

        private partial class Success_
        {
            public override IEnumerable<T> ToEnumerable() => Sequence.Of(Value);
        }

        private partial class Error_
        {
            public override IEnumerable<T> ToEnumerable() => Enumerable.Empty<T>();
        }
    }
}
