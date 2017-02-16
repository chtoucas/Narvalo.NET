﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;
    using System.Runtime.ExceptionServices;

    internal sealed class ExceptionCatcher<T1Exception, T2Exception, T3Exception, T4Exception>
        : IExceptionCatcher
        where T1Exception : Exception
        where T2Exception : Exception
        where T3Exception : Exception
        where T4Exception : Exception
    {
        public ExceptionCatcher() { }

        public VoidOrError TryInvoke(Action action)
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError>();

            ExceptionDispatchInfo edi;

            try
            {
                action.Invoke();

                return VoidOrError.Void;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.FromError(edi);
        }

        public ResultOrError<TResult> TryInvoke<TResult>(Func<TResult> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<ResultOrError<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult result = thunk.Invoke();

                return ResultOrError.Of(result);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return ResultOrError.FromError<TResult>(edi);
        }

        public ResultOrError<TResult> TryInvoke<TSource, TResult>(Func<TSource, TResult> thunk, TSource value)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<ResultOrError<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult result = thunk.Invoke(value);

                return ResultOrError.Of(result);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return ResultOrError.FromError<TResult>(edi);
        }
    }
}
