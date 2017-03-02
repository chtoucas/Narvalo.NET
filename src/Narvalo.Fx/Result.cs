// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    using Narvalo.Fx.Properties;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Result.DebugView))]
    public partial struct Result : IEquatable<Result>, Internal.IEither<Unit, ExceptionDispatchInfo>
    {
        private readonly ExceptionDispatchInfo _exceptionInfo;

        private Result(ExceptionDispatchInfo exceptionInfo)
        {
            Demand.NotNull(exceptionInfo);

            _exceptionInfo = exceptionInfo;
            IsError = true;
        }

        /// <summary>
        /// Gets a value indicating whether the object is the result of an unsuccessful computation.
        /// </summary>
        /// <value>true if the outcome was unsuccessful; otherwise false.</value>
        public bool IsError { get; }

        /// <summary>
        /// Gets a value indicating whether the object is the result of a successful computation.
        /// </summary>
        /// <value>true if the outcome was successful; otherwise false.</value>
        public bool IsSuccess => !IsError;

        /// <summary>
        /// Obtains the captured exception state.
        /// </summary>
        /// <remarks>Any access to this method must be protected by checking before that
        /// <see cref="IsError"/> is true.</remarks>
        internal ExceptionDispatchInfo ExceptionInfo { get { Demand.State(IsError); return _exceptionInfo; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsError ? "Error" : "Success";

        public void ThrowIfError()
        {
            if (IsError) { ExceptionInfo.Throw(); }
        }

        public override string ToString()
            => IsError ? Format.Current("Error({0})", ExceptionInfo.SourceException.Message) : "Success";

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Result"/>.
        /// </summary>
        /// <remarks>Ensure that <see cref="Result.ExceptionInfo"/> does not throw in the debugger
        /// for DEBUG builds.</remarks>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Result _inner;

            public DebugView(Result inner)
            {
                _inner = inner;
            }

            public bool IsSuccess => _inner.IsSuccess;

            public ExceptionDispatchInfo ExceptionInfo => _inner._exceptionInfo;
        }
    }

    // Conversion operators.
    public partial struct Result
    {
        public ExceptionDispatchInfo ToExceptionInfo()
        {
            if (IsSuccess) { throw new InvalidCastException(Strings.InvalidCast_ToError); }
            return ExceptionInfo;
        }

        public static explicit operator ExceptionDispatchInfo(Result value) => value.ToExceptionInfo();
    }

    // Implements the Internal.IEither<Unit, ExceptionDispatchInfo> interface.
    public partial struct Result
    {
        public TResult Match<TResult>(
            Func<TResult> caseSuccess,
            Func<ExceptionDispatchInfo, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsSuccess ? caseSuccess() : caseError(ExceptionInfo);
        }

        public void Do(Action onSuccess, Action<ExceptionDispatchInfo> onError)
        {
            Require.NotNull(onSuccess, nameof(onSuccess));
            Require.NotNull(onError, nameof(onError));

            if (IsSuccess) { onSuccess(); } else { onError(ExceptionInfo); }
        }

        public void WhenSuccess(Func<bool> predicate, Action action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsSuccess && predicate()) { action(); }
        }

        public void WhenError(
            Func<ExceptionDispatchInfo, bool> predicate,
            Action<ExceptionDispatchInfo> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsError && predicate(ExceptionInfo)) { action(ExceptionInfo); }
        }

        public void OnSuccess(Action action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSuccess) { action(); }
        }

        public void OnError(Action<ExceptionDispatchInfo> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsError) { action(ExceptionInfo); }
        }

        #region Publicly hidden methods.

        bool Internal.IContainer<Unit>.Contains(Unit value)
        {
            throw new NotSupportedException();
        }

        bool Internal.IContainer<Unit>.Contains(Unit value, IEqualityComparer<Unit> comparer)
        {
            throw new NotSupportedException();
        }

        bool Internal.ISecondaryContainer<ExceptionDispatchInfo>.Contains(ExceptionDispatchInfo value)
        {
            throw new NotSupportedException();
        }

        bool Internal.ISecondaryContainer<ExceptionDispatchInfo>.Contains(
            ExceptionDispatchInfo value,
            IEqualityComparer<ExceptionDispatchInfo> comparer)
        {
            throw new NotSupportedException();
        }

        // Alias for Match().
        TResult Internal.IEither<Unit, ExceptionDispatchInfo>.Match<TResult>(
            Func<Unit, TResult> caseLeft,
            Func<ExceptionDispatchInfo, TResult> caseRight)
            => Match(() => caseLeft(Narvalo.Fx.Unit.Default), caseRight);

        // Alias for Do().
        void Internal.IEither<Unit, ExceptionDispatchInfo>.Do(
            Action<Unit> onLeft,
            Action<ExceptionDispatchInfo> onRight)
            => Do(() => onLeft(Narvalo.Fx.Unit.Default), onRight);

        // Alias for WhenSuccess().
        void Internal.IContainer<Unit>.When(Func<Unit, bool> predicate, Action<Unit> action)
            => WhenSuccess(() => predicate(Narvalo.Fx.Unit.Default), () => action(Narvalo.Fx.Unit.Default));

        // Alias for WhenError().
        void Internal.ISecondaryContainer<ExceptionDispatchInfo>.When(
            Func<ExceptionDispatchInfo, bool> predicate,
            Action<ExceptionDispatchInfo> action)
            => WhenError(predicate, action);

        // Alias for OnSuccess().
        void Internal.IContainer<Unit>.Do(Action<Unit> action) => OnSuccess(() => action(Narvalo.Fx.Unit.Default));

        // Alias for OnError().
        void Internal.ISecondaryContainer<ExceptionDispatchInfo>.Do(Action<ExceptionDispatchInfo> action)
            => OnError(action);

        #endregion
    }

    // Implements the IEquatable<Result> interface.
    public partial struct Result
    {
        public static bool operator ==(Result left, Result right) => left.Equals(right);

        public static bool operator !=(Result left, Result right) => !left.Equals(right);

        public bool Equals(Result other)
        {
            if (IsError) { return other.IsError && ReferenceEquals(ExceptionInfo, other.ExceptionInfo); }
            return other.IsSuccess;
        }

        public override bool Equals(object obj) => (obj is Result) && Equals((Result)obj);

        public override int GetHashCode() => _exceptionInfo?.GetHashCode() ?? 0;
    }
}
