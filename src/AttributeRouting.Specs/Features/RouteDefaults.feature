Feature: Route Defaults

Background: 
	Given I generate the routes defined in the subject controllers

Scenario: Route default specified with an attribute
	When I fetch the routes for the RouteDefaults controller's Index action
	Then the default for "p1" is "variable"

Scenario: Optional parameters specified with a url parameter token
	When I fetch the routes for the RouteDefaults controller's Optionals action
	Then the route url is "Optionals/{p1}/{p2}"
	 And the parameter "p1" is optional
	 And the parameter "p2" is optional

Scenario: Multiple routes with different defaults
	When I fetch the routes for the RouteDefaults controller's MultipleRoutes action
	Then the route named "MultipleDefaults1" has a default for "p1" of "first"
	 And the route named "MultipleDefaults2" has a default for "p1" of "second"
