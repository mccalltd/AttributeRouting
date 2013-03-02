Imports System.Web.Routing
Imports AttributeRouting.Web.Mvc

<assembly: WebActivator.PreApplicationStartMethod(GetType($rootnamespace$.AttributeRoutingConfig), "Start")>

Namespace $rootnamespace$
    Public Class AttributeRoutingConfig
		Public Shared Sub RegisterRoutes(routes As RouteCollection)
            
			' See http://github.com/mccalltd/AttributeRouting/wiki for more options.
			' To debug routes locally using the built in ASP.NET development server, go to /routes.axd
            
			routes.MapAttributeRoutes()
		End Sub

        Public Shared Sub Start()
            RegisterRoutes(RouteTable.Routes)
        End Sub
    End Class
End Namespace
