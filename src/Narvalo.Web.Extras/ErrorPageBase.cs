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
        /// Initialise un nouvel objet de type <see cref="Narvalo.Web.ErrorPageBase"/>.
        /// </summary>
        protected ErrorPageBase()
        {
            Load += Page_Load;
        }

        /// <summary>
        /// Retourne le statut de la réponse HTTP.
        /// </summary>
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
