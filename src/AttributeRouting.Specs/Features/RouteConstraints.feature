Feature: Route Constraints

Background: 
	Given I generate the routes defined in the subject controllers

Scenario: Regex route constraints specified with an attribute
	When I fetch the routes for the RouteConstraints controller's Index action
	Then the parameter "p1" is constrained by the pattern "\d+"
	When I fetch the routes for the ApiRouteConstraints controller's Get action
	Then the parameter "p1" is constrained by the pattern "\d+"

Scenario: Regex route constraints specified inline
	When I fetch the routes for the RouteConstraints controller's InlineConstraints action
	Then the route url is "InlineConstraints/{number}/{word}/{alphanum}/{capture}"
	Then the parameter "number" is constrained by the pattern "\d+"
	Then the parameter "word" is constrained by the pattern "\w{2}"
	Then the parameter "alphanum" is constrained by the pattern "[A-Za-z0-9]*"
	Then the parameter "capture" is constrained by the pattern "(gotcha)"
	When I fetch the routes for the HttpRouteConstraints controller's InlineConstraints action
	Then the route url is "InlineConstraints/{number}/{word}/{alphanum}/{capture}"
	Then the parameter "number" is constrained by the pattern "\d+"
	Then the parameter "word" is constrained by the pattern "\w{2}"
	Then the parameter "alphanum" is constrained by the pattern "[A-Za-z0-9]*"
	Then the parameter "capture" is constrained by the pattern "(gotcha)"

Scenario Outline: Inline constraints
	# MVC
	When I fetch the routes for the InlineRouteConstraints controller's <actionName> action
	Then the route url is "Inline-Constraints/<actionName>/{x}"
	And the parameter "x" is constrained by an inline AttributeRouting.Web.Constraints.<constraintTypeName>
	# Web API
	When I fetch the routes for the HttpInlineRouteConstraints controller's <actionName> action
	Then the route url is "Http-Inline-Constraints/<actionName>/{x}"
	And the parameter "x" is constrained by an inline AttributeRouting.Web.Constraints.<constraintTypeName>
	Examples: 
	| actionName  | constraintTypeName                                         |
	| Int         | IntRouteConstraint                                         |
	| Long        | LongRouteConstraint                                        |
	| Float       | FloatRouteConstraint                                       |
	| Double      | DoubleRouteConstraint                                      |
	| Decimal     | DecimalRouteConstraint                                     |
	| Bool        | BoolRouteConstraint                                        |
	| Length      | LengthRouteConstraint                                      |
	| MinLength   | MinLengthRouteConstraint                                   |
	| MaxLength   | MaxLengthRouteConstraint                                   |
	| LengthRange | LengthRouteConstraint                                      |
	| Min         | MinRouteConstraint                                         |
	| Max         | MaxRouteConstraint                                         |
	| Range       | RangeRouteConstraint                                       |
	| Regex       | RegexRouteConstraint                                       |

Scenario: Compound inline constraints
	# MVC
	When I fetch the routes for the InlineRouteConstraints controller's Compound action
	Then the route url is "Inline-Constraints/Compound/{x}"
	And the parameter "x" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint
	And the parameter "x" is constrained by an inline AttributeRouting.Web.Constraints.MaxRouteConstraint
	# Web API
	When I fetch the routes for the HttpInlineRouteConstraints controller's Compound action
	Then the route url is "Http-Inline-Constraints/Compound/{x}"
	And the parameter "x" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint
	And the parameter "x" is constrained by an inline AttributeRouting.Web.Constraints.MaxRouteConstraint

Scenario: Multiple routes with different constraints
	When I fetch the routes for the RouteConstraints controller's MultipleRoutes action
	Then the route named "MultipleConstraints1" has a constraint on "p1" of "\d+"
	And the route named "MultipleConstraints2" has a constraint on "p1" of "\d{4}" 
	And the route named "ApiMultipleConstraints1" has a constraint on "p1" of "\d+"
	And the route named "ApiMultipleConstraints2" has a constraint on "p1" of "\d{4}"
	When I fetch the routes for the HttpRouteConstraints controller's MultipleRoutes action
	Then the route named "MultipleConstraints1" has a constraint on "p1" of "\d+"
	And the route named "MultipleConstraints2" has a constraint on "p1" of "\d{4}" 
	And the route named "ApiMultipleConstraints1" has a constraint on "p1" of "\d+"
	And the route named "ApiMultipleConstraints2" has a constraint on "p1" of "\d{4}"
