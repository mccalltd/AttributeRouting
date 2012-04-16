﻿using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Tests.Web.Areas.Api.Controllers;
using AttributeRouting.Tests.Web.Controllers;
using AttributeRouting.Web;
using AttributeRouting.Web.Http.WebHost;
using AttributeRouting.Web.Mvc;
using ControllerBase = AttributeRouting.Tests.Web.Controllers.ControllerBase;

namespace AttributeRouting.Tests.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var translationProvider = new FluentTranslationProvider();
            var routePrefixDict = new Dictionary<string, string>
                                      {
                                          {"es", "es-RoutePrefixUrl"},
                                          {"fr", "fr-RoutePrefixUrl"},
                                      };
            var indexRouteUrlDict = new Dictionary<string, string>
                                        {
                                            {"es", "es-RouteUrl"},
                                            {"fr", "fr-RouteUrl"},
                                        };

            // MVC localized controller
            translationProvider.AddTranslations()
                .ForController<Controllers.LocalizationController>()
                    .AreaUrl(new Dictionary<string, string>
                    {
                        {"es", "{culture}/es-AreaUrl"},
                        {"fr", "{culture}/fr-AreaUrl"},
                    })
                    .RoutePrefixUrl(routePrefixDict)
                    .RouteUrl(c => c.Index(), indexRouteUrlDict);

            // Web API localized controller
            translationProvider.AddTranslations()
                .ForController<Areas.Api.Controllers.LocalizationController>()
                    .AreaUrl(new Dictionary<string, string>
                    {
                        {"es", "api/{culture}/es-AreaUrl"},
                        {"fr", "api/{culture}/fr-AreaUrl"},
                    })
                    .RoutePrefixUrl(routePrefixDict)
                    .RouteUrl(c => c.GetLocalized(), indexRouteUrlDict);

            // Web API (WebHost)
            routes.MapHttpAttributeRoutes(config =>
            {
                config.ScanAssemblyOf<PlainController>();
                config.AddDefaultRouteConstraint(@"[Ii]d$", new RegexRouteConstraint(@"^\d+$"));
                config.UseRouteHandler(() => new HttpCultureAwareRoutingHandler());
                config.AddTranslationProvider(translationProvider);                
                config.UseLowercaseRoutes = true;
                config.InheritActionsFromBaseController = true;
            });

            routes.MapAttributeRoutes(config =>
            {
                config.ScanAssemblyOf<ControllerBase>();
                config.AddDefaultRouteConstraint(@"[Ii]d$", new RegexRouteConstraint(@"^\d+$"));
                config.AddTranslationProvider(translationProvider);
                config.UseRouteHandler(() => new CultureAwareRouteHandler());
                config.UseLowercaseRoutes = true;
                config.InheritActionsFromBaseController = true;
            });

            routes.MapRoute("CatchAll",
                            "{*path}",
                            new { controller = "home", action = "filenotfound" },
                            new[] { typeof(HomeController).Namespace });
        }
    }
}