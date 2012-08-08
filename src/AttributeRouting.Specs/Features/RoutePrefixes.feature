Feature: Route Prefixes

Background: 
	Given I generate the routes defined in the subject controllers

Scenario: Generating prefixed routes
	When I fetch the routes for the RoutePrefixes controller's Index action
	Then the route url is "Prefix/Index"
	When I fetch the routes for the HttpRoutePrefixes controller's Get action
	Then the route url is "ApiPrefix/Get"

Scenario: Generating prefixed routes when route urls specify a duplicate prefix
	When I fetch the routes for the RoutePrefixes controller's DuplicatePrefix action
	Then the route url is "Prefix/DuplicatePrefix"
	When I fetch the routes for the HttpRoutePrefixes controller's DuplicatePrefix action
	Then the route url is "ApiPrefix/DuplicatePrefix"

Scenario: Generating absolute routes when a route prefix is defined
	When I fetch the routes for the RoutePrefixes controller's Absolute action
	Then the route url is "PrefixAbsolute"
	When I fetch the routes for the HttpRoutePrefixes controller's Absolute action
	Then the route url is "ApiPrefixAbsolute"

Scenario: Generating prefixed routes when route url starts with the route prefix
	When I fetch the routes for the RoutePrefixes controller's RouteBeginsWithRoutePrefix action
	Then the route url is "Prefix/Prefixer"
	When I fetch the routes for the HttpRoutePrefixes controller's RouteBeginsWithRoutePrefix action
	Then the route url is "ApiPrefix/ApiPrefixer"

Scenario: Generating prefixed area routes
	When I fetch the routes for the AreaRoutePrefixes controller's Index action
	Then the route url is "Area/Prefix/Index"
	When I fetch the routes for the HttpAreaRoutePrefixes controller's Get action
	Then the route url is "ApiArea/ApiPrefix/Get"
	
Scenario: Generating prefixed area routes when route urls specify a duplicate prefix
	When I fetch the routes for the AreaRoutePrefixes controller's DuplicatePrefix action
	Then the route url is "Area/Prefix/DuplicatePrefix"
	When I fetch the routes for the HttpAreaRoutePrefixes controller's DuplicatePrefix action
	Then the route url is "ApiArea/ApiPrefix/DuplicatePrefix"

Scenario: Generating absolute routes when a route area and route prefix is defined
	When I fetch the routes for the AreaRoutePrefixes controller's Absolute action
	Then the route url is "AreaPrefixAbsolute"
	When I fetch the routes for the HttpAreaRoutePrefixes controller's Absolute action
	Then the route url is "ApiAreaPrefixAbsolute"
	
Scenario: Generating routes when a route area and route prefix are defined and the action respecifies the area url
	When I fetch the routes for the AreaRoutePrefixes controller's RelativeUrlIsAreaUrl action
	Then the route url is "Area/Prefix/Area"
	When I fetch the routes for the HttpAreaRoutePrefixes controller's RelativeUrlIsAreaUrl action
	Then the route url is "ApiArea/ApiPrefix/ApiArea"