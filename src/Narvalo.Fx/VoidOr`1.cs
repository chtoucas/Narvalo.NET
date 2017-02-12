// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    // Friendly version of Either<TMessage, Unit>.
    public abstract partial class VoidOr<TMessage> : Internal.IAlternative<TMessage>
    {
        private VoidOr() { }

        private VoidOr(bool isMessage)
        {
            IsMessage = isMessage;
        }

        public bool IsMessage { get; }

        public bool IsVoid => !IsMessage;

        internal abstract TMessage Message { get; }

        [DebuggerDisplay("Void")]
        private sealed partial class Void_ : VoidOr<TMessage>
        {
            public Void_() : base(false) { }

            internal override TMessage Message
            {
                get { throw new InvalidOperationException("XXX"); }
            }

            public override VoidOr<TResult> Bind<TResult>(Func<TMessage, VoidOr<TResult>> selector)
                => VoidOr<TResult>.Void;

            public override VoidOr<TMessage> OrElse(VoidOr<TMessage> other) => other;

            public override TResult Match<TResult>(Func<TMessage, TResult> caseMessage, Func<TResult> caseVoid)
            {
                Require.NotNull(caseVoid, nameof(caseVoid));

                return caseVoid.Invoke();
            }

            public override TResult Match<TResult>(Func<TMessage, TResult> caseMessage, TResult caseVoid)
                => caseVoid;

            public override TResult Coalesce<TResult>(
                Func<TMessage, bool> predicate,
                Func<TMessage, TResult> selector,
                Func<TResult> otherwise)
            {
                Require.NotNull(otherwise, nameof(otherwise));

                return otherwise.Invoke();
            }

            public override TResult Coalesce<TResult>(Func<TMessage, bool> predicate, TResult then, TResult other)
                => other;

            public override void Do(Func<TMessage, bool> predicate, Action<TMessage> action, Action otherwise)
            {
                Require.NotNull(otherwise, nameof(otherwise));

                otherwise.Invoke();
            }

            public override void Do(Action<TMessage> caseMessage, Action caseVoid)
            {
                Require.NotNull(caseVoid, nameof(caseVoid));

                caseVoid.Invoke();
            }

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return "Void";
            }
        }

        [DebuggerDisplay("Message")]
        [DebuggerTypeProxy(typeof(VoidOr<>.Message_.DebugView))]
        private sealed partial class Message_ : VoidOr<TMessage>
        {
            private readonly TMessage _message;

            public Message_(TMessage message)
                : base(true)
            {
                Demand.NotNullUnconstrained(message);

                _message = message;
            }

            internal override TMessage Message
            {
                get { Warrant.NotNullUnconstrained<TMessage>(); return _message; }
            }

            public override VoidOr<TResult> Bind<TResult>(Func<TMessage, VoidOr<TResult>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Message);
            }

            public override VoidOr<TMessage> OrElse(VoidOr<TMessage> other) => this;

            public override TResult Match<TResult>(Func<TMessage, TResult> caseMessage, Func<TResult> caseVoid)
            {
                Require.NotNull(caseMessage, nameof(caseMessage));

                return caseMessage.Invoke(Message);
            }

            public override TResult Match<TResult>(Func<TMessage, TResult> caseMessage, TResult caseVoid)
            {
                Require.NotNull(caseMessage, nameof(caseMessage));

                return caseMessage.Invoke(Message);
            }

            public override TResult Coalesce<TResult>(
                Func<TMessage, bool> predicate,
                Func<TMessage, TResult> selector,
                Func<TResult> otherwise)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(selector, nameof(selector));
                Require.NotNull(otherwise, nameof(otherwise));

                return predicate.Invoke(Message) ? selector.Invoke(Message) : otherwise.Invoke();
            }

            public override TResult Coalesce<TResult>(Func<TMessage, bool> predicate, TResult then, TResult other)
            {
                Require.NotNull(predicate, nameof(predicate));

                return predicate.Invoke(Message) ? then : other;
            }

            public override void Do(Func<TMessage, bool> predicate, Action<TMessage> action, Action otherwise)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));
                Require.NotNull(otherwise, nameof(otherwise));

                if (predicate.Invoke(Message))
                {
                    action.Invoke(Message);
                }
                else
                {
                    otherwise.Invoke();
                }
            }

            public override void Do(Action<TMessage> caseMessage, Action caseVoid)
            {
                Require.NotNull(caseMessage, nameof(caseMessage));

                caseMessage.Invoke(Message);
            }

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return "Message(" + Message.ToString() + ")";
            }

            /// <summary>
            /// Represents a debugger type proxy for <see cref="VoidOr{TMessage}.Message_"/>.
            /// </summary>
            [ContractVerification(false)] // Debugger-only code.
            [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
            private sealed class DebugView
            {
                private readonly Message_ _inner;

                public DebugView(Message_ inner)
                {
                    _inner = inner;
                }

                public TMessage Message => _inner.Message;
            }
        }
    }

    // Provides the core Monad methods.
    public partial class VoidOr<TMessage>
    {
        public abstract VoidOr<TResult> Bind<TResult>(Func<TMessage, VoidOr<TResult>> selector);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static VoidOr<TMessage> η(TMessage value)
        {
            Require.NotNullUnconstrained(value, nameof(value));
            Warrant.NotNull<VoidOr<TMessage>>();

            return new VoidOr<TMessage>.Message_(value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static VoidOr<TMessage> μ(VoidOr<VoidOr<TMessage>> square)
            => square.IsMessage ? square.Message : VoidOr<TMessage>.Void;
    }

    // Provides the core MonadOr methods.
    public partial class VoidOr<TMessage>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly VoidOr<TMessage> s_Void = new VoidOr<TMessage>.Void_();

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static VoidOr<TMessage> Void
        {
            get { Warrant.NotNull<VoidOr<TMessage>>(); return s_Void; }
        }

        public abstract VoidOr<TMessage> OrElse(VoidOr<TMessage> other);
    }

    // Implements the Internal.IAlternative<TMessage> interface.
    public partial class VoidOr<TMessage>
    {
        public abstract TResult Match<TResult>(Func<TMessage, TResult> caseMessage, Func<TResult> caseVoid);

        public abstract TResult Match<TResult>(Func<TMessage, TResult> caseMessage, TResult caseVoid);

        public abstract TResult Coalesce<TResult>(
            Func<TMessage, bool> predicate,
            Func<TMessage, TResult> selector,
            Func<TResult> otherwise);

        public abstract TResult Coalesce<TResult>(Func<TMessage, bool> predicate, TResult then, TResult other);

        public abstract void Do(Func<TMessage, bool> predicate, Action<TMessage> action, Action otherwise);

        public abstract void Do(Action<TMessage> caseMessage, Action caseVoid);
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Fx
{
    using System.Diagnostics.Contracts;

    public partial class VoidOr<TMessage>
    {
        private sealed partial class Message_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_message != null);
                Contract.Invariant(IsMessage);
            }
        }
    }
}

#endif
