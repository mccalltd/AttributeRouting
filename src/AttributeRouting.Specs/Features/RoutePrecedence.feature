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

Scenario: Route precedence among controllers using the configuration api