using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebFormsApp.Startup))]
namespace WebFormsApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
        }
    }
}
