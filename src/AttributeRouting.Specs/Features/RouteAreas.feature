Feature: Route Areas

Scenario: Generating area routes
	Given I have registered the routes for the AreasController
	When I fetch the routes for the Areas controller's Index action
	Then the route url is "Area/Index"
	And the data token for "area" is "Area"

Scenario: Generating area routes when route urls specify a duplicate area prefix
	Given I have registered the routes for the AreasController
	When I fetch the routes for the Areas controller's DuplicatePrefix action
	Then the route url is "Area/DuplicatePrefix"

Scenario: Generating absolute routes when a route area is defined
	Given I have registered the routes for the AreasController
	When I fetch the routes for the Areas controller's Absolute action
	Then the route url is "AreaAbsolute"

Scenario: Generating area routes when route url starts with the area prefix
	Given I have registered the routes for the AreasController
	When I fetch the routes for the Areas controller's RouteBeginsWithAreaName action
	Then the route url is "Area/Areas"

Scenario: Generating area routes with an explicit area url
	Given I have registered the routes for the ExplicitAreaUrlController
	When I fetch the routes for the ExplicitAreaUrl controller's Index action
	Then the route url is "ExplicitArea/Index"
	And the data token for "area" is "Area"
	 	 
Scenario: Generating area routes with an explicit area url when route urls specify a duplicate area prefix
	Given I have registered the routes for the ExplicitAreaUrlController
	When I fetch the routes for the ExplicitAreaUrl controller's DuplicatePrefix action
	Then the route url is "ExplicitArea/DuplicatePrefix"
	And the data token for "area" is "Area"

Scenario: Generating area routes with the default ctor of the RouteAreaAttribute
	Given I have registered the routes for the DefaultRouteAreaController
	When I fetch the routes for the DefaultRouteArea controller's Index action
	Then the route url is "Subjects/Index"
	And the data token for "area" is "Subjects"

