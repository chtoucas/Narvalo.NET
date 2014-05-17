// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;

    public class ErrorModel { }

    public interface IErrorView : IView<ErrorModel> { }

    public class NewPresenter : ErrorPresenter<IErrorView>
    {
        protected NewPresenter(IErrorView view)
            : base(view)
        {
        }
    }

    public class ErrorPresenter<TView> : HttpPresenter<TView>
        where TView : class, IView
    {
        protected ErrorPresenter(TView view)
            : base(view)
        {
            View.Load += Load;
        }

        protected virtual void Load(object sender, EventArgs e)
        {
            // TODO: Peut-on plutôt utiliser Context.Error au lieu de Server.GetLastError() ?
            var ex = Server.GetLastError() as HttpException;
            if (ex == null) {
                // En théorie, cela ne devrait jamais se produire.
                // Cependant, cela dépend du mode d'exécution de cette page et il semble bien
                // que ASP.NET ne garde pas l'erreur d'origine.
                return;
            }

            var statusCode = ex.GetHttpCode();

            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = statusCode;
        }
    }
}
