using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Playground.WebForms
{
    public partial class Startup {

        // Pour plus d'informations sur le regroupement (Bundling), consultez http://go.microsoft.com/fwlink/?LinkId=301883
        public void ConfigureAuth(IAppBuilder app)
        {
            // Laisser l'application utiliser un cookie pour stocker les informations de l'utilisateur connecté
            // et stocke aussi les informations sur la connexion d'un utilisateur avec un fournisseur de connexion tiers.
            // Ceci est obligatoire si votre application autorise les utilisateurs à se connecter
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Décommenter les lignes suivantes pour activer la connexion avec des fournisseurs de connexion tiers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();
        }
    }
}
