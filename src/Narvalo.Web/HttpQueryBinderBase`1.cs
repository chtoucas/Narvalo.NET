namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Narvalo.Linq;

    public abstract class HttpQueryBinderBase<TQuery> : IHttpQueryBinder<TQuery>
    {
        readonly IList<HttpQueryBinderException> _errors = new List<HttpQueryBinderException>();

        bool _successful = false;

        protected HttpQueryBinderBase() { }

        public IEnumerable<HttpQueryBinderException> BindingErrors { get { return _errors; } }

        public bool Successful { get { return _successful; } }

        public TQuery Bind(HttpRequest request)
        {
            Require.NotNull(request, "request");

            var query = BindCore(request);

            _successful = Validate(query);

            return query;
        }

        protected abstract TQuery BindCore(HttpRequest request);

        protected virtual bool Validate(TQuery query)
        {
            DebugCheck.NotNull(query);

            return (from prop in TypeDescriptor.GetProperties(query).Cast<PropertyDescriptor>()
                    from attr in prop.Attributes.OfType<ValidationAttribute>()
                    where !attr.IsValid(prop.GetValue(query))
                    select attr).IsEmpty();
        }

        protected void AddError(HttpQueryBinderException exception)
        {
            _errors.Add(exception);
        }
    }
}
