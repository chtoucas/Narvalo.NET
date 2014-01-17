namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.Web;

    public static partial class HttpResponseExtensions
    {
        // "X-AspNet-Version" peut être aussi supprimé dans le Web.config : 
        // <system.web>
        //  <httpRuntime enableVersionHeader="false" />
        // </system.web>
        // "X-AspNetMvc-Version" peut être supprimé via 
        //  MvcHandler.DisableMvcResponseHeader = true;
        // "X-Powered-By" peut être aussi supprimé dans le Web.config
        // <system.webServer>
        //  <httpProtocol>
        //    <customHeaders>
        //      <clear />
        //      <remove name="X-Powered-By" />
        //    </customHeaders>
        //   </httpProtocol>
        // </system.webServer>
        static readonly List<string> HeadersToRemove_ = new List<string> {
            "Server",
        };

        public static void CleanupHeaders(this HttpResponse response)
        {
            // Cf. aussi http://www.iis.net/learn/extensions/working-with-urlscan/urlscan-3-reference

            HeadersToRemove_.ForEach(_ => response.Headers.Remove(_));

            // Habituellement on essaie de prévenir l'inclusion d'une page du site dans une "frame" via javascript.
            // Malheureusement cela ne suffit pas et il existe des solutions pour contrer les techniques basées sur javascript.
            // Cf. http://en.wikipedia.org/wiki/Framekiller
            response.Headers.Add("X-Frame-Options", "DENY");
        }
    }
}
