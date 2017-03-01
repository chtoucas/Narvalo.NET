// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Result.DebugView))]
    public partial struct Result : IEquatable<Result>
    {
        private readonly ExceptionDispatchInfo _exceptionInfo;

        private Result(ExceptionDispatchInfo exceptionInfo)
        {
            _exceptionInfo = exceptionInfo;
            IsError = true;
        }

        public bool IsError { get; }

        public bool IsSuccess => !IsError;

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
        public Exception ToException()
        {
            if (IsSuccess) { throw new InvalidCastException("XXX"); }
            return ExceptionInfo.SourceException;
        }

        public ExceptionDispatchInfo ToExceptionInfo()
        {
            if (IsSuccess) { throw new InvalidCastException("XXX"); }
            return ExceptionInfo;
        }

        public static explicit operator ExceptionDispatchInfo(Result value) => value.ToExceptionInfo();

        public static explicit operator Result(ExceptionDispatchInfo exceptionInfo)
            => FromError(exceptionInfo);
    }

    // Implements the IEquatable<Result> interfaces.
    public partial struct Result
    {
        public static bool operator ==(Result left, Result right) => left.Equals(right);

        public static bool operator !=(Result left, Result right) => !left.Equals(right);

        public bool Equals(Result other)
        {
            if (IsError) { return ReferenceEquals(ExceptionInfo, other.ExceptionInfo); }

            return other.IsSuccess;
        }

        public override bool Equals(object obj) => obj is Result && Equals((Result)obj);

        public override int GetHashCode() => IsError ? ExceptionInfo.GetHashCode() : 0;
    }
}
