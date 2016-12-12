﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;

    using Narvalo.Fx;

    public abstract partial class HttpQueryBinder<TQuery> : IHttpQueryBinder<TQuery>
    {
        private readonly List<HttpQueryBinderException> _errors = new List<HttpQueryBinderException>();

        protected HttpQueryBinder() { }

        public IEnumerable<HttpQueryBinderException> BindingErrors
        {
            get
            {
                Warrant.NotNull<IEnumerable<HttpQueryBinderException>>();

                return _errors;
            }
        }

        public Maybe<TQuery> Bind(HttpRequest request)
        {
            Require.NotNull(request, nameof(request));

            return from _ in BindCore(request) where Validate(_) select _;
        }

        protected abstract Maybe<TQuery> BindCore(HttpRequest request);

        [Pure]
        protected bool Validate(TQuery query)
        {
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

#if CONTRACTS_FULL

namespace Narvalo.Web
{
    using System.Diagnostics.Contracts;
    using System.Web;

    using Narvalo.Fx;

    [ContractClass(typeof(HttpQueryBinderContract<>))]
    public abstract partial class HttpQueryBinder<TQuery>
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_errors != null);
        }
    }

    [ContractClassFor(typeof(HttpQueryBinder<>))]
    internal abstract class HttpQueryBinderContract<TQuery> : HttpQueryBinder<TQuery>
    {
        protected override Maybe<TQuery> BindCore(HttpRequest request)
        {
            Contract.Requires(request != null);
            Contract.Ensures(Contract.Result<Maybe<TQuery>>() != null);

            return default(Maybe<TQuery>);
        }
    }
}

#endif
