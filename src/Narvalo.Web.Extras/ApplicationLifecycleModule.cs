namespace Narvalo.Web
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.UI;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;
    using Serilog;

    public class ApplicationLifecycleModule : IHttpModule
    {
        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(ApplicationLifecycleModule));
        }

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

            context.Error += OnError_;
            context.Disposed += OnDisposed;
            context.PreSendRequestHeaders += OnPreSendRequestHeaders_;
        }

        public void Dispose() { }

        static HttpStatusCode GetStatusCode_(Exception ex)
        {
            Type type = ex.GetType();
            var httpException = ex as HttpException;

            // Lorsqu'un exception de type ViewStateException ou HttpRequestValidationException est levée,
            // ASP.NET retourne une erreur HTTP 500, on préfère utiliser une erreur HTTP 400.
            if (httpException == null) {
                return type == typeof(ViewStateException)
                     ? HttpStatusCode.BadRequest
                     : HttpStatusCode.InternalServerError;
            }
            else {
                return type == typeof(HttpRequestValidationException)
                    ? HttpStatusCode.BadRequest
                    : (HttpStatusCode)httpException.GetHttpCode();
            }
        }
        
        /// <summary>
        /// Se produit lorsque l'application est supprimée.
        /// </summary>
        void OnDisposed(object sender, EventArgs e)
        {
            Log.Information("Application disposed.");
        }

        /// <summary>
        /// Se produit lorsqu'une exception non gérée est levée.
        /// NB: Cet événement peut être déclenché à tout moment du cycle de vie de l'application.
        /// </summary>
        void OnError_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var server = app.Server;

            var ex = server.GetLastError();
            if (ex == null) {
                Log.Fatal(SR.ApplicationLifecycleModule_UnknownError);
                return;
            }

            var statusCode = GetStatusCode_(ex);

            switch (statusCode) {
                case HttpStatusCode.BadRequest:
                    Log.Warning(ex, ex.Message);
                    server.ClearError();
                    app.Response.SetStatusCode(statusCode);
                    break;
                case HttpStatusCode.NotFound:
                    Log.Debug(ex, ex.Message);
                    break;
                default:
                    Log.Fatal(ex, ex.Message);
                    break;
            }
        }

        void OnPreSendRequestHeaders_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            var response = app.Response;
            if (response == null) {
                // Peut arriver si trySkipIisCustomErrors est égal à "true".
                return;
            }

            response.CleanupHeaders();
        }
    }
}
