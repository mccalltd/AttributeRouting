Feature: Route Areas

Background: 
	Given I generate the routes defined in the subject controllers

Scenario: Generating area routes
	When I fetch the routes for the Areas controller's Index action
	Then the route url is "Area/Index"
	 And the data token for "area" is "Area"

Scenario: Generating area routes when route urls specify a duplicate area prefix
	When I fetch the routes for the Areas controller's DuplicatePrefix action
	Then the route url is "Area/DuplicatePrefix"

Scenario: Generating absolute routes when a route area is defined
	When I fetch the routes for the Areas controller's Absolute action
	Then the route url is "AreaAbsolute"

Scenario: Generating area routes with an explicit area url
	When I fetch the routes for the ExplicitAreaUrl controller's Index action
	Then the route url is "ExplicitArea/Index"
	 And the data token for "area" is "Area"
	 
Scenario: Generating area routes with an explicit area url when route urls specify a duplicate area prefix
	When I fetch the routes for the ExplicitAreaUrl controller's DuplicatePrefix action
	Then the route url is "ExplicitArea/DuplicatePrefix"
	 And the data token for "area" is "Area"