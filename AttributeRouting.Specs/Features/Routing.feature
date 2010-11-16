Feature: Route Builder
	In order to generate routes for the MVC framework
	As a user of the library
	I want to have routes built according to my specifications

Background: 
	Given I generate the routes defined in the subject controllers

Scenario: Generating routes for an action method
	When I fetch the routes for the RestfulTest controller's Index action
	Then the route url is "Resources"
	 And the default for "controller" is "RestfulTest"
	 And the default for "action" is "Index"
	 And the namespace is "AttributeRouting.Specs.Subjects"

Scenario: Generating routes with the restful http method attribute
	When I fetch the routes for the RestfulTest controller
	Then the route for Index is constrained to GET requests
	 And the route for Create is constrained to POST requests
	 And the route for Update is constrained to PUT requests
	 And the route for Destroy is constrained to DELETE requests

Scenario: Optional parameters specified with a url parameter token
	When I fetch the routes for the Test controller's OptionalParametersWithAToken action
	Then the route url is "Test/Optionals/{p1}/{p2}/{p3}"
	 And the parameter p1 is optional
	 And the parameter p2 is optional
	 And the parameter p3 is optional
	 
Scenario: Multiple routes for a single action
	When I fetch the routes for the Test controller's MultipleRoutes action
	Then 3 routes should be found
	 And the 1st route url is "Test/Multiple"
	 And the 2nd route url is "Test/Multiple/Routes"
	 And the 3rd route url is "Test/Multiple/Routes/Again"

Scenario: Route defaults
	When I fetch the routes for the Test controller's Default action
	Then the default for "param1" is "mapleleaf"

Scenario: Regex route constraints specified with an attribute
	When I fetch the routes for the Test controller's Constraint action
	Then the parameter "cat" is constrained by the pattern "^(kitty|meow-meow|purrbot)$"

Scenario: Multiple routes with different defaults and constraints for a single action
	When I fetch the routes for the Test controller's MultipleRoutesWithDefaultsAndConstraints action
	Then the route named "FirstDitty" has a default for "number" of 666
	 And the route named "FirstDitty" has a contraint on "number" of "^\d{4}$" 
	 And the route named "SecondDitty" has a default for "number" of 777
	 And the route named "SecondDitty" has a contraint on "number" of "^\d{1}$" 