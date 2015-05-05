// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Web
{
    using System;
    ////using System.Security;
    using System.Web;

    /// <summary>
    /// <para>
    /// Au cours de son cycle de vie, l'application déclenche des événements que vous pouvez
    /// gérer et appelle des méthodes spécifiques que vous pouvez substituer. Pour gérer des
    /// événements ou des méthodes d'application, vous pouvez créer un fichier nommé Global.asax
    /// dans le répertoire racine de votre application.
    /// </para><para>
    /// Si vous créez un fichier Global.asax, ASP.NET le compile en une classe dérivée de la
    /// classe HttpApplication, puis utilise la classe dérivée pour représenter
    /// l'application.
    /// </para><para>
    /// Une instance de HttpApplication ne traite qu'une demande à la fois. Cela simplifie la
    /// gestion d'événements d'application, puisque vous n'avez pas besoin de verrouiller les
    /// membres non statiques de la classe d'application lorsque vous y accédez. Cela vous permet
    /// également de stocker des données spécifiques à la demande dans les membres non statiques
    /// de la classe d'application. Par exemple, vous pouvez définir une propriété dans le fichier
    /// Global.asax et lui assigner une valeur spécifique à la demande.
    /// </para><para>
    /// ASP.NET lie automatiquement des événements d'application à des gestionnaires dans
    /// le fichier Global.asax à l'aide de la convention d'affectation de noms Application_événement,
    /// par exemple Application_BeginRequest. Cela est comparable à la façon dont les méthodes
    /// de page ASP.NET sont automatiquement liées à des événements, comme l'événement Page_Load
    /// de la page.Pour plus d'informations, consultez Vue d'ensemble du cycle de vie des pages ASP.NET.
    /// </para><para>
    /// Les méthodes Application_Start et Application_End sont des méthodes spéciales qui ne
    /// représentent pas d'événements HttpApplication. ASP.NET les appelle une fois dans toute
    /// la durée de vie du domaine d'application, et non pour chaque instance de HttpApplication.
    /// </para><para>
    /// Une méthode Application_événement est déclenché au moment approprié du cycle de vie
    /// de l'application, comme le montre la table des cycles de vie de l'application,
    /// vue précédemment dans cette rubrique.
    /// </para><para>
    /// Application_Error peut être déclenché à tout moment du cycle de vie de l'application.
    /// </para><para>
    /// Application_EndRequest est le seul événement qui sera déclenché à coup sûr à chaque demande,
    /// parce qu'une demande peut être court-circuitée. Par exemple, si deux modules gèrent
    /// l'événement Application_BeginRequest et si le premier lève une exception, l'événement
    /// Application_BeginRequest ne sera pas appelé pour le deuxième module. Toutefois,
    /// la méthode Application_EndRequest est toujours appelée pour autoriser l'application à nettoyer
    /// les ressources.
    /// </para><para>
    /// Pour les détails concernant HttpApplication, on se reportera à
    /// http://msdn.microsoft.com/fr-fr/library/system.web.httpapplication%28v=vs.100%29.aspx
    /// Pour comprendre le cycle de vie de HttpApplication,
    /// http://msdn.microsoft.com/en-us/library/ms178473%28v=vs.100%29.aspx
    /// </para>
    /// </summary>
    public class WebApplication : HttpApplication
    {
        /// <summary>
        /// Exécute un code d'initialisation personnalisé une fois que tous les modules ont été créés.
        /// Demandé une fois pour chaque instance de la classe HttpApplication.
        /// </summary>
        public override void Init()
        {
            base.Init();
        }

        #region Méthodes spéciales appelées une seule fois dans toute la durée de vie du domaine d'application.

        /// <summary>
        /// <para>
        /// Appelé lorsque la première ressource (par exemple une page) d'une application ASP.NET
        /// est demandée. La méthode Application_Start n'est appelée qu'une fois au cours du cycle
        /// de vie d'une application.Vous pouvez utiliser cette méthode pour effectuer des tâches
        /// de démarrage telles que le chargement de données dans le cache ou l'initialisation de
        /// valeurs statiques.
        /// </para><para>
        /// Les données statiques ne doivent être définies qu'au démarrage de l'application.
        /// Ne définissez pas de données d'instance parce qu'elles ne seront disponibles que pour
        /// la première instance de la classe HttpApplication créée.
        /// </para><para>
        /// NB: Pour exécuter un code avant Application_Start, utiliser les facilités offertes par
        /// l'attribut PreApplicationStartMethod. Voir aussi WebActivator.
        /// </para>
        /// </summary>
        protected void Application_Start() { }

        /// <summary>
        /// Appelé une fois dans la durée de vie de l'application avant que celle-ci ne soit déchargée.
        /// </summary>
        protected void Application_End(object sender, EventArgs e) { }

        #endregion
    }
}
