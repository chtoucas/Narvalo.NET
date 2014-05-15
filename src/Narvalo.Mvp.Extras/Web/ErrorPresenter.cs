// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Net;
    using System.Web;

    public class ErrorViewModel
    {
        public string ErrorMessage { get; set; }
    }

    public abstract class ErrorPresenter : HttpPresenterOf<ErrorViewModel>
    {
        public ErrorPresenter(IView<ErrorViewModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        protected abstract HttpStatusCode StatusCode { get; }

        void Load(object sender, EventArgs e)
        {
            var ex = Server.GetLastError() as HttpException;
            if (ex == null) {
                // En théorie, cela ne devrait jamais se produire.
                // Cependant, cela dépend du mode d'exécution de cette page et il semble bien
                // que ASP.NET ne garde pas l'erreur d'origine.
                return;
            }

            var statusCode = ex.GetHttpCode();

            if (statusCode >= 500) {
                View.Model.ErrorMessage = "XX";
            }
            else if (statusCode >= 400) {
                View.Model.ErrorMessage = "XX";
            }

            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)StatusCode;
        }
    }
}
