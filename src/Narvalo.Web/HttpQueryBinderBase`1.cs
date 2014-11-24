// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public abstract class HttpQueryBinderBase<TQuery> : IHttpQueryBinder<TQuery>
    {
        readonly IList<HttpQueryBinderException> _errors = new List<HttpQueryBinderException>();

        protected HttpQueryBinderBase() { }

        public IEnumerable<HttpQueryBinderException> BindingErrors { get { return _errors; } }

        public Maybe<TQuery> Bind(HttpRequest request)
        {
            Require.NotNull(request, "request");

            return from _ in BindCore(request) where Validate(_) select _;
        }

        protected abstract Maybe<TQuery> BindCore(HttpRequest request);

        protected virtual bool Validate(TQuery query)
        {
            //Require.NotNull(query, "query");

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
