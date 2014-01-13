namespace Narvalo.Fx.Linq
{
    using System;
    using System.Linq.Expressions;

    public abstract class SpecificationBase<T> : ISpecification<T>
    {
        protected SpecificationBase() { }

        public abstract Expression<Func<T, bool>> Predicate { get; }

        public bool IsSatisfiedBy(T candidate)
        {
            return Predicate.Compile().Invoke(candidate);
        }

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
