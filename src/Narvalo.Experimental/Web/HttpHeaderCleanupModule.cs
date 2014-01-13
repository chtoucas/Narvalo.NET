namespace Narvalo.Web
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    // Cf. aussi http://www.iis.net/learn/extensions/working-with-urlscan/urlscan-3-reference
    public class HttpHeaderCleanupModule : IHttpModule
    {
        readonly static List<string> HeadersToRemove_ = new List<string> {
			"Server",
            // X-AspNet-Version peut être aussi supprimé dans le Web.config : 
            // <system.web>
            //  <httpRuntime enableVersionHeader="false" />
            // </system.web>
			//"X-AspNet-Version",			
            // X-AspNetMvc-Version peut être supprimé via 
            //  MvcHandler.DisableMvcResponseHeader = true;
			//"X-AspNetMvc-Version",          
            // Peut être aussi supprimé dans le Web.config
            // <system.webServer>
            //  <httpProtocol>
            //    <customHeaders>
            //      <clear />
            //      <remove name="X-Powered-By" />
            //    </customHeaders>
            //   </httpProtocol>
            // </system.webServer>
			//"X-Powered-By",				
		};

        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(HttpHeaderCleanupModule));
        }

        void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            var response = app.Response;
            if (response == null) {
                // Peut arriver si trySkipIisCustomErrors est égal à true.
                return;
            }

            HeadersToRemove_.ForEach(_ => response.Headers.Remove(_));
        }
    }
}
