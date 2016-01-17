using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TentsNTrails.Startup))]
namespace TentsNTrails
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
