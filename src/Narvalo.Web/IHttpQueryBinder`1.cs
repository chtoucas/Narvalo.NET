namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.Web;
    using Narvalo.Fx;

    public interface IHttpQueryBinder<TQuery>
    {
        IEnumerable<HttpQueryBinderException> BindingErrors { get; }
        
        Maybe<TQuery> Bind(HttpRequest request);
    }
}
