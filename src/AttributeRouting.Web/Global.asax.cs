﻿using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework.Localization;
using AttributeRouting.Web.Controllers;
using ControllerBase = AttributeRouting.Web.Controllers.ControllerBase;

namespace AttributeRouting.Web
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
            translationProvider.AddTranslations().ForController<LocalizationController>()
                .AreaUrl(new Dictionary<string, string>
                {
                    { "es", "{culture}/es-AreaUrl" },
                    { "fr", "{culture}/fr-AreaUrl" },
                })
                .RoutePrefixUrl(new Dictionary<string, string>
                {
                    { "es", "es-RoutePrefixUrl" },
                    { "fr", "fr-RoutePrefixUrl" },
                })
                .RouteUrl(c => c.Index(), new Dictionary<string, string>
                {
                    { "es", "es-RouteUrl" },
                    { "fr", "fr-RouteUrl" },
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