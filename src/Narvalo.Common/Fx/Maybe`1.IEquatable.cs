namespace Narvalo.Fx
{
    using System.Collections.Generic;

    /* Egalité référentielle et égalité structurelle
     * ---------------------------------------------
     * 
     * On redéfinit la méthode Equals() afin qu'elle suive les règles d'égalité structurelle.
     * Par contre, on ne touche pas aux opérateurs d'égalité (== et !=) qui continuent donc à tester l'égalité 
     * référentielle, comportement en ligne avec celui du framework .NET, d'autant plus qu'un Maybe<T> n'est
     * en général pas immuable.
     * Une autre possibilité, abandonnée, aurait été d'implémenter l'interface IStructuralEquatable.
     */

    public partial class Maybe<T>
    {
        /// <summary />
        public bool Equals(Maybe<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        /// <summary />
        public bool Equals(Maybe<T> other, IEqualityComparer<T> comparer)
        {
            if (ReferenceEquals(other, null)) {
                return IsNone;
            }

            if (IsNone) {
                return other.IsNone;
            }

            // Les deux options contiennent la même valeur.
            return comparer.Equals(_value, other._value);
        }

        public bool Equals(T other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(T other, IEqualityComparer<T> comparer)
        {
            return Equals(Maybe.Create(other), comparer);
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            return Equals(obj, EqualityComparer<T>.Default);
        }

        /// <summary />
        public bool Equals(object other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            if (other == null) {
                return IsNone;
            }

            if (other is T) {
                return Equals((T)other, comparer);
            }

            // Habituellement, on teste obj.GetType() == this.GetType() au cas où "this" ou "obj" 
            // serait une instance d'une classe dérivée. Comme Maybe<T> est fermée à l'extensibilité, 
            // on n'a pas ce problème.
            return Equals(other as Maybe<T>, comparer);
        }

        /// <summary />
        public override int GetHashCode()
        {
            return GetHashCode(EqualityComparer<T>.Default);
        }

        /// <summary />
        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            return ReferenceEquals(_value, null) ? 0 : comparer.GetHashCode(_value);
        }
    }
}
