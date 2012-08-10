Feature: Route Areas

Scenario: Generating area routes
	# MVC
	Given I have registered the routes for the AreasController
	When I fetch the routes for the Areas controller's Index action
	Then the route url is "Area/Index"
	And the data token for "area" is "Area"
	# Web API
	Given I have registered the routes for the HttpAreasController
	When I fetch the routes for the HttpAreas controller's Get action
	Then the route url is "ApiArea/Get"	
	And the data token for "area" is "ApiArea"

Scenario: Generating area routes when route urls specify a duplicate area prefix
	# MVC
	Given I have registered the routes for the AreasController
	When I fetch the routes for the Areas controller's DuplicatePrefix action
	Then the route url is "Area/DuplicatePrefix"
	# Web API
	Given I have registered the routes for the HttpAreasController
	When I fetch the routes for the HttpAreas controller's DuplicatePrefix action
	Then the route url is "ApiArea/DuplicatePrefix"

Scenario: Generating absolute routes when a route area is defined
	# MVC
	Given I have registered the routes for the AreasController
	When I fetch the routes for the Areas controller's Absolute action
	Then the route url is "AreaAbsolute"
	# Web API
	Given I have registered the routes for the HttpAreasController
	When I fetch the routes for the HttpAreas controller's Absolute action
	Then the route url is "ApiAreaAbsolute"

Scenario: Generating area routes when route url starts with the area prefix
	# MVC
	Given I have registered the routes for the AreasController
	When I fetch the routes for the Areas controller's RouteBeginsWithAreaName action
	Then the route url is "Area/Areas"
	# Web API
	Given I have registered the routes for the HttpAreasController
	When I fetch the routes for the HttpAreas controller's RouteBeginsWithAreaName action
	Then the route url is "ApiArea/ApiAreas"

Scenario: Generating area routes with an explicit area url
	# MVC
	Given I have registered the routes for the ExplicitAreaUrlController
	When I fetch the routes for the ExplicitAreaUrl controller's Index action
	Then the route url is "ExplicitArea/Index"
	And the data token for "area" is "Area"
	# Web API
	Given I have registered the routes for the HttpExplicitAreaUrlController
	When I fetch the routes for the HttpExplicitAreaUrl controller's Get action
	Then the route url is "ApiExplicitArea/Get"
	And the data token for "area" is "ApiArea"
	 	 
Scenario: Generating area routes with an explicit area url when route urls specify a duplicate area prefix
	# MVC
	Given I have registered the routes for the ExplicitAreaUrlController
	When I fetch the routes for the ExplicitAreaUrl controller's DuplicatePrefix action
	Then the route url is "ExplicitArea/DuplicatePrefix"
	And the data token for "area" is "Area"
	# Web API
	Given I have registered the routes for the HttpExplicitAreaUrlController
	When I fetch the routes for the HttpExplicitAreaUrl controller's DuplicatePrefix action
	Then the route url is "ApiExplicitArea/DuplicatePrefix"
	And the data token for "area" is "ApiArea"
