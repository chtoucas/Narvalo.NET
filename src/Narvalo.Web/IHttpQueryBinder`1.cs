// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.Web;

    using Narvalo.Fx;

    public partial interface IHttpQueryBinder<TQuery>
    {
        IEnumerable<HttpQueryBinderException> BindingErrors { get; }

        Maybe<TQuery> Bind(HttpRequest request);
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web;

    using Narvalo.Fx;

    [ContractClass(typeof(IHttpQueryBinderContract<>))]
    public partial interface IHttpQueryBinder<TQuery> { }

    [ContractClassFor(typeof(IHttpQueryBinder<>))]
    internal abstract class IHttpQueryBinderContract<TQuery> : IHttpQueryBinder<TQuery>
    {
        IEnumerable<HttpQueryBinderException> IHttpQueryBinder<TQuery>.BindingErrors
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<HttpQueryBinderException>>() != null);

                return default(IEnumerable<HttpQueryBinderException>);
            }
        }

        Maybe<TQuery> IHttpQueryBinder<TQuery>.Bind(HttpRequest request)
        {
            Contract.Ensures(Contract.Result<Maybe<TQuery>>() != null);

            return default(Maybe<TQuery>);
        }
    }
}

#endif
