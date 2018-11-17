using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Emiasto.Startup))]
namespace Emiasto
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
