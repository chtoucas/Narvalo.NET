using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Playground.Mvc.Startup))]
namespace Playground.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
