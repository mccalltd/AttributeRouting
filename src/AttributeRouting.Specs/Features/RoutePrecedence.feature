Feature: Route Precedence

Scenario: Route precedence among routes for an action using the Order property
	# MVC
	Given I have registered the routes for the RoutePrecedenceAmongRoutesController
	When I fetch the routes for the RoutePrecedenceAmongRoutes controller's Index action
	Then the 1st route's url is "Index/First"
	And the 2nd route's url is "Index/Second"
	And the 3rd route's url is "Index/Third"
	And the 4th route's url is "Index/Fourth"
	And the 5th route's url is "Index/Fifth"
	And the 6th route's url is "Index/Sixth"
	And the 7th route's url is "Index/Seventh"
	# Web API
	Given I have registered the routes for the HttpRoutePrecedenceAmongRoutesController
	When I fetch the routes for the HttpRoutePrecedenceAmongRoutes controller's Get action
	Then the 1st route's url is "Get/First"
	And the 2nd route's url is "Get/Second"
	And the 3rd route's url is "Get/Third"
	And the 4th route's url is "Get/Fourth"
	And the 5th route's url is "Get/Fifth"
	And the 6th route's url is "Get/Sixth"
	And the 7th route's url is "Get/Seventh"

Scenario: Route precedence among actions within a controller using the Precedence property
	# MVC
	Given I have registered the routes for the RoutePrecedenceAmongActionsController
	When I fetch the routes for the RoutePrecedenceAmongActions controller
	Then the 1st route's url is "Route1"
	And the 2nd route's url is "Route2"
	And the 3rd route's url is "Route3"
	And the 4th route's url is "Route4"
	And the 5th route's url is "Route5"
	And the 6th route's url is "Route6"
	And the 7th route's url is "Route7"
	# Web API
	Given I have registered the routes for the HttpRoutePrecedenceAmongActionsController
	When I fetch the routes for the HttpRoutePrecedenceAmongActions controller
	Then the 1st route's url is "ApiRoute1"
	And the 2nd route's url is "ApiRoute2"
	And the 3rd route's url is "ApiRoute3"
	And the 4th route's url is "ApiRoute4"
	And the 5th route's url is "ApiRoute5"
	And the 6th route's url is "ApiRoute6"
	And the 7th route's url is "ApiRoute7"

Scenario: Route precedence among controllers added individually using the configuration api
	Given I have a new configuration object
	And I add the routes from the RoutePrecedenceAmongControllers1 controller
	And I add the routes from the RoutePrecedenceAmongControllers2 controller
	And I add the routes from the RoutePrecedenceAmongControllers3 controller
	When I generate the routes with this configuration
	Then the routes from the RoutePrecedenceAmongControllers1 controller precede those from the RoutePrecedenceAmongControllers2 controller
	And the routes from the RoutePrecedenceAmongControllers2 controller precede those from the RoutePrecedenceAmongControllers3 controller

Scenario: Route precedence among controllers added by base type using the configuration api
	Given I have a new configuration object
	And I add the routes from controllers derived from the RoutePrecedenceAmongDerivedControllersBase controller
	And I add the routes from the RoutePrecedenceAmongControllers1 controller
	When I generate the routes with this configuration
	Then the routes from the RoutePrecedenceAmongDerivedControllers1 controller precede those from the RoutePrecedenceAmongControllers1 controller
	And the routes from the RoutePrecedenceAmongDerivedControllers2 controller precede those from the RoutePrecedenceAmongControllers1 controller

Scenario: Route precedence set for the site using the SitePrecedence property
	# Ensure that the controller with the top route is NOT scanned first
	Given I have a new configuration object
	And I add the routes from the RoutePrecedenceAmongControllers1 controller
	And I add the routes from the RoutePrecedenceAmongTheSitesRoutes controller
	When I generate the routes with this configuration
	And I fetch all the routes
	Then the 1st route's url is "I-Am-The-First-Route"

# Web API

Scenario: Web API route precedence among controllers added individually using the configuration api
	Given I have a new configuration object
	And I add the routes from the HttpRoutePrecedenceAmongControllers1 controller
	And I add the routes from the HttpRoutePrecedenceAmongControllers2 controller
	And I add the routes from the HttpRoutePrecedenceAmongControllers3 controller
	When I generate the routes with this configuration
	Then the routes from the HttpRoutePrecedenceAmongControllers1 controller precede those from the HttpRoutePrecedenceAmongControllers2 controller
	And the routes from the HttpRoutePrecedenceAmongControllers2 controller precede those from the HttpRoutePrecedenceAmongControllers3 controller

Scenario: Web API route precedence among controllers added by base type using the configuration api
	Given I have a new configuration object
	And I add the routes from controllers derived from the HttpRoutePrecedenceAmongDerivedControllersBase controller
	And I add the routes from the HttpRoutePrecedenceAmongControllers1 controller
	When I generate the routes with this configuration
	Then the routes from the HttpRoutePrecedenceAmongDerivedControllers1 controller precede those from the HttpRoutePrecedenceAmongControllers1 controller
	And the routes from the HttpRoutePrecedenceAmongDerivedControllers2 controller precede those from the HttpRoutePrecedenceAmongControllers1 controller