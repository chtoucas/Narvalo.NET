// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;

    public partial class Applicative
    {
        public static Applicative<T> Of<T>(T value) { throw new FakeClassException(); }
    }

    public partial class Applicative<T> : IApplicative<T>
    {
        public Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative)
        {
            throw new FakeClassException();
        }

        public Applicative<TSource> Of_<TSource>(TSource value) => Applicative.Of(value);
    }

    public partial class Applicative<T> : IApplicativeSyntax<T>
    {
        // [GHC.Base] (<*) = liftA2 const
        // [Control.Applicative] u <* v = pure const <*> u <*> v
        public Applicative<T> Ignore<TResult>(Applicative<TResult> other)
        {
            //Func<TResult, Applicative<T>> me = _ => this;
            //Func<T, Func<TResult, Applicative<T>>> always1 = _ => me;

            //var applicative = Applicative.Of(always1);
            //Applicative<Func<TResult, Applicative<T>>> second = this.Gather(applicative);
            //Applicative<Applicative<T>> third = other.Gather(second);

            throw new NotImplementedException();
        }

        // Same as Functor<T>.Replace()
        public Applicative<TResult> Replace<TResult>(TResult other) => Select(_ => other);

        // [GHC.Base] a1 *> a2 = (id <$ a1) <*> a2
        // [Control.Applicative] u *> v = pure (const id) <*> u <*> v
        public Applicative<TResult> ReplaceBy<TResult>(Applicative<TResult> other)
        {
            Func<TResult, TResult> id = _ => _;
            Func<T, Func<TResult, TResult>> value = _ => id;

            var applicative = Applicative.Of(value);
            Applicative<Func<TResult, TResult>> second = this.Gather(applicative);

            return other.Gather(second);
        }

        // [GHC.Base] liftA f a = pure f <*> a
        // [Control.Applicative] fmap f x = pure f <*> x
        public Applicative<TResult> Select<TResult>(Func<T, TResult> selector)
            => Gather(Applicative.Of(selector));

        // [GHC.Base] liftA2 f a b = fmap f a <*> b
        public Applicative<TResult> Zip<TSecond, TResult>(
            Applicative<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Func<T, Func<TSecond, TResult>> selector
                = (T x) => (TSecond y) => resultSelector.Invoke(x, y);

            return second.Gather(Select(selector));
        }

        // [GHC.Base] liftA3 f a b c = fmap f a <*> b <*> c
        public Applicative<TResult> Zip<T2, T3, TResult>(
            Applicative<T2> second,
            Applicative<T3> third,
            Func<T, T2, T3, TResult> resultSelector)
        {
            Func<T, Func<T2, Func<T3, TResult>>> selector
                = (T x) => (T2 y) => (T3 z) => resultSelector.Invoke(x, y, z);

            Applicative<Func<T3, TResult>> app = second.Gather(Select(selector));

            return third.Gather(app);
        }
    }

    public partial class Applicative : IApplicativeOperators
    {
        // GHC.Base: (<**>) = liftA2 (flip ($))
        public Applicative<TResult> Apply<TSource, TResult>(
            Applicative<Func<TSource, TResult>> @this,
            Applicative<TSource> value)
            => value.Gather(@this);

        // Data.Functor: (<$>) = fmap
        public Applicative<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> selector, Applicative<TSource> value)
            => value.Select(selector);
    }
}
