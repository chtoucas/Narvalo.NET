namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    // NB: par convention, quand Either est utilisé pour représenter une valeur soit correcte soit 
    // incorrecte, Left contient la valeur en cas d'erreur, et Right contient la valeur en cas de succès.
    public abstract class Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
    {
        readonly bool _isLeft;
        readonly TLeft _left;
        readonly TRight _right;

        Either(TLeft left)
        {
            _isLeft = true;
            _left = left;
            _right = default(TRight);
        }

        Either(TRight right)
        {
            _isLeft = false;
            _left = default(TLeft);
            _right = right;
        }

        public bool IsLeft { get { return _isLeft; } }

        public bool IsRight { get { return !_isLeft; } }

        public TLeft LeftValue { get { return _left; } }

        public TRight RightValue { get { return _right; } }

        public abstract TResult Switch<TResult>(
            Func<TLeft, TResult> caseLeft,
            Func<TRight, TResult> caseRight);

        public abstract void Switch(
            Action<TLeft> caseLeft,
            Action<TRight> caseRight);

        #region > Opérations monadiques <

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public Either<TLeft, TResult> Bind<TResult>(
            Func<TRight, Either<TLeft, TResult>> fun)
        {
            Requires.NotNull(fun, "fun");

            return _isLeft ? Either<TLeft, TResult>.Left(_left) : fun(_right);
        }

        public Either<TResult, TRight> MapLeft<TResult>(
           Func<TLeft, TResult> selector)
        {
            Requires.NotNull(selector, "selector");

            return _isLeft
               ? Either<TResult, TRight>.Left(selector(_left))
               : Either<TResult, TRight>.Right(_right);
        }

        public Either<TLeft, TResult> MapRight<TResult>(
            Func<TRight, TResult> selector)
        {
            Requires.NotNull(selector, "selector");

            return _isLeft
                ? Either<TLeft, TResult>.Left(_left)
                : Either<TLeft, TResult>.Right(selector(_right));
        }

        #endregion

        public static Either<TLeft, TRight> Left(TLeft left)
        {
            return new Either<TLeft, TRight>.LeftImpl(left);
        }

        public static Either<TLeft, TRight> Right(TRight right)
        {
            return new Either<TLeft, TRight>.RightImpl(right);
        }

        public sealed class LeftImpl : Either<TLeft, TRight>, IEquatable<LeftImpl>
        {
            public LeftImpl(TLeft value) : base(value) { }

            public TLeft Value { get { return LeftValue; } }

            public override TResult Switch<TResult>(
                Func<TLeft, TResult> caseLeft,
                Func<TRight, TResult> caseRight)
            {
                return caseLeft(LeftValue);
            }

            public override void Switch(
                Action<TLeft> caseLeft,
                Action<TRight> caseRight)
            {
                caseLeft(LeftValue);
            }

            #region IEquatable<Left>

            public bool Equals(LeftImpl other)
            {
                if (other == this) { return true; }
                if (other == null) { return false; }

                return EqualityComparer<TLeft>.Default.Equals(LeftValue, other.LeftValue);
            }

            #endregion

            public override bool Equals(object obj)
            {
                return Equals(obj as LeftImpl);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<TLeft>.Default.GetHashCode(LeftValue);
            }

            public override string ToString()
            {
                return String.Format(CultureInfo.CurrentCulture, "Left({0})", LeftValue);
            }
        }

        public sealed class RightImpl : Either<TLeft, TRight>, IEquatable<RightImpl>
        {
            public RightImpl(TRight value) : base(value) { }

            public TRight Value { get { return RightValue; } }

            public override TResult Switch<TResult>(
                Func<TLeft, TResult> caseLeft,
                Func<TRight, TResult> caseRight)
            {
                return caseRight(RightValue);
            }

            public override void Switch(
                Action<TLeft> caseLeft,
                Action<TRight> caseRight)
            {
                caseRight(RightValue);
            }

            #region IEquatable<Right>

            public bool Equals(RightImpl other)
            {
                if (other == this) { return true; }
                if (other == null) { return false; }

                return EqualityComparer<TRight>.Default.Equals(RightValue, other.RightValue);
            }

            #endregion

            public override bool Equals(object obj)
            {
                return Equals(obj as RightImpl);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<TRight>.Default.GetHashCode(RightValue);
            }

            public override string ToString()
            {
                return String.Format(CultureInfo.CurrentCulture, "Right({0})", RightValue);
            }
        }

        #region IEquatable<Either<TLeft,TRight>>

        public bool Equals(Either<TLeft, TRight> other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
