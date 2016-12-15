using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnnouncementsWebApp1.Startup))]
namespace AnnouncementsWebApp1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
