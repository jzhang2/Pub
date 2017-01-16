using System.Web.Mvc;
using System.Web.Routing;

namespace LeaRun.Application.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Books",
                url: "Books",
                defaults: new { controller = "Default", action = "Books", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MapCulture",
                url: "MapCulture",
                defaults: new { controller = "Default", action = "MapCulture", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "News",
                url: "News",
                defaults: new { controller = "Default", action = "News", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SignUp",
                url: "SignUp",
                defaults: new { controller = "Default", action = "SignUp", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SignIn",
                url: "SignIn",
                defaults: new { controller = "Default", action = "SignIn", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}