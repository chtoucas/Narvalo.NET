// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    // FIXME: Thread-safety for Func.
    public sealed class ReliabilityWrapper
    {
        private readonly IReliabilitySentinel _sentinel;

        public ReliabilityWrapper(IReliabilitySentinel sentinel)
        {
            Require.NotNull(sentinel, nameof(sentinel));

            _sentinel = sentinel;
        }

        public Action Wrap(Action action)
            => () => _sentinel.Invoke(action);

        public Action<T> Wrap<T>(Action<T> action)
            => (arg) => _sentinel.Invoke(() => action(arg));

        public Action<T1, T2> Wrap<T1, T2>(Action<T1, T2> action)
            => (arg1, arg2) => _sentinel.Invoke(() => action(arg1, arg2));

        public Action<T1, T2, T3> Wrap<T1, T2, T3>(Action<T1, T2, T3> action)
            => (arg1, arg2, arg3) => _sentinel.Invoke(() => action(arg1, arg2, arg3));

        public Action<T1, T2, T3, T4> Wrap<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
            => (arg1, arg2, arg3, arg4) => _sentinel.Invoke(() => action(arg1, arg2, arg3, arg4));

        public Func<TResult> Wrap<TResult>(Func<TResult> func)
            => () =>
            {
                var retval = default(TResult);
                _sentinel.Invoke(() => { retval = func(); });
                return retval;
            };

        public Func<T, TResult> Wrap<T, TResult>(Func<T, TResult> func)
            => (arg) =>
            {
                var retval = default(TResult);
                _sentinel.Invoke(() => { retval = func(arg); });
                return retval;
            };

        public Func<T1, T2, TResult> Wrap<T1, T2, TResult>(Func<T1, T2, TResult> func)
            => (arg1, arg2) =>
            {
                var retval = default(TResult);
                _sentinel.Invoke(() => { retval = func(arg1, arg2); });
                return retval;
            };

        public Func<T1, T2, T3, TResult> Wrap<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
            => (arg1, arg2, arg3) =>
            {
                var retval = default(TResult);
                _sentinel.Invoke(() => { retval = func(arg1, arg2, arg3); });
                return retval;
            };

        public Func<T1, T2, T3, T4, TResult> Wrap<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
            => (arg1, arg2, arg3, arg4) =>
            {
                var retval = default(TResult);
                _sentinel.Invoke(() => { retval = func(arg1, arg2, arg3, arg4); });
                return retval;
            };
    }
}
