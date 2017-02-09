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
                name: "ResetPwd",
                url: "ResetPwd",
                defaults: new { controller = "Default", action = "ResetPwd", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ForgotPassWord",
                url: "ForgotPassWord",
                defaults: new { controller = "Default", action = "ForgotPassWord", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Books",
                url: "Books",
                defaults: new { controller = "Default", action = "Books", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Service",
                url: "Service/{id}",
                defaults: new { controller = "Default", action = "Service", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PBook",
                url: "PBook/{id}",
                defaults: new { controller = "Default", action = "PBook", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MyAccount",
                url: "MyAccount/{id}",
                defaults: new { controller = "Default", action = "MyAccount", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Detail",
                url: "Detail/{id}",
                defaults: new { controller = "Default", action = "Detail", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AboutUs",
                url: "AboutUs/{id}",
                defaults: new { controller = "Default", action = "AboutUs", id = UrlParameter.Optional }
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