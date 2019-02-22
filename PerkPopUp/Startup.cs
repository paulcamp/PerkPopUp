using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(PerkPopUp.Startup))]
namespace PerkPopUp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}