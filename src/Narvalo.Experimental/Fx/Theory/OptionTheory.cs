namespace Narvalo.Fx.Theory
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Fx;

    public partial class OptionTheory
    {
        // Deux manières de définir un monad
        // - Unit
        // - Bind
        // ou
        // - Unit
        // - Map
        // - Join

        #region + Unit +

        // Transformation naturelle Unit.
        //static Option<X> Unit<X>(X value)
        //{
        //    return Option<X>.Monad.η(value);
        //}

        #endregion

        #region + Bind +

        //static Option<Y> Bind<X, Y>(Func<X, Option<Y>> f, Option<X> m)
        //{
        //    return m.Bind(f);
        //}

        //// Transformation naturelle Multiplication.
        //static Option<X> Multiply<X>(Option<Option<X>> m)
        //{
        //    return Bind(_ => _, m);
        //}

        //// Opération fonctorielle sur les morphismes.
        //static Option<Y> Map<X, Y>(Func<X, Y> f, Option<X> m)
        //{
        //    return Bind(_ => Unit(f(_)), m);
        //}

        //// Composition de Kleisli.
        //static Option<Z> Compose<X, Y, Z>(
        //    Func<Y, Option<Z>> g,
        //    Func<X, Option<Y>> f,
        //    X value)
        //{
        //    return Bind(g, f(value));
        //}

        #endregion

        #region + Join & Map +

        //static Option<Y> Bind_<X, Y>(
        //   Func<X, Option<Y>> f,
        //   Option<X> m)
        //{
        //    return Join_(Map_(f, m));
        //}

        //// Transformation naturelle Multiplication.
        //static Option<X> Multiply_<X>(Option<Option<X>> m)
        //{
        //    return m.IsSome ? m.Value : Option.None;
        //}

        //// Opération fonctorielle sur les morphismes.
        //static Option<Y> Map_<X, Y>(Func<X, Y> f, Option<X> m)
        //{
        //    return m.IsSome ? Unit(f(m.Value)) : Option.None;
        //}

        //// Composition de Kleisli.
        //static Option<Z> Compose_<X, Y, Z>(
        //    Func<Y, Option<Z>> g,
        //    Func<X, Option<Y>> f,
        //    X value)
        //{
        //    return Multiply_(Map_(g, f(value)));
        //}

        #endregion

        #region + Lift +

        //static Option<R> Lift<X, Y, R>(
        //   Func<X, Y, R> f,
        //   Option<X> m1,
        //   Option<Y> m2)
        //{
        //    Func<X, Option<R>> g = _ => Map(y => f(_, y), m2);

        //    return Bind(g, m1);
        //}

        //static Option<R> Lift<X, Y, Z, R>(
        //    Func<X, Y, Z, R> f,
        //    Option<X> m1,
        //    Option<Y> m2,
        //    Option<Z> m3)
        //{
        //    Func<X, Option<R>> g = _ => Lift((y, z) => f(_, y, z), m2, m3);

        //    return Bind(g, m1);
        //}

        //static Option<R> Lift<X, Y, Z, T, R>(
        //    Func<X, Y, Z, T, R> f,
        //    Option<X> m1,
        //    Option<Y> m2,
        //    Option<Z> m3,
        //    Option<T> m4)
        //{
        //    Func<X, Option<R>> g = _ => Lift((y, z, t) => f(_, y, z, t), m2, m3, m4);

        //    return Bind(g, m1);
        //}

        //public static Option<R> Lift<X, Y, Z, T, U, R>(
        //   Func<X, Y, Z, T, U, R> f,
        //   Option<X> m1,
        //   Option<Y> m2,
        //   Option<Z> m3,
        //   Option<T> m4,
        //   Option<U> m5)
        //{
        //    Func<X, Option<R>> g = _ => Lift((y, z, t, u) => f(_, y, z, t, u), m2, m3, m4, m5);

        //    return Bind(g, m1);
        //}

        #endregion

        #region Les trois lois monadiques

        // Première loi (identité à gauche):
        //      return x >>= f      ==  f x
        static bool Monad_FirstLaw<X, Y>(
            Func<X, Option<Y>> f,
            X value)
        {
            return Bind(f, Unit(value)) == f(value);
        }

        // Deuxième loi (identité à droite):
        //     m >>= return        ==  m
        static bool Monad_SecondLaw<X>(Option<X> m)
        {
            return Bind(Unit, m) == m;
        }

        // Troisième loi (associativité):
        //      (m >>= f) >>= g     ==  m >>= (\x -> f x >>= g)
        static bool Monad_ThirdLaw<X, Y, Z>(
            Func<X, Option<Y>> g,
            Func<Y, Option<Z>> f,
            Option<X> m)
        {
            return Bind(f, Bind(g, m)) == Bind(_ => Bind(f, g(_)), m);
        }

        #endregion

        #region Catégorie de Kleisli.

        // Même si cela ne semble pas évident, cela équivaut à dire :
        //  - Unit est l'élément neutre
        //  - L'opération de composition est associative.

        // Unit * f = f
        static bool Kleisli_LeftIdentity<X, Y>(
            Func<X, Option<Y>> f,
            X value)
        {
            return Compose(Unit, f, value) == f(value);
        }

        // f * Unit = f
        static bool Kleisli_RightIdentity<X, Y>(
            Func<X, Option<Y>> f,
            X value)
        {
            return Compose(f, Unit, value) == f(value);
        }

        // h * (g * f) = (h * g) * f
        static bool Kleisli_Associativity<X, Y, Z, T>(
            Func<Z, Option<T>> h,
            Func<Y, Option<Z>> g,
            Func<X, Option<Y>> f,
            X value)
        {
            return Compose(h, _ => Compose(g, f, _), value)
                == Compose(_ => Compose(h, g, _), f, value);
        }

        #endregion
    }
}
