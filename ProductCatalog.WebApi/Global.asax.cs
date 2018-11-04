using System.Web.Http;
using System.Web.Mvc;

using ProductCatalog.Data.Context;

namespace ProductCatalog.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());

            AreaRegistration.RegisterAllAreas();
        }
    }
}
