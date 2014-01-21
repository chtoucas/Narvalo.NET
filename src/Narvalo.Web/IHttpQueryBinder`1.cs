namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.Web;

    public interface IHttpQueryBinder<TQuery>
    {
        IEnumerable<HttpQueryBinderException> BindingErrors { get; }
        
        bool Successful { get; }

        TQuery Bind(HttpRequest request);
    }
}
