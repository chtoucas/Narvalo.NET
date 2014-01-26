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
     * J'hésite encore concernant les deux points suivants.
     * 
     * ### Classe vs structure ###
     * 
     * Arguments en faveur de l'utilisation d'une structure :
     * C# garantit qu'une instance Maybe n'est jamais nulle, ce qui me semble être une bonne chose ici.
     * N'est-ce-pas, au départ, la raison d'exister de cette classe ?
     * 
     * Arguments en défaveur de l'utilisation d'une structure :
     * Une instance n'est pas immuable si `T` n'est pas de type valeur. Comme on construit cette classe 
     * principalement pour les types référence, on a un GROS problème ; 
     *   
     * Conclusion provisoire : 
     * Créer une structure non immuable me semble être suffisamment dangereux pour ne pas choisir cette voie.
     *   
     * ### Ajouter une contrainte sur le type générique ###
     * Pour les types valeur, `T?` fournit une bien meilleure alternative.
     * Je ne pense pas non plus que cela soit une bonne idée de rajouter une contrainte aussi restrictive.
     * Par exemple, on ne pourrait plus transformer un `Maybe<string>` en `Maybe<int>`. Afin de ne pas encourager 
     * l'utilisation d'un type Maybe quand un type nullable serait préférable, il faudrait créer une nouvelle règle FxCop.
     */

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Il ne s'agit pas réellement d'une collection.")]
    public sealed partial class Maybe<T> : IEnumerable<T>, IEquatable<Maybe<T>>, IEquatable<T>
    {
        static readonly Maybe<T> None_ = new Maybe<T>();

        readonly bool _isSome;
        readonly T _value;

        Maybe()
        {
            _isSome = false;
        }

        Maybe(T value)
        {
            // NB: La seule manière d'appeler le constructeur est via la méthode Maybe<T>.η() 
            // qui se charge de vérifier que "value" n'est pas null.
            _value = value;
            _isSome = true;
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Une version non générique n'améliorerait pas la compréhension de la méthode.")]
        public static Maybe<T> None { get { return None_; } }

        public bool IsNone { get { return !_isSome; } }

        public bool IsSome { get { return _isSome; } }

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

        public T ValueOrDefault()
        {
            return _isSome ? _value : default(T);
        }

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
        public override String ToString()
        {
            return _isSome ? _value.ToString() : "None";
        }

        /// <summary />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
