Feature: Route Conventions

Background: 
	Given I generate the routes defined in the subject controllers

Scenario Outline: Generating routes using the RestfulRouteConvention
	When I fetch the routes for the RestfulRouteConvention controller's <action> action
	Then the route url is "<url>"
	 And the default for "controller" is "RestfulRouteConvention"
	 And the default for "action" is "<action>"
	 And the route for <action> is constrained to <method> requests

	Examples:
		| action	| method	| url										|
		| Index		| GET		| RestfulRouteConvention				|
		| New		| GET		| RestfulRouteConvention/New			|		
		| Create	| POST		| RestfulRouteConvention				|		
		| Show		| GET		| RestfulRouteConvention/{id}			|		
		| Edit		| GET		| RestfulRouteConvention/{id}/Edit		|		
		| Update	| PUT		| RestfulRouteConvention/{id}			|		
		| Delete	| GET		| RestfulRouteConvention/{id}/Delete	|		
		| Destroy	| DELETE	| RestfulRouteConvention/{id}			|		
		| Custom	| GET		| RestfulRouteConvention/Custom			|

Scenario Outline: Generating routes using the RestfulRouteConvention on controllers with a RoutePrefix attribute
	When I fetch the routes for the RestfulRouteConventionPrefix controller's <action> action
	Then the route url is "<url>"
	 And the default for "controller" is "RestfulRouteConventionPrefix"
	 And the default for "action" is "<action>"
	 And the route for <action> is constrained to <method> requests

	Examples:
		| action	| method	| url					|
		| Index		| GET		| Prefix				|
		| New		| GET		| Prefix/New			|		
		| Create	| POST		| Prefix				|		
		| Show		| GET		| Prefix/{id}			|		
		| Edit		| GET		| Prefix/{id}/Edit		|		
		| Update	| PUT		| Prefix/{id}			|		
		| Delete	| GET		| Prefix/{id}/Delete	|		
		| Destroy	| DELETE	| Prefix/{id}			|		

Scenario: Generating routes using the RestfulRouteConvention on actions with an explicit route defined
	When I fetch the routes for the RestfulRouteConventionWithExplicitRoute controller's Index action
	Then the 1st route url is "RestfulRouteConventionWithExplicitRoute"
	 And the 2nd route url is "Legacy"

Scenario: Generating routes using the RestfulRouteConvention on actions with an explicit ordered route defined
	When I fetch the routes for the RestfulRouteConventionWithExplicitOrderedRoute controller's Index action
	Then the 1st route url is "RestfulRouteConventionWithExplicitOrderedRoute/Primary"
	 And the 2nd route url is "RestfulRouteConventionWithExplicitOrderedRoute"
