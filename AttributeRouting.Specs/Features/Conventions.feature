Feature: Defining Routes Via Conventions

Background: 
	Given I generate the routes defined in the subject controllers

Scenario Outline: Generating routes using the RestfulRouteConvention
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
		| Custom	| GET		| RestfulRouteConventionTest/Custom			|

Scenario Outline: Generating routes using the RestfulRouteConvention on controllers with a RoutePrefix attribute
	When I fetch the routes for the RestfulRouteConventionPrefixTest controller's <action> action
	Then the route url is "<url>"
	 And the default for "controller" is "RestfulRouteConventionPrefixTest"
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
	When I fetch the routes for the RestfulRouteConventionTest controller's Index action
	Then the 1st route url is "RestfulRouteConventionTest"
	 And the 2nd route url is "Legacy"
