namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /* Monade Maybe
     * ============
     * 
     * Design de `Maybe<T>`
     * --------------------
     * 
     * ### Classe vs structure ###
     * 
     * + _Argument en faveur de l'utilisation d'une structure :_
     * C# garantit qu'une instance Maybe n'est jamais nulle, ce qui me semble être une bonne chose ici.
     * N'est-ce-pas, au départ, une des raisons d'exister de cette classe ?
     * 
     * + _Argument en défaveur de l'utilisation d'une structure :_
     * Une instance n'est pas immuable si `T` n'est pas de type valeur. Comme on construit cette classe 
     * principalement pour les types référence, on a un **gros** problème : une structure non immuable ouvre
     * presque systématiquement la porte à la création de bugs difficiles à appréhender.
     *   
     * Au final, créer une structure non immuable me semble être suffisamment dangereux 
     * pour ne pas poursuivre dans cette voie.
     *   
     * ### Ajout d'une contrainte de type référence ###
     * 
     * Pour les types valeur, `T?` fournit une bien meilleure alternative.
     * Je ne pense pas non plus que cela soit une bonne idée de rajouter une contrainte aussi restrictive.
     * Par exemple, on ne pourrait plus transformer un `Maybe<string>` en `Maybe<int>`. Afin de ne pas encourager 
     * l'utilisation d'un type Maybe quand un type nullable serait préférable, il faudrait créer une nouvelle règle FxCop.
     * 
     * Références
     * ----------
     * 
     * + [Wikipedia]: http://en.wikipedia.org/wiki/Monad_(functional_programming)#The_Maybe_monad
     * + [Haskell]: http://hackage.haskell.org/package/base-4.6.0.1/docs/Data-Maybe.html
     * 
     * Autres implémentations :
     * + [iSynaptic.Commons]: https://github.com/iSynaptic/iSynaptic.Commons
     */

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Il ne s'agit pas réellement d'une collection.")]
    public sealed partial class Maybe<T> : IEnumerable<T>, IEquatable<Maybe<T>>, IEquatable<T>
    {
        static readonly Maybe<T> None_ = new Maybe<T>();

        readonly bool _isSome;
        readonly T _value;

        /* Constructeur
         * ------------
         * 
         * La seule manière d'appeler le constructeur est via la méthode statique interne `Maybe<T>.η(value)` qui
         * garantit que `value` n'est jamais `null` quand on appelle le constructeur `Maybe.Maybe(value)`.
         * 
         * On expose deux méthodes et une propriété créationnelles :
         * + `Maybe.Create<T>(value)` qui est juste un alias pour `Maybe<T>.η(value)` ;
         * + `Maybe.Create<T?>(value)` pour les types nullable ;
         * + `Maybe<T>.None` qui est un alias pour `Maybe.Maybe()`.
         */

        Maybe()
        {
            _isSome = false;
        }

        Maybe(T value)
        {
            _value = value;
            _isSome = true;
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "Une version non générique n'améliorerait pas la lisibilité.")]
        public static Maybe<T> None { get { return None_; } }

        /// <summary>
        /// Retourne vrai si l'objet est vide, faux sinon.
        /// </summary>
        public bool IsNone { get { return !_isSome; } }

        /// <summary>
        /// Retourne vrai si l'objet contient une valeur, faux sinon.
        /// </summary>
        public bool IsSome { get { return _isSome; } }

        /// <summary>
        /// Retourne la valeur encapsulée.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">L'objet ne contient pas de valeur.</exception>
        public T Value
        {
            get
            {
                if (!_isSome) {
                    throw new InvalidOperationException(SR.Maybe_NoneHasNoValue);
                }

                return _value;
            }
        }

        /// <summary>
        /// Récupère la valeur encapsulée si elle existe, la valeur par défaut du type T sinon.
        /// </summary>
        /// <returns>La valeur sous-jacente ou la valeur par défaut.</returns>
        public T ValueOrDefault()
        {
            return _isSome ? _value : default(T);
        }

        /// <summary>
        /// Récupère la valeur encapsulée si elle existe, defaultValue sinon.
        /// </summary>
        /// <param name="defaultValue">Valeur à utiliser si l'objet ne contient pas de valeur.</param>
        /// <returns>La valeur sous-jacente ou defaultValue.</returns>
        public T ValueOrElse(T defaultValue)
        {
            return _isSome ? _value : defaultValue;
        }

        public T ValueOrElse(Func<T> defaultValueFactory)
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return _isSome ? _value : defaultValueFactory.Invoke();
        }

        public T ValueOrThrow(Exception exception)
        {
            Require.NotNull(exception, "exception");

            return ValueOrThrow(() => exception);
        }

        public T ValueOrThrow(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");

            if (!_isSome) {
                throw exceptionFactory.Invoke();
            }

            return _value;
        }

        /// <summary />
        public IEnumerator<T> GetEnumerator()
        {
            if (!_isSome) {
                return Enumerable.Empty<T>().GetEnumerator();
            }
            else {
                return new List<T> { _value }.GetEnumerator();
            }
        }

        /// <summary />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary />
        public override String ToString()
        {
            return _isSome ? _value.ToString() : "None";
        }
    }
}
