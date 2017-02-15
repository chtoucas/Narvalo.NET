// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;
    using System.Runtime.ExceptionServices;

    internal sealed class ExceptionCatcher<TException>
        : IExceptionCatcher
        where TException : Exception
    {
        public ExceptionCatcher() { }

        public VoidOrError Invoke(Action action)
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError>();

            try
            {
                action.Invoke();

                return VoidOrError.Void;
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return VoidOrError.FromError(edi);
            }
        }

        public Outcome<TResult> Invoke<TResult>(Func<TResult> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Outcome<TResult>>();

            try
            {
                TResult result = thunk.Invoke();

                return Outcome.Of(result);
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Outcome.FromError<TResult>(edi);
            }
        }

        public Outcome<TResult> Invoke<TSource, TResult>(
            Func<TSource, TResult> thunk,
            TSource value)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Outcome<TResult>>();

            try
            {
                TResult result = thunk.Invoke(value);

                return Outcome.Of(result);
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Outcome.FromError<TResult>(edi);
            }
        }
    }
}
