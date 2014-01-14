using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Playground.WebForms.Account
{
    public partial class OpenAuthProviders : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var provider = Request.Form["provider"];
                if (provider == null)
                {
                    return;
                }
                // Demande de reconnexion au fournisseur de connexion externe
                string redirectUrl = ResolveUrl(String.Format(CultureInfo.InvariantCulture, "~/Account/RegisterExternalLogin?{0}={1}&returnUrl={2}", IdentityHelper.ProviderNameKey, provider, ReturnUrl));
                var properties = new AuthenticationProperties() { RedirectUri = redirectUrl };
                // Ajouter une vérification xsrf lors de la liaison de comptes
                if (Context.User.Identity.IsAuthenticated)
                {
                    properties.Dictionary[IdentityHelper.XsrfKey] = Context.User.Identity.GetUserId();
                }
                Context.GetOwinContext().Authentication.Challenge(properties, provider);
                Response.StatusCode = 401;
                Response.End();
            }
        }

        public string ReturnUrl { get; set; }

        public IEnumerable<string> GetProviderNames()
        {
            return Context.GetOwinContext().Authentication.GetExternalAuthenticationTypes().Select(t => t.AuthenticationType);
        }
    }
}