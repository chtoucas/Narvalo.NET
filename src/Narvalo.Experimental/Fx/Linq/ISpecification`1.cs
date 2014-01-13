namespace Narvalo.Fx.Linq
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// ISpecification<Buddy> IsVendor = new Specification<Buddy>(b => b.Kind == "Vendor");
    /// ISpecification<Buddy> IsFrench = new Specification<Buddy>(b => b.Country == "FR");
    /// List<Buddy> frenchVendors = 
    ///     (from b in Buddies where IsVendor.And(IsFrench).IsSatisfiedBy(b) select b).ToList();
    /// </example>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Predicate { get; }

        bool IsSatisfiedBy(T candidate);

        ISpecification<T> And(ISpecification<T> other);

        ISpecification<T> Or(ISpecification<T> other);

        ISpecification<T> Not();
    }
}
