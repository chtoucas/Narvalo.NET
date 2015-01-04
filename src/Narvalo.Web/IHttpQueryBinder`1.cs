// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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
