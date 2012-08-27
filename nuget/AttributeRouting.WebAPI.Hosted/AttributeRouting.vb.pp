Imports System.Reflection
Imports System.Web.Http.SelfHost
Imports AttributeRouting.Web.Http.SelfHost

Namespace $rootnamespace$
    Public Class AttributeRouting
	
		' Call this static method from a start up class in your applicaton (e.g.Program.cs)
		' Pass in the configuration you're using for your self-hosted Web API
		Public Shared Sub RegisterRoutes(config As HttpSelfHostConfiguration)
            
			' See http://github.com/mccalltd/AttributeRouting/wiki for more options.
			' To debug routes locally, you can log to Console.Out (or any other TextWriter) like so:
			'     config.Routes.Cast<HttpRoute>().LogTo(Console.Out);

			' Self-hosted Web API

            ' Attribute Routing
			Dim arConfig As New HttpAttributeRoutingConfiguration
            arConfig.ScanAssembly(Assembly.GetExecutingAssembly())
            ' Must have this on, otherwise you need to specify RouteName in your attributes
            arConfig.AutoGenerateRouteNames = true
			
			config.Routes.MapHttpAttributeRoutes(arConfig)
		End Sub
    End Class
End Namespace
