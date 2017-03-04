// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    using Narvalo;

    internal delegate Action RecursiveAction(RecursiveAction rs);

    internal delegate Action<T> RecursiveAction<T>(RecursiveAction<T> rec);

    internal delegate Action<T1, T2> RecursiveAction<T1, T2>(RecursiveAction<T1, T2> rec);

    internal delegate Func<T> Recursive<T>(Recursive<T> rec);

    internal delegate Func<T1, T2, TResult> Recursive<T1, T2, TResult>(Recursive<T1, T2, TResult> rec);

    public static class Recursion
    {
        public static Action Fix(Func<Action, Action> generator)
        {
            Require.NotNull(generator, nameof(generator));

            RecursiveAction rec = r => generator(r(r));
            return rec(rec);
        }

        public static Action<T> Fix<T>(Func<Action<T>, Action<T>> generator)
        {
            Require.NotNull(generator, nameof(generator));

            RecursiveAction<T> rec = r => generator(r(r));
            return rec(rec);
        }


        public static Action<T1, T2> Fix<T1, T2>(Func<Action<T1, T2>, Action<T1, T2>> generator)
        {
            Require.NotNull(generator, nameof(generator));

            RecursiveAction<T1, T2> rec = r => generator(r(r));
            return rec(rec);
        }

        public static Func<TResult> Fix<TResult>(
            Func<Func<TResult>, Func<TResult>> generator)
        {
            Require.NotNull(generator, nameof(generator));

            Recursive<TResult> rec = r => generator(r(r));
            return rec(rec);
        }

        // Readable version.
        public static Func<T1, T2, TResult> Fix<T1, T2, TResult>(
            Func<Func<T1, T2, TResult>, Func<T1, T2, TResult>> generator)
        {
            Require.NotNull(generator, nameof(generator));

            Recursive<T1, T2, TResult> rec
                = r => (arg1, arg2) => generator.Invoke(r.Invoke(r)).Invoke(arg1, arg2);

            return rec.Invoke(rec);
        }
    }
}
