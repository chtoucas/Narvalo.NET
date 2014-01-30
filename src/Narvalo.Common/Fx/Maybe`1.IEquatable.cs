namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /* Egalité référentielle et égalité structurelle
     * ---------------------------------------------
     * 
     * On redéfinit la méthode Equals() afin qu'elle suive les mêmes règles que les objets de type valeur.
     * Par contre, on ne touche pas aux opérateurs d'égalité (== et !=) qui continuent donc à tester l'égalité 
     * référentielle, comportement attendu par le framework .NET.
     * Une autre possibilité, abandonnée, aurait été d'utiliser l'interface IStructuralEquatable.
     * 
     * J'ai pensé implémenter l'interface IEquatable<T> mais cela pose plus de problème qu'autre chose.
     * On aurait créé une méthode qui aurait ressembler à ce qui suit :
     * '''
     * public bool Equals(T other, IEqualityComparer<T> comparer)
     * {
     *   // NB: Maybe<T>.None.Equals((T)null) retourne false. Pour un object de type valeur, c'est normal.
     *   // Par contre, pour un objet de type référence, cela peut paraître incohérent puisque Maybe<T>.None
     *   // encapsule la valeur null...
     *   if (ReferenceEquals(other, null)) {
     *     return false;
     *   }
     *
     *   return Equals(new Maybe<T>(other));
     * }
     * '''
     * et il aurait aussi fallu modifier Equals(object obj) en intercalant un test dans le genre :
     * '''
     * if (obj is T) {
     *   return Equals((T)obj);
     * }
     * '''
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
            // "this" n'est jamais null.
            if (ReferenceEquals(other, null)) {
                return false;
            }

            if (IsNone) {
                return other.IsNone;
            }

            // Les deux options contiennent la même valeur.
            return comparer.Equals(_value, other._value);
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

            // "this" n'est jamais null.
            if (ReferenceEquals(other, null)) {
                return false;
            }

            // Habituellement, on teste obj.GetType() == this.GetType() au cas où "this" ou "obj" 
            // serait une instance d'une classe dérivée. Comme Maybe<T> est fermée à l'extensibilité, 
            // on n'a pas ce problème.
            return Equals(other as Maybe<T>);
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
