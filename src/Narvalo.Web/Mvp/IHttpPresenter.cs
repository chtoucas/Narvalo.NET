// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Mvp
{
    using System.Web;
    using Narvalo.Mvp;

    public interface IHttpPresenter : IPresenter
    {
        HttpContextBase HttpContext { get; }

        IAsyncTaskManager AsyncManager { get; }
    }
}
