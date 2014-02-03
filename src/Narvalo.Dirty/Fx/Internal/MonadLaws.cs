namespace Narvalo.Fx.Internal
{
    static class MonadLaws
    {
        #region > Les trois lois monadiques <

        // Première loi (identité à gauche):
        //      return x >>= f      ==  f x
        static bool FirstLaw<X, Y>(Kunc<X, Y> f, X value)
        {
            return Monad.Return(value).Bind(f) == f(value);
        }

        // Deuxième loi (identité à droite):
        //     m >>= return        ==  m
        static bool SecondLaw<X>(Monad<X> m)
        {
            return m.Bind(Monad.Return) == m;
        }

        // Troisième loi (associativité):
        //      (m >>= f) >>= g     ==  m >>= (\x -> f x >>= g)
        static bool ThirdLaw<X, Y, Z>(Kunc<X, Y> g, Kunc<Y, Z> f, Monad<X> m)
        {
            return m.Bind(g).Bind(f) == m.Bind(_ => g(_).Bind(f));
            //return Bind(f, Bind(g, m)) == Bind(_ => Bind(f, g(_)), m);
        }

        #endregion

        #region > Catégorie de Kleisli <

        // Même si cela ne semble pas évident, cela équivaut à dire :
        //  - Unit est l'élément neutre
        //  - L'opération de composition est associative.

        // Unit * f = f
        static bool LeftIdentity<X, Y>(Kunc<X, Y> f, X value)
        {
            return Monad.Compose(Monad.Return, f, value) == f(value);
        }

        // f * Unit = f
        static bool RightIdentity<X, Y>(Kunc<X, Y> f, X value)
        {
            return Monad.Compose(f, Monad.Return, value) == f(value);
        }

        // h * (g * f) = (h * g) * f
        static bool Associativity<X, Y, Z, T>(
            Kunc<X, Y> f,
            Kunc<Y, Z> g,
            Kunc<Z, T> h,
            X value)
        {
            return Monad.Compose(_ => Monad.Compose(f, g, _), h, value)
                == Monad.Compose(f, _ => Monad.Compose(g, h, _), value);
        }

        #endregion
    }
}
