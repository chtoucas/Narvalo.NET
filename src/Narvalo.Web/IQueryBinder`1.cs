namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.Web;

    public interface IQueryBinder<TQuery>
    {
        IReadOnlyCollection<QueryBinderException> BindingErrors { get; }
        
        bool Successful { get; }

        TQuery Bind(HttpRequest request);
    }
}
