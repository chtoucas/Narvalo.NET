// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract partial class Error
    {
        private Error() { }

        public abstract bool IsVoid { get; }

        public bool IsError => !IsVoid;

        internal abstract ExceptionDispatchInfo ExceptionInfo { get; }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsVoid ? "Void" : "Error";

        public abstract void ThrowIfError();

        [DebuggerTypeProxy(typeof(Void_.DebugView))]
        private sealed partial class Void_ : Error
        {
            public Void_() { }

            public override bool IsVoid => true;

            internal override ExceptionDispatchInfo ExceptionInfo
            {
                get { throw new InvalidOperationException("XXX"); }
            }

            public override void ThrowIfError() { }

            public override string ToString() => "Void";

            /// <summary>
            /// Represents a debugger type proxy for <see cref="Error.Void_"/>.
            /// </summary>
            [ExcludeFromCodeCoverage]
            private sealed class DebugView { }
        }

        [DebuggerTypeProxy(typeof(Error_.DebugView))]
        private sealed partial class Error_ : Error
        {
            private readonly ExceptionDispatchInfo _exceptionInfo;

            public Error_(ExceptionDispatchInfo exceptionInfo)
            {
                Demand.NotNull(exceptionInfo);

                _exceptionInfo = exceptionInfo;
            }

            public override bool IsVoid => false;

            internal override ExceptionDispatchInfo ExceptionInfo
            {
                get { return _exceptionInfo; }
            }

            public override void ThrowIfError() => ExceptionInfo.Throw();

            public override string ToString()
            {
                return Format.Current("Error({0})", ExceptionInfo.SourceException.Message);
            }

            /// <summary>
            /// Represents a debugger type proxy for <see cref="Error.Error_"/>.
            /// </summary>
            [ExcludeFromCodeCoverage]
            private sealed class DebugView
            {
                private readonly Error_ _inner;

                public DebugView(Error_ inner)
                {
                    _inner = inner;
                }

                public ExceptionDispatchInfo ExceptionInfo => _inner.ExceptionInfo;
            }
        }
    }

    // Conversion operators.
    public partial class Error
    {
        public abstract Exception ToException();

        public abstract ExceptionDispatchInfo ToExceptionInfo();

        public static explicit operator Exception(Error value)
            => value?.ToException();

        public static explicit operator ExceptionDispatchInfo(Error value)
            => value?.ToExceptionInfo();

        public static explicit operator Error(ExceptionDispatchInfo exceptionInfo)
            => FromError(exceptionInfo);

        private partial class Void_
        {
            public override Exception ToException()
            {
                throw new InvalidCastException("XXX");
            }

            public override ExceptionDispatchInfo ToExceptionInfo()
            {
                throw new InvalidCastException("XXX");
            }
        }

        private partial class Error_
        {
            public override Exception ToException() => ExceptionInfo.SourceException;

            public override ExceptionDispatchInfo ToExceptionInfo() => ExceptionInfo;
        }
    }

    // Factory methods.
    public partial class Error
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Error s_Void = new Error.Void_();

        public static Error Void { get { return s_Void; } }

        public static Error FromError(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, nameof(exceptionInfo));

            return new Error.Error_(exceptionInfo);
        }

        // NB: This method serves a different purpose than the trywith from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of VoidOrError.")]
        public static Error TryWith(Action action)
        {
            Require.NotNull(action, nameof(action));

            try
            {
                action.Invoke();

                return Void;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError(edi);
            }
        }

        // NB: This method serves a different purpose than the tryfinally from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of VoidOrError.")]
        public static Error TryFinally(Action action, Action finallyAction)
        {
            Require.NotNull(action, nameof(action));
            Require.NotNull(finallyAction, nameof(finallyAction));

            try
            {
                action.Invoke();

                return Void;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError(edi);
            }
            finally
            {
                finallyAction.Invoke();
            }
        }
    }
}
