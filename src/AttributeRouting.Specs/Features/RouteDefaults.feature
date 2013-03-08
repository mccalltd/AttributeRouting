Feature: Route Defaults

Background: 
	Given I have a new configuration object
	And I add the routes from the RouteDefaults controller
	And I add the routes from the HttpRouteDefaults controller
	And I generate the routes with this configuration

Scenario: Route default specified inline
	When I fetch the routes for the RouteDefaults controller's InlineDefaults action
	Then the route url is "InlineDefaults/{hello}/{goodnight}"
	Then the default for "hello" is "sun"
	Then the default for "goodnight" is "moon"
	When I fetch the routes for the HttpRouteDefaults controller's InlineDefaults action
	Then the route url is "InlineDefaults/{hello}/{goodnight}"
	Then the default for "hello" is "sun"
	Then the default for "goodnight" is "moon"

Scenario: Optional parameters specified with a url parameter token
	When I fetch the routes for the RouteDefaults controller's Optionals action
	Then the route url is "Optionals/{p1}"
	And the parameter "p1" is optional
	When I fetch the routes for the HttpRouteDefaults controller's Optionals action
	Then the route url is "Optionals/{p1}"
	And the parameter "p1" is optional

Scenario: Using the controller and action url params
	When I fetch the routes for the RouteDefaults controller's TheActionName action
	Then the route url is "RouteDefaults/TheActionName"
	When I fetch the routes for the HttpRouteDefaults controller's TheActionName action
	Then the route url is "HttpRouteDefaults/TheActionName"

