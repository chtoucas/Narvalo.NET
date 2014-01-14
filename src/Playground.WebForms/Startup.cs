using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Playground.WebForms.Startup))]
namespace Playground.WebForms
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
