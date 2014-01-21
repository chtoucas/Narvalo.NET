namespace Narvalo.Web
{
    using System.Web;

    public interface IQueryBinder<TQuery>
    {
        bool CanValidate { get; }

        TQuery Bind(HttpRequest request);
        bool Validate();
    }
}
