Feature: Defining Routes Via Conventions

Background: 
	Given I generate the routes defined in the subject controllers

Scenario Outline: Generating routes using a custom RouteConvention
	When I fetch the routes for the RestfulRouteConventionTest controller's <action> action
	Then the route url is "<url>"
	 And the default for "controller" is "RestfulRouteConventionTest"
	 And the default for "action" is "<action>"
	 And the route for <action> is constrained to <method> requests

	Examples:
		| action	| method	| url										|
		| Index		| GET		| RestfulRouteConventionTest				|
		| New		| GET		| RestfulRouteConventionTest/New			|		
		| Create	| POST		| RestfulRouteConventionTest				|		
		| Show		| GET		| RestfulRouteConventionTest/{id}			|		
		| Edit		| GET		| RestfulRouteConventionTest/{id}/Edit		|		
		| Update	| PUT		| RestfulRouteConventionTest/{id}			|		
		| Delete	| GET		| RestfulRouteConventionTest/{id}/Delete	|		
		| Destroy	| DELETE	| RestfulRouteConventionTest/{id}			|		
