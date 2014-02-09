// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Globalization;

    // NB: Par convention, quand Either est utilisé pour représenter une valeur soit correcte soit 
    // incorrecte, Left contient la valeur en cas d'erreur, et Right contient la valeur en cas de succès.
    public abstract partial class Either<TLeft, TRight>
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

        public TLeft LeftValue
        {
            get
            {
                if (!_isLeft) {
                    throw new InvalidOperationException(SR.Either_RightHasNoLeftValue);
                }
                return _left;
            }
        }

        public TRight RightValue
        {
            get
            {
                if (_isLeft) {
                    throw new InvalidOperationException(SR.Either_LeftHasNoRightValue);
                }
                return _right;
            }
        }

        public Either<TLeft, TResult> Bind<TResult>(
            Func<TRight, Either<TLeft, TResult>> fun)
        {
            Require.NotNull(fun, "fun");

            return _isLeft ? Either<TLeft, TResult>.Left(_left) : fun(_right);
        }

        public Either<TResult, TRight> MapLeft<TResult>(
           Func<TLeft, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return _isLeft
               ? Either<TResult, TRight>.Left(selector(_left))
               : Either<TResult, TRight>.Right(_right);
        }

        public Either<TLeft, TResult> MapRight<TResult>(
            Func<TRight, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return _isLeft
                ? Either<TLeft, TResult>.Left(_left)
                : Either<TLeft, TResult>.Right(selector(_right));
        }

        public static Either<TLeft, TRight> Left(TLeft value)
        {
            return new LeftImpl(value);
        }

        public static Either<TLeft, TRight> Right(TRight value)
        {
            return new RightImpl(value);
        }

        sealed class LeftImpl : Either<TLeft, TRight>
        {
            public LeftImpl(TLeft value) : base(value) { }

            //public TLeft Value { get { return LeftValue; } }

            //public override TResult Switch<TResult>(
            //    Func<TLeft, TResult> caseLeft,
            //    Func<TRight, TResult> caseRight)
            //{
            //    return caseLeft(LeftValue);
            //}

            //public override void Switch(
            //    Action<TLeft> caseLeft,
            //    Action<TRight> caseRight)
            //{
            //    caseLeft(LeftValue);
            //}

            public override string ToString()
            {
                return Format.CurrentCulture("Left({0})", LeftValue);
            }
        }

        sealed class RightImpl : Either<TLeft, TRight>
        {
            public RightImpl(TRight value) : base(value) { }

            //public TRight Value { get { return RightValue; } }

            //public override TResult Switch<TResult>(
            //    Func<TLeft, TResult> caseLeft,
            //    Func<TRight, TResult> caseRight)
            //{
            //    return caseRight(RightValue);
            //}

            //public override void Switch(
            //    Action<TLeft> caseLeft,
            //    Action<TRight> caseRight)
            //{
            //    caseRight(RightValue);
            //}

            public override string ToString()
            {
                return Format.CurrentCulture("Right({0})", RightValue);
            }
        }

        //    public abstract TResult Switch<TResult>(
        //        Func<TLeft, TResult> caseLeft,
        //        Func<TRight, TResult> caseRight);

        //    public abstract void Switch(
        //        Action<TLeft> caseLeft,
        //        Action<TRight> caseRight);
    }
}
