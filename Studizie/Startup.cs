using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Studizie.Startup))]
namespace Studizie
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
