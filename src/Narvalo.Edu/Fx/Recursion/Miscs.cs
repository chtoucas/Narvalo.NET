// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx.Recursion
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Edu.Fx;

    // http://codebetter.com/matthewpodwysocki/2008/06/12/functional-c-unfolding-lists/
    // http://williamsadventures.wordpress.com/
    // http://www.mikeobrien.net/blog/folding-and-unfolding-in-f-and-linq/
    // http://themechanicalbride.blogspot.fr/2009/07/introducing-rx-linq-to-events.html
    // http://blogs.msdn.com/b/wesdyer/archive/2007/02/02/anonymous-recursion-in-c.aspx
    // http://blog.functorial.com/posts/2010-12-05-Greatest-Fixed-Points.html
    // http://blog.functorial.com/posts/2010-12-04-Least-Fixed-Points.html

    // http://en.wikipedia.org/wiki/Fixed-point_combinator#cite_ref-12

    interface Functor<A>
    {
        Functor<B> Map<B>(Func<A, B> f);
    }

    class Mu
    {
        public Functor<Mu> Out { get; private set; }

        public Mu(Functor<Mu> f)
        {
            this.Out = f;
        }

        public TResult Cata<TResult>(Func<Functor<TResult>, TResult> phi)
        {
            return phi(Out.Map(_ => _.Cata(phi)));
        }

        public static Mu Ana<TResult>(Func<TResult, Functor<TResult>> psi, TResult seed)
        {
            return new Mu(psi(seed).Map(_ => Mu.Ana(psi, _)));
        }

        public static T2 Hylo<T1, T2>(
            Func<T1, Functor<T1>> psi,
            Func<Functor<T2>, T2> phi,
            T1 seed)
        {
            return Mu.Ana(psi, seed).Cata(phi);
        }
    }

    public interface ICatamorphic<T>
    {
        TResult Cata<TResult>(
           TResult seed,
           Func<TResult, bool> predicate,
           Func<TResult, T, TResult> accumulator);
    }

    interface IAnamorphic<T>
    {
        IAnamorphic<T> Ana<TResult>(
             Func<T, T> succ,
             T seed,
             Func<T, bool> predicate,
             Func<T, TResult> resultSelector);
    }

    struct StreamF<A, T>
    {
        public T Seed { get; set; }
        public Func<T, Tuple<T, A>> Generator { get; set; }
    }

    interface StreamFunction<A, TResult>
    {
        TResult Apply<T>(StreamF<A, T> n);
    }

    interface Stream<A>
    {
        TResult Apply<TResult>(StreamFunction<A, TResult> f);
    }

    class AnaStream<A, T> : Stream<A>
    {
        readonly StreamF<A, T> sf;

        public AnaStream(StreamF<A, T> sf)
        {
            this.sf = sf;
        }

        public R Apply<R>(StreamFunction<A, R> f)
        {
            return f.Apply<T>(sf);
        }

        //static Func<T, Stream<A>> Ana<A, T>(Func<T, Tuple<T, A>> generator)
        //{
        //    return seed => new AnaStream<A, T>(new StreamF<A, T>
        //    {
        //        Seed = seed,
        //        Generator = generator
        //    });
        //}
    }

    // Cf. http://blogs.msdn.com/b/wesdyer/archive/2007/02/02/anonymous-recursion-in-c.aspx
    delegate Func<TSource, TResult> Recursive<TSource, TResult>(Recursive<TSource, TResult> r);

    public static class Recursion
    {
        static Func<TSource, TResult> Y0<TSource, TResult>(Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Recursive<TSource, TResult> rec = r => _ => generator(r(r))(_);
            return rec(rec);
        }

        static Func<TSource, TResult> Y1<TSource, TResult>(Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Func<TSource, TResult> g = null;
            g = _ => generator(g)(_);

            return g;

        }

        static Func<TSource, TResult> Y2<TSource, TResult>(Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Func<TSource, TResult> g = null;
            g = generator(_ => g(_));

            return g;
        }

        static Func<int, int> Fibonacci()
        {
            return Y2<int, int>(f => n => n > 1 ? f(n - 1) + f(n - 2) : n);
        }

        static IEnumerable<int> Fibonacci_Lenses()
        {
            return Lenses.Unfold(
                _ => Tuple.Create(_.Item1, Tuple.Create(_.Item2, _.Item1 + _.Item2)),
                Tuple.Create(1, 1));
        }

        static IEnumerable<int> Fibonacci_Lenses2()
        {
            return Lenses.Ana<Tuple<int, int>, int>(
                _ => Tuple.Create(_.Item2, _.Item1 + _.Item2),
                Tuple.Create(1, 1),
                _ => _.Item1);
        }

        //public static Func<Tuple<TResult, Func<TResult>>> Ana<T, TResult>(Func<TResult, Tuple<T, TResult>> succ)
        //{
        //    throw new NotImplementedException();
        //}

        //public static Func<Tuple<TResult, Func<TResult>>> Ana<T, TResult>(
        //    Func<T, T> succ,
        //    T seed,
        //    Func<T, TResult> selector)
        //{
        //    return Ana(succ, seed, _ => true, selector);
        //}

        //public static Func<Tuple<T, Func<T>>> Return<T>(T value)
        //{
        //    return Repeat(value, 1);
        //}
    }
}
