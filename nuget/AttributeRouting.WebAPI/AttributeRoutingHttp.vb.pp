Imports System.Web.Routing
Imports AttributeRouting.Web.Http.WebHost

<assembly: WebActivator.PreApplicationStartMethod(GetType($rootnamespace$.App_Start.AttributeRoutingHttp), "Start")>

Namespace $rootnamespace$.App_Start
    Public Class AttributeRoutingHttp
		Public Shared Sub RegisterRoutes(routes As RouteCollection)
            
			' See http://github.com/mccalltd/AttributeRouting/wiki for more options.
			' To debug routes locally using the built in ASP.NET development server, go to /routes.axd
            
			' ASP.NET Web API
            routes.MapHttpAttributeRoutes()
		End Sub

        Public Shared Sub Start()
            RegisterRoutes(RouteTable.Routes)
        End Sub
    End Class
End Namespace
