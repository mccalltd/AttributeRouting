Feature: Prefixing Routes

Background: 
	Given I generate the routes defined in the subject controllers

Scenario: Generating prefixed routes
	When I fetch the routes for the RoutePrefixes controller's Index action
	Then the route url is "Prefix/Index"

Scenario: Generating prefixed routes when route urls specify a duplicate prefix
	When I fetch the routes for the RoutePrefixes controller's DuplicatePrefix action
	Then the route url is "Prefix/DuplicatePrefix"

Scenario: Generating absolute routes when a route prefix is defined
	When I fetch the routes for the RoutePrefixes controller's Absolute action
	Then the route url is "PrefixAbsolute"

Scenario: Generating prefixed area routes
	When I fetch the routes for the AreaRoutePrefixes controller's Index action
	Then the route url is "Area/Prefix/Index"
	
Scenario: Generating prefixed area routes when route urls specify a duplicate prefix
	When I fetch the routes for the AreaRoutePrefixes controller's DuplicatePrefix action
	Then the route url is "Area/Prefix/DuplicatePrefix"

Scenario: Generating absolute routes when a route area and route prefix is defined
	When I fetch the routes for the AreaRoutePrefixes controller's Absolute action
	Then the route url is "AreaPrefixAbsolute"