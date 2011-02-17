Feature: Standard Usage

Background: 
	Given I generate the routes defined in the subject controllers

Scenario Outline: Generating routes for an action method
	When I fetch the routes for the StandardUsage controller's <action> action
	Then the route is constrained to <method> requests
	 And the route url is "<url>"
	 And the default for "controller" is "StandardUsage"
	 And the default for "action" is "<action>"
	 And the namespace is "AttributeRouting.Specs.Subjects"
	
	Examples:
		| action	| method	| url			|
		| Index		| GET		| Index			|
		| Create	| POST		| Create		|
		| Update	| PUT		| Update/{id}	|
		| Destroy	| DELETE	| Destroy/{id}	|