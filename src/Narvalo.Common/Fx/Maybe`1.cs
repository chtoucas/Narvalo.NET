// Copyright (c) 2014, Narvalo.Org
// All rights reserved.

// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met:

// 1. Redistributions of source code must retain the above copyright notice,
// this list of conditions and the following disclaimer.

// 2. Redistributions in binary form must reproduce the above copyright notice,
// this list of conditions and the following disclaimer in the documentation 
// and/or other materials provided with the distribution.

// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

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
     * C# garantirait qu'une instance Maybe n'est jamais nulle, ce qui me semble être une bonne chose ici.
     * N'est-ce-pas, au départ, une des raisons d'exister de cette classe ?
     * 
     * + _Argument en défaveur de l'utilisation d'une structure :_
     * Une instance n'est pas immuable si `T` n'est pas de type valeur. On a un **gros** problème : 
     * une structure non immuable ouvre presque systématiquement la porte à la création de bugs difficiles à tracer.
     *   
     * Au final, créer une structure non immuable me semble être suffisamment dangereux 
     * pour ne pas poursuivre dans cette voie.
     *   
     * ### Contrainte de type ###
     * 
     * Pour les types valeur, `T?` fournit _le plus souvent_ une bien meilleure alternative. Afin de ne pas encourager 
     * l'utilisation d'un type Maybe quand un type nullable serait préférable, il faudrait créer une nouvelle règle FxCop.
     * 
     * ### Egalité structurelle ###
     * Se reporter à la section dédiée à ce sujet.
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

    /// <summary>
    /// Represents a value that is either a single value of type T, or no value at all.
    /// </summary>
    /// <typeparam name="T">The type of the underlying value.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Il ne s'agit pas réellement d'une collection.")]
    public sealed partial class Maybe<T> : IEnumerable<T>, IEquatable<Maybe<T>>
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

        /// <summary>
        /// Initializes a new instance of <see cref="Narvalo.Fx.Maybe{T}" /> that does not hold any value.
        /// </summary>
        Maybe()
        {
            _isSome = false;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Narvalo.Fx.Maybe{T}" /> using the specified value. 
        /// </summary>
        /// <param name="value">The underlying value.</param>
        Maybe(T value)
        {
            _value = value;
            _isSome = true;
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "Une version non générique n'améliorerait pas la lisibilité.")]
        public static Maybe<T> None { get { return None_; } }

        /// <summary>
        /// Returns true if the object does not have an underlying value, false otherwise.
        /// </summary>
        public bool IsNone { get { return !IsSome; } }

        /// <summary>
        /// Returns true if the object contains a value, false otherwise.
        /// </summary>
        public bool IsSome { get { return _isSome; } }

        /// <summary>
        /// Returns the underlying value.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The object does not contain any value.</exception>
        public T Value
        {
            get
            {
                if (!IsSome) {
                    throw new InvalidOperationException(SR.Maybe_NoneHasNoValue);
                }

                return _value;
            }
        }

        /// <summary>
        /// Returns the underlying value if any, the default value of the type T otherwise.
        /// </summary>
        /// <returns>The underlying value or the default value of the type T.</returns>
        public T ValueOrDefault()
        {
            return IsSome ? _value : default(T);
        }

        /// <summary>
        /// Returns the underlying value if any, defaultValue otherwise.
        /// </summary>
        /// <param name="defaultValue">A default value to be used if if there is no underlying value.</param>
        /// <returns>The underlying value or defaultValue.</returns>
        public T ValueOrElse(T defaultValue)
        {
            return IsSome ? _value : defaultValue;
        }

        public T ValueOrElse(Func<T> defaultValueFactory)
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return IsSome ? _value : defaultValueFactory.Invoke();
        }

        public T ValueOrThrow(Exception exception)
        {
            Require.NotNull(exception, "exception");

            return ValueOrThrow(() => exception);
        }

        public T ValueOrThrow(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");

            if (IsNone) {
                throw exceptionFactory.Invoke();
            }

            return _value;
        }

        /// <summary />
        public IEnumerator<T> GetEnumerator()
        {
            if (IsNone) {
                return Enumerable.Empty<T>().GetEnumerator();
            }
            else {
                return new List<T> { Value }.GetEnumerator();
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
            return IsSome ? _value.ToString() : "None";
        }
    }
}
