Feature: Route Prefixes

Scenario: Generating prefixed routes
	# MVC
	Given I have registered the routes for the RoutePrefixesController
	When I fetch the routes for the RoutePrefixes controller's Index action
	Then the route url is "Prefix/Index"
	# Web API
	Given I have registered the routes for the HttpRoutePrefixesController
	When I fetch the routes for the HttpRoutePrefixes controller's Get action
	Then the route url is "ApiPrefix/Get"

Scenario: Generating prefixed routes when route urls specify a duplicate prefix
	# MVC
	Given I have registered the routes for the RoutePrefixesController
	When I fetch the routes for the RoutePrefixes controller's DuplicatePrefix action
	Then the route url is "Prefix/DuplicatePrefix"
	# Web API
	Given I have registered the routes for the HttpRoutePrefixesController
	When I fetch the routes for the HttpRoutePrefixes controller's DuplicatePrefix action
	Then the route url is "ApiPrefix/DuplicatePrefix"

Scenario: Generating absolute routes when a route prefix is defined
	# MVC
	Given I have registered the routes for the RoutePrefixesController
	When I fetch the routes for the RoutePrefixes controller's Absolute action
	Then the route url is "PrefixAbsolute"
	# Web API
	Given I have registered the routes for the HttpRoutePrefixesController
	When I fetch the routes for the HttpRoutePrefixes controller's Absolute action
	Then the route url is "ApiPrefixAbsolute"

Scenario: Generating prefixed routes when route url starts with the route prefix
	# MVC
	Given I have registered the routes for the RoutePrefixesController
	When I fetch the routes for the RoutePrefixes controller's RouteBeginsWithRoutePrefix action
	Then the route url is "Prefix/Prefixer"
	# Web API
	Given I have registered the routes for the HttpRoutePrefixesController
	When I fetch the routes for the HttpRoutePrefixes controller's RouteBeginsWithRoutePrefix action
	Then the route url is "ApiPrefix/ApiPrefixer"

Scenario: Generating prefixed area routes
	# MVC
	Given I have registered the routes for the AreaRoutePrefixesController
	When I fetch the routes for the AreaRoutePrefixes controller's Index action
	Then the route url is "Area/Prefix/Index"
	# Web API
	Given I have registered the routes for the HttpAreaRoutePrefixesController
	When I fetch the routes for the HttpAreaRoutePrefixes controller's Get action
	Then the route url is "ApiArea/ApiPrefix/Get"
	
Scenario: Generating prefixed area routes when route urls specify a duplicate prefix
	# MVC
	Given I have registered the routes for the AreaRoutePrefixesController
	When I fetch the routes for the AreaRoutePrefixes controller's DuplicatePrefix action
	Then the route url is "Area/Prefix/DuplicatePrefix"
	# Web API
	Given I have registered the routes for the HttpAreaRoutePrefixesController
	When I fetch the routes for the HttpAreaRoutePrefixes controller's DuplicatePrefix action
	Then the route url is "ApiArea/ApiPrefix/DuplicatePrefix"

Scenario: Generating absolute routes when a route area and route prefix is defined
	# MVC
	Given I have registered the routes for the AreaRoutePrefixesController
	When I fetch the routes for the AreaRoutePrefixes controller's Absolute action
	Then the route url is "AreaPrefixAbsolute"
	# Web API
	Given I have registered the routes for the HttpAreaRoutePrefixesController
	When I fetch the routes for the HttpAreaRoutePrefixes controller's Absolute action
	Then the route url is "ApiAreaPrefixAbsolute"
	
Scenario: Generating routes when a route area and route prefix are defined and the action respecifies the area url
	# MVC
	Given I have registered the routes for the AreaRoutePrefixesController
	When I fetch the routes for the AreaRoutePrefixes controller's RelativeUrlIsAreaUrl action
	Then the route url is "Area/Prefix/Area"
	# Web API
	Given I have registered the routes for the HttpAreaRoutePrefixesController
	When I fetch the routes for the HttpAreaRoutePrefixes controller's RelativeUrlIsAreaUrl action
	Then the route url is "ApiArea/ApiPrefix/ApiArea"