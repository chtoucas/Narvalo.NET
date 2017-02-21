// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Sequences
{
    using System;
    using System.Collections.Generic;

    // Step by step explanation for YCombinator, Memoization and Currying, taken from:
    // https://blogs.msdn.microsoft.com/wesdyer/2007/01/29/currying-and-partial-function-application/
    // https://blogs.msdn.microsoft.com/wesdyer/2007/01/25/function-memoization/
    // https://blogs.msdn.microsoft.com/wesdyer/2007/02/02/anonymous-recursion-in-c/
    // Other useful posts:
    // https://blogs.msdn.microsoft.com/madst/2007/05/11/recursive-lambda-expressions/
    // https://blogs.msdn.microsoft.com/ericlippert/2006/08/18/why-does-a-recursive-lambda-cause-a-definite-assignment-error/
    public static class Anonymous
    {
        delegate TResult Recursive1<TSource, TResult>(Recursive1<TSource, TResult> rec, TSource source);

        delegate Func<TSource, TResult> Recursive<TSource, TResult>(Recursive<TSource, TResult> rec);

        public static Func<T1, Func<T2, TResult>> Curry1<T1, T2, TResult>(Func<T1, T2, TResult> func)
            => arg1 => arg2 => func(arg1, arg2);

        public static Func<T2, Func<T1, TResult>> Curry2<T1, T2, TResult>(Func<T1, T2, TResult> func)
            => arg2 => arg1 => func(arg1, arg2);

        public static Func<T2, TResult> Apply1<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 arg1)
            => arg2 => func(arg1, arg2);

        public static Func<T1, TResult> Apply2<T1, T2, TResult>(Func<T1, T2, TResult> func, T2 arg2)
            => arg1 => func(arg1, arg2);

        public static Func<Result> Memoize<Result>(Func<Result> func)
        {
            var value = default(Result);
            bool hasValue = false;

            return () =>
            {
                if (!hasValue)
                {
                    hasValue = true;
                    value = func();
                }

                return value;
            };
        }

        public static Func<TSource, TResult> Memoize<TSource, TResult>(Func<TSource, TResult> func)
        {
            var cache = new Dictionary<TSource, TResult>();

            return arg =>
            {
                TResult value;
                if (cache.TryGetValue(arg, out value))
                {
                    return value;
                }

                value = func(arg);
                cache.Add(arg, value);
                return value;
            };
        }

        public static int Fibonacci1(int n)
        {
            // We can not write:
            // > Func<int, int> func = i => i > 1 ? func(i - 1) + func(i - 2) : i;
            // since func is not yet defined.
            Func<int, int> func = null;
            func = i => i > 1 ? func(i - 1) + func(i - 2) : i;
            // This is not a recursive definition, since func() just calls the local variable "func"
            // it is refering to. A quirk: we can redefine "func"
            // > func = i => 2 * i;
            // func(i) is no longer the fibonacci of i. More quirks:
            // > Func<int, int> copy = func;
            // > func = i => 2 * i;
            // copy(i) is no longer equal to func(i) as copy() still points to the original func.
            // It is not even the same as the original func(i), in plain terms we have something
            // like:
            // > copy = i => i > 1 ? newfunc(i - 1) + newfunc(i - 2) : i;
            return func(n);
        }

        public static int Fibonacci2(int n)
        {
            // To obtain a recursive definition, just pass the function itself as a parameter.
            Recursive1<int, int> rec = (self, i) => i > 1 ? self(self, i - 1) + self(self, i - 2) : i;

            return rec(rec, n);
        }

        public static int Fibonacci3(int n)
        {
            // "Currification" to make the call to look easier to the eyes.
            Recursive<int, int> rec = self => i => i > 1 ? self(self)(i - 1) + self(self)(i - 2) : i;

            return rec(rec)(n);
        }

        public static int Fibonacci4(int n)
        {
            // Remove the duplicate call to func(func).
            Recursive<int, int> rec = self => i =>
            {
                Func<Func<int, int>, int, int> g = (f, j) => j > 1 ? f(j - 1) + f(j - 2) : j;
                return g(self(self), i);
            };

            return rec(rec)(n);
        }

        public static int Fibonacci5(int n)
        {
            // Push g outside rec.
            Func<Func<int, int>, int, int> g = (func, i) => i > 1 ? func(i - 1) + func(i - 2) : i;

            Recursive<int, int> rec = self => i => g(self(self), i);

            return rec(rec)(n);
        }

        public static int Fibonacci6(int n)
        {
            // "Currification" of g.
            Func<Func<int, int>, Func<int, int>> g = func => i => i > 1 ? func(i - 1) + func(i - 2) : i;

            Recursive<int, int> rec = self => i => g(self(self))(i);

            return rec(rec)(n);
        }

        // Extract the YCombinator.
        public static Func<int, int> Fix(Func<Func<int, int>, Func<int, int>> generator)
        {
            Recursive<int, int> rec = self => generator(self(self));

            return rec(rec);
        }

        // Final version.
        public static int Fibonacci(int n)
        {
            Func<Func<int, int>, Func<int, int>> g = f => j => j > 1 ? f(j - 1) + f(j - 2) : j;

            Func<int, int> func = Fix(g);

            return func(n);
        }

        // Generic version.
        public static Func<TSource, TResult> Y1<TSource, TResult>(
            Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            // NB: At each step, we will apply generator (once) and func (twice).
            Recursive<TSource, TResult> rec = func => generator(func(func));

            return rec(rec);
        }

        // Faster version.
        public static Func<TSource, TResult> Y2<TSource, TResult>(
            Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            // NB: At each step, we will apply generator (once) and g (once).
            Func<TSource, TResult> g = null;
            g = arg => generator(g)(arg);

            return g;
        }

        // Even Faster version.
        public static Func<TSource, TResult> Y3<TSource, TResult>(
            Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            // NB: We apply generator only once (here) and, at each step, we will apply g (once).
            Func<TSource, TResult> g = null;
            g = generator(arg => g(arg));

            return g;
        }

        // With memoization.
        public static Func<TSource, TResult> Y4<TSource, TResult>(
            Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Func<TSource, TResult> h = null;
            Func<TSource, TResult> g = null;

            g = arg => generator(h)(arg);
            h = Memoize(g);

            return h;
        }
    }
}
