// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Net;
    using System.Web.UI;

    /// <summary>
    /// Fournit une classe abstraite représentant une page d'erreur.
    /// </summary>
    public abstract class ErrorPageBase : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorPageBase"/> class.
        /// </summary>
        protected ErrorPageBase()
        {
            Load += Page_Load;
        }

        /// <summary>
        /// Gets the status of the HTTP response.
        /// </summary>
        /// <value>The status of the HTTP response.</value>
        protected abstract HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Méthode exécutée lorsque la page est chargée dans l'objet <see cref="System.Web.UI.Page"/>.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.TrySkipIisCustomErrors = true;
            Response.SetStatusCode(StatusCode);

            Page_LoadCore(sender, e);
        }

        protected virtual void Page_LoadCore(object sender, EventArgs e) { }
    }
}
