// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using Narvalo.Mvp.PresenterBinding;

    public interface IHttpContextAdapter
    {
        HttpContextBase Create(HttpContext httpContext);
    }

    public class DefaultHttpContextAdapter : IHttpContextAdapter
    {
        public HttpContextBase Create(HttpContext httpContext)
        {
            return new HttpContextWrapper(httpContext);
        }
    }

    public sealed class AspNetPresenterBinder : PresenterBinder
    {
        readonly HttpContextBase _httpContext;

        public AspNetPresenterBinder(object host, HttpContext httpContext)
            : this(new[] { host }, httpContext) { }

        public AspNetPresenterBinder(IEnumerable<object> hosts, HttpContext httpContext)
            : base(hosts)
        {
            Require.NotNull(httpContext, "httpContext");

            _httpContext = new HttpContextWrapper(httpContext);
        }

        protected override IPresenter CreatePresenter(PresenterBindingParameter binding, IView view)
        {
            var presenter = base.CreatePresenter(binding, view);

            //presenter.HttpContext = _httpContext;

            return presenter;
        }
    }
}
