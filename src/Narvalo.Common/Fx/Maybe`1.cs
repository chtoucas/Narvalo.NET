namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public partial struct Maybe<T> : IEnumerable<T>, IEquatable<Maybe<T>>, IEquatable<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static readonly Maybe<T> None = default(Maybe<T>);

        readonly bool _isSome;
        readonly T _value;

        Maybe(T value)
        {
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

        #region > Extraction de la valeur sous-jacente <

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
            Requires.NotNull(defaultValueFactory, "defaultValueFactory");

            return _isSome ? _value : defaultValueFactory();
        }

        public T ValueOrThrow(Exception ex)
        {
            Requires.NotNull(ex, "ex");

            if (!_isSome) {
                throw ex;
            }
            return _value;
        }

        public T ValueOrThrow(Func<Exception> exceptionFactory)
        {
            Requires.NotNull(exceptionFactory, "exceptionFactory");

            if (!_isSome) {
                throw exceptionFactory();
            }
            return _value;
        }

        #endregion

        #region IEnumerable<T>

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

        #endregion

        #region IEnumerable

        /// <summary />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEquatable<T>

        /// <summary />
        public bool Equals(T other)
        {
            if (!_isSome) { return false; }

            return _value.Equals(other);
        }

        #endregion

        #region IEquatable<Maybe<T>>

        /// <summary />
        public bool Equals(Maybe<T> other)
        {
            if (_isSome != other._isSome) { return false; }
            return
                // Les deux options sont vides.
                !_isSome
                // Les deux options ont la même valeur.
                || _value.Equals(other._value);
        }

        #endregion

        #region > Surchages d'Object <

        /// <summary />
        public override bool Equals(object obj)
        {
            if (!_isSome && obj is Unit) {
                return true;
            }
            if (!(obj is Maybe<T>)) {
                return false;
            }

            return Equals((Maybe<T>)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            return _isSome ? _value.GetHashCode() : 0;
        }

        /// <summary />
        public override String ToString()
        {
            return _isSome ? _value.ToString() : "None";
        }

        #endregion

        #region > Opérateurs <

        /// <summary />
        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return !left.Equals(right);
        }

        ///// <summary />
        //[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        //public static implicit operator Maybe<T>(Maybe<Unit> value)
        //{
        //    return Maybe.None;
        //}

        #endregion
    }
}
