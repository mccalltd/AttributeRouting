﻿Feature: Route Defaults

Background: 
	Given I generate the routes defined in the subject controllers

Scenario: Route default specified with an attribute
	When I fetch the routes for the RouteDefaults controller's Index action
	 And I fetch the routes for the HttpRouteDefaults controller's Get action
	Then the default for "p1" is "variable"

Scenario: Route default specified inline
	When I fetch the routes for the RouteDefaults controller's InlineDefaults action
	 And I fetch the routes for the HttpRouteDefaults controller's InlineDefaults action
	Then the route url is "InlineDefaults/{hello}/{goodnight}"
	Then the default for "hello" is "sun"
	Then the default for "goodnight" is "moon"

Scenario: Optional parameters specified with a url parameter token
	When I fetch the routes for the RouteDefaults controller's Optionals action
	 And I fetch the routes for the HttpRouteDefaults controller's Optionals action
	Then the route url is "Optionals/{p1}/{p2}/{p3}"
	 And the parameter "p1" is optional
	 And the parameter "p2" is optional
	 And the parameter "p3" is optional

Scenario: Multiple routes with different defaults
	When I fetch the routes for the RouteDefaults controller's MultipleRoutes action
	 And I fetch the routes for the HttpRouteDefaults controller's MultipleRoutes action
	Then the route named "MultipleDefaults1" has a default for "p1" of "first"
	 And the route named "MultipleDefaults2" has a default for "p1" of "second"
	 And the route named "ApiMultipleDefaults1" has a default for "p1" of "first"
	 And the route named "ApiMultipleDefaults2" has a default for "p1" of "second"