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
		| action	| method	| url			|
		| Index		| GET		|				|
		| New		| GET		| New			|		
		| Create	| POST		|				|		
		| Show		| GET		| {id}			|		
		| Edit		| GET		| {id}/Edit		|		
		| Update	| PUT		| {id}			|		
		| Delete	| GET		| {id}/Delete	|		
		| Destroy	| DELETE	| {id}			|		
		| Custom	| GET		| Custom		|

Scenario: Generating routes using the RestfulRouteConvention on actions with an explicit route defined
	When I fetch the routes for the RestfulRouteConventionTest controller's Index action
	Then the 1st route url is ""
	 And the 2nd route url is "Legacy"
