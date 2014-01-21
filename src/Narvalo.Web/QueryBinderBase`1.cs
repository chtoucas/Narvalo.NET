namespace Narvalo.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public abstract class QueryBinderBase<TQuery> : IQueryBinder<TQuery>
    {
        readonly IList<QueryBinderException> _errors = new List<QueryBinderException>();

        bool _successful = false;

        protected QueryBinderBase() { }

        public IReadOnlyCollection<QueryBinderException> BindingErrors
        {
            get { return new ReadOnlyCollection<QueryBinderException>(_errors); }
        }

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
            if (query == null) {
                throw new ArgumentNullException("query");
            }

            return !(from prop in TypeDescriptor.GetProperties(query).Cast<PropertyDescriptor>()
                    from attribute in prop.Attributes.OfType<ValidationAttribute>()
                    where !attribute.IsValid(prop.GetValue(query))
                    select attribute).Any();
        }

        protected void AddError(QueryBinderException exception)
        {
            _errors.Add(exception);
        }
    }
}
