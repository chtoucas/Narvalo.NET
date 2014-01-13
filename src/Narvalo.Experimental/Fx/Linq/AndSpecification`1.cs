namespace Narvalo.Fx.Linq
{
    using System;
    using System.Linq.Expressions;

    public sealed class AndSpecification<T> : SpecificationBase<T>
    {
        ISpecification<T> _left;
        ISpecification<T> _right;
        Expression<Func<T, bool>> _predicate;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
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
                    var param = Expression.Parameter(typeof(T), "p");
                    var left = Expression.Invoke(_left.Predicate, param);
                    var right = Expression.Invoke(_right.Predicate, param);
                    var combined = Expression.MakeBinary(ExpressionType.And, left, right);

                    _predicate = Expression.Lambda<Func<T, bool>>(combined);
                }

                return _predicate;
            }
        }
    }
}
