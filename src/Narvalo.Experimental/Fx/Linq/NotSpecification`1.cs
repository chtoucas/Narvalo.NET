namespace Narvalo.Fx.Linq
{
    using System;
    using System.Linq.Expressions;

    public sealed class NotSpecification<T> : SpecificationBase<T>
    {
        private ISpecification<T> _inner;
        private Expression<Func<T, bool>> _predicate;

        public NotSpecification(ISpecification<T> inner)
            : base()
        {
            _inner = inner;
        }

        public override Expression<Func<T, bool>> Predicate
        {
            get
            {
                if (_predicate == null) {
                    ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
                    InvocationExpression inner
                        = Expression.Invoke(_inner.Predicate, parameter);
                    UnaryExpression combined = Expression.Negate(inner);
                    _predicate = Expression.Lambda<Func<T, bool>>(combined);
                }
                return _predicate;
            }
        }
    }
}
