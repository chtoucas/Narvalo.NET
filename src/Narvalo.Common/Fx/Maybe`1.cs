namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Il ne s'agit pas réellement d'une collection.")]
    public partial struct Maybe<T> : IEnumerable<T>, IEquatable<Maybe<T>>, IEquatable<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Une version non générique n'améliorerait pas la compréhension de la méthode.")]
        public static readonly Maybe<T> None = default(Maybe<T>);

        readonly bool _isSome;
        readonly T _value;

        Maybe(T value)
        {
            // NB: La seule manière d'appeler le constructeur est via la méthode Maybe<T>.η() 
            // qui se charge de vérifier que "value" n'est pas null.
            _value = value;
            _isSome = true;
        }

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

            if (!_isSome) {
                throw exception;
            }
            
            return _value;
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
