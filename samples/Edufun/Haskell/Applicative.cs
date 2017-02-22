// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define APPLICATIVE_USE_GHC_BASE

namespace Edufun.Haskell
{
    using System;

    using Narvalo.Fx;

    public partial class Applicative
    {
        public static Applicative<T> Of<T>(T value) { throw new FakeClassException(); }
    }

    public partial class Applicative<T> : IApplicative<T>
    {
        // Control.Applicative: <*>
        public Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative)
        {
            throw new FakeClassException();
        }

        // Control.Applicative: pure
        public Applicative<TSource> Of_<TSource>(TSource value) => Applicative.Of(value);
    }

    public partial class Applicative<T> : IApplicativeSyntax<T>
    {
        // [GHC.Base] (<*) = liftA2 const
        // Control.Applicative: u <* v = pure const <*> u <*> v
        public Applicative<T> Ignore<TOther>(Applicative<TOther> other)
        {
#if APPLICATIVE_USE_GHC_BASE
            Func<T, TOther, T> ignorethat = (_, that) => _;

            return Zip(other, ignorethat);
#else
            Func<T, Func<TOther, T>> ignorethat = _ => that => _;

            return other.Gather(Gather(Applicative.Of(ignorethat)));
#endif
        }

        // [GHC.Base] (<$) = fmap . const
        public Applicative<TResult> Replace<TResult>(TResult other) => Select(_ => other);

        // [GHC.Base] a1 *> a2 = (id <$ a1) <*> a2
        // Control.Applicative: u *> v = pure (const id) <*> u <*> v
        public Applicative<TResult> ReplaceBy<TResult>(Applicative<TResult> other)
        {
#if APPLICATIVE_USE_GHC_BASE
            Func<TResult, TResult> id = _ => _;

            return other.Gather(Replace(id));
#else
            Func<T, Func<TResult, TResult>> id = _ => r => r;

            return other.Gather(Gather(Applicative.Of(id)));
#endif
        }

        // [GHC.Base] liftA f a = pure f <*> a
        // Control.Applicative: fmap f x = pure f <*> x
        public Applicative<TResult> Select<TResult>(Func<T, TResult> selector)
            => Gather(Applicative.Of(selector));

        // [GHC.Base] liftA2 f a b = fmap f a <*> b
        public Applicative<TResult> Zip<TSecond, TResult>(
            Applicative<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Func<T, Func<TSecond, TResult>> selector = x => y => resultSelector.Invoke(x, y);

            return second.Gather(Select(selector));
        }

        // [GHC.Base] liftA3 f a b c = fmap f a <*> b <*> c
        public Applicative<TResult> Zip<T2, T3, TResult>(
            Applicative<T2> second,
            Applicative<T3> third,
            Func<T, T2, T3, TResult> resultSelector)
        {
            Func<T, Func<T2, Func<T3, TResult>>> selector = x => y => z => resultSelector.Invoke(x, y, z);

            return third.Gather(second.Gather(Select(selector)));
        }
    }

    public partial class Applicative : IApplicativeOperators
    {
        // [GHC.Base] (<**>) = liftA2 (flip ($))
        public Applicative<TResult> Apply<TSource, TResult>(
            Applicative<Func<TSource, TResult>> applicative,
            Applicative<TSource> value)
            => value.Gather(applicative);

        // [Data.Functor] (<$>) = fmap
        public Applicative<TResult> InvokeWith<TSource, TResult>(
            Func<TSource, TResult> func,
            Applicative<TSource> value)
            => value.Select(func);
    }
}
