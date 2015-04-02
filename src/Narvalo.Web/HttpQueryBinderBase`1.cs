// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;

    using Narvalo.Fx;

    public abstract partial class HttpQueryBinderBase<TQuery> : IHttpQueryBinder<TQuery>
    {
        private readonly List<HttpQueryBinderException> _errors = new List<HttpQueryBinderException>();

        protected HttpQueryBinderBase() { }

        public IEnumerable<HttpQueryBinderException> BindingErrors
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<HttpQueryBinderException>>() != null);

                return _errors;
            }
        }

        public Maybe<TQuery> Bind(HttpRequest request)
        {
            Require.NotNull(request, "request");
            Contract.Ensures(Contract.Result<Maybe<TQuery>>() != null);

            return from _ in BindCore(request) where Validate(_) select _;
        }

        protected abstract Maybe<TQuery> BindCore(HttpRequest request);

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

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_errors != null);
        }

#endif
    }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

    [ContractClass(typeof(HttpQueryBinderBaseContract<>))]
    public abstract partial class HttpQueryBinderBase<TQuery> { }

    [ContractClassFor(typeof(HttpQueryBinderBase<>))]
    internal abstract class HttpQueryBinderBaseContract<TQuery> : HttpQueryBinderBase<TQuery>
    {
        protected override Maybe<TQuery> BindCore(HttpRequest request)
        {
            Contract.Requires(request != null);
            Contract.Ensures(Contract.Result<Maybe<TQuery>>() != null);

            return default(Maybe<TQuery>);
        }
    }

#endif
}
