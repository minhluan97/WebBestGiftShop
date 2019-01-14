using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShopQuaTang.Startup))]
namespace ShopQuaTang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
