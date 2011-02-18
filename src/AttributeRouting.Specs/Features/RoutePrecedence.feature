Feature: Route Precedence

Background: 
	Given I generate the routes defined in the subject controllers

Scenario: Route precedence among routes for an action using the Order property
	When I fetch the routes for the RoutePrecedenceAmongRoutes controller's Index action
	Then 3 routes are found
	 And the 1st route's url is "Index/First"
	 And the 2nd route's url is "Index/Second"
	 And the 3rd route's url is "Index/Third"

Scenario: Route precedence among actions within a controller using the Precedence property
	When I fetch the routes for the RoutePrecedenceAmongActions controller
	Then the 1st route's url is "Route1"
	 And the 2nd route's url is "Route2"
	 And the 3rd route's url is "Route3"

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