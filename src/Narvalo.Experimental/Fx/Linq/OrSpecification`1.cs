namespace Narvalo.Fx.Linq
{
    using System;
    using System.Linq.Expressions;

    public sealed class OrSpecification<T> : SpecificationBase<T>
    {
        private ISpecification<T> _left;
        private ISpecification<T> _right;
        private Expression<Func<T, bool>> _predicate;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
            : base()
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> Predicate
        {
            get
            {
                if (_predicate == null) {
                    ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
                    InvocationExpression callLeft
                        = Expression.Invoke(_left.Predicate, parameter);
                    InvocationExpression callRight
                        = Expression.Invoke(_right.Predicate, parameter);
                    BinaryExpression combined
                        = Expression.MakeBinary(ExpressionType.Or, callLeft, callRight);
                    _predicate = Expression.Lambda<Func<T, bool>>(combined);
                }

                return _predicate;
            }
        }
    }
}
