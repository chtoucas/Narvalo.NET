namespace Narvalo.Fx
{
    using System;

    // NB: Par convention, quand Either est utilisé pour représenter une valeur soit correcte soit 
    // incorrecte, Left contient la valeur en cas d'erreur, et Right contient la valeur en cas de succès.
    public abstract class EitherBase<TLeft, TRight>
    {
        readonly bool _isLeft;
        readonly TLeft _left;
        readonly TRight _right;

        protected EitherBase(TLeft left)
        {
            _isLeft = true;
            _left = left;
            _right = default(TRight);
        }

        protected EitherBase(TRight right)
        {
            _isLeft = false;
            _left = default(TLeft);
            _right = right;
        }

        protected bool IsLeft { get { return _isLeft; } }

        protected bool IsRight { get { return !_isLeft; } }

        protected TLeft LeftValue
        {
            get
            {
                if (!_isLeft) {
                    throw new InvalidOperationException(SR.EitherBase_RightHasNoLeftValue);
                }

                return _left;
            }
        }

        protected TRight RightValue
        {
            get
            {
                if (_isLeft) {
                    throw new InvalidOperationException(SR.EitherBase_LeftHasNoRightValue);
                }

                return _right;
            }
        }
    }
}
