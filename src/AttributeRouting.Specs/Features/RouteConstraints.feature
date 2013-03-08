Feature: Route Constraints

Scenario Outline: Inline constraints
	# MVC
	Given I have registered the routes for the InlineRouteConstraintsController
	When I fetch the routes for the InlineRouteConstraints controller's <actionName> action
	Then the route url is "Inline-Constraints/<actionName>/{x}"
	And the parameter "x" is constrained by an inline AttributeRouting.Web.Constraints.<constraintTypeName>
	# Web API
	Given I have registered the routes for the HttpInlineRouteConstraintsController
	When I fetch the routes for the HttpInlineRouteConstraints controller's <actionName> action
	Then the route url is "Http-Inline-Constraints/<actionName>/{x}"
	And the parameter "x" is constrained by an inline AttributeRouting.Web.Constraints.<constraintTypeName>
	Examples: 
	| actionName  | constraintTypeName       |
	| Alpha       | AlphaRouteConstraint     |
	| Int         | IntRouteConstraint       |
	| Long        | LongRouteConstraint      |
	| Float       | FloatRouteConstraint     |
	| Double      | DoubleRouteConstraint    |
	| Decimal     | DecimalRouteConstraint   |
	| Bool        | BoolRouteConstraint      |
	| Guid        | GuidRouteConstraint      |
	| DateTime    | DateTimeRouteConstraint  |
	| Length      | LengthRouteConstraint    |
	| MinLength   | MinLengthRouteConstraint |
	| MaxLength   | MaxLengthRouteConstraint |
	| LengthRange | LengthRouteConstraint    |
	| Min         | MinRouteConstraint       |
	| Max         | MaxRouteConstraint       |
	| Range       | RangeRouteConstraint     |
	| Regex       | RegexRouteConstraint     |
	| RegexRange  | RegexRouteConstraint     |
	| Compound    | IntRouteConstraint       |
	| Compound    | MaxRouteConstraint       |

Scenario: Inline constraints in the querystring
	# MVC
	Given I have registered the routes for the InlineRouteConstraintsController
	When I fetch the routes for the InlineRouteConstraints controller's Querystring action
	Then the route url is "Inline-Constraints/Querystring"
	And the parameter "x" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint
	And the parameter "y" is constrained by an inline AttributeRouting.Web.Constraints.QueryStringRouteConstraint
	# Web API - NOTE: this won't work until web api vNext
	#Given I have registered the routes for the HttpInlineRouteConstraintsController
	#When I fetch the routes for the HttpInlineRouteConstraints controller's Querystring action
	#Then the route url is "Http-Inline-Constraints/Querystring"
	#And the parameter "x" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint
	#And the parameter "y" is constrained by an inline AttributeRouting.Web.Constraints.QueryStringRouteConstraint

Scenario: Multiple inline constraints per url segment
	# MVC
	Given I have registered the routes for the InlineRouteConstraintsController
	When I fetch the routes for the InlineRouteConstraints controller's MultipleWithinUrlSegment action
	Then the route url is "Inline-Constraints/avatar/{width}x{height}/{image}"
	And the parameter "width" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint
	And the parameter "height" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint
	# Web API
	Given I have registered the routes for the HttpInlineRouteConstraintsController
	When I fetch the routes for the HttpInlineRouteConstraints controller's MultipleWithinUrlSegment action
	Then the route url is "Http-Inline-Constraints/avatar/{width}x{height}/{image}"
	And the parameter "width" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint
	And the parameter "height" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint

Scenario: Inline constraints specified in the RoutePrefixAttribute
	# MVC
	Given I have registered the routes for the PrefixedInlineRouteConstraintsController
	When I fetch the routes for the PrefixedInlineRouteConstraints controller's Index action
	Then the route url is "Prefixed-Inline-Constraints/{id}/Howdy"
	And the parameter "id" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint
	# Web API
	Given I have registered the routes for the HttpPrefixedInlineRouteConstraintsController
	When I fetch the routes for the HttpPrefixedInlineRouteConstraints controller's Index action
	Then the route url is "Http-Prefixed-Inline-Constraints/{id}/Howdy"
	And the parameter "id" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint

Scenario: Inline constraints specified in the RouteAreaAttribute
	# MVC
	Given I have registered the routes for the AreaInlineRouteConstraintsController
	When I fetch the routes for the AreaInlineRouteConstraints controller's Index action
	Then the route url is "Area-Inline-Constraints/{id}/Howdy"
	And the parameter "id" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint
	# Web API
	Given I have registered the routes for the HttpAreaInlineRouteConstraintsController
	When I fetch the routes for the HttpAreaInlineRouteConstraints controller's Index action
	Then the route url is "Http-Area-Inline-Constraints/{id}/Howdy"
	And the parameter "id" is constrained by an inline AttributeRouting.Web.Constraints.IntRouteConstraint

Scenario Outline: Matching inline route constraints
	# MVC
	Given I have registered the routes for the InlineRouteConstraintsController
	When a request for "Inline-Constraints/<url>" is made
	Then the <action> action <condition> matched
	# Web API
	Given I have registered the routes for the HttpInlineRouteConstraintsController
	When a request for "Http-Inline-Constraints/<url>" is made
	Then the <action> action <condition> matched
	Examples:
	| url                                       | action       | condition |
	| Alpha/abc                                 | Alpha        | is        |
	| Alpha/123                                 | Alpha        | is not    |
	| Int/53                                    | Int          | is        |
	| Int/abc                                   | Int          | is not    |
	| IntOptional                               | IntOptional  | is        |
	| Long/79                                   | Long         | is        |
	| Long/xyz                                  | Long         | is not    |
	| Float/1.334                               | Float        | is        |
	| Float/gg2                                 | Float        | is not    |
	| Double/3.14                               | Double       | is        |
	| Double/adf78                              | Double       | is not    |
	| Decimal/99.32123345                       | Decimal      | is        |
	| Decimal/d8uasdf                           | Decimal      | is not    |
	| Bool/true                                 | Bool         | is        |
	| Bool/false                                | Bool         | is        |
	| Bool/truish                               | Bool         | is not    |
	| Bool/falsish                              | Bool         | is not    |
	| Guid/6076668C-57AA-47FD-A914-94FD552C8493 | Guid         | is        |
	| Guid/6076668C-57AA-47FD-A914-94FD552C     | Guid         | is not    |
	| DateTime/2012-4-22                        | DateTime     | is        |
	| DateTime/Today                            | DateTime     | is not    |
	| Length/a                                  | Length       | is        |
	| Length/aa                                 | Length       | is not    |
	| MinLength/abcdefghi                       | MinLength    | is not    |
	| MinLength/abcdefghij                      | MinLength    | is        |
	| MaxLength/abcdefghij                      | MaxLength    | is        |
	| MaxLength/abcdefghijk                     | MaxLength    | is not    |
	| LengthRange/abcdefghijk                   | LengthRange  | is not    |
	| LengthRange/a                             | LengthRange  | is not    |
	| LengthRange/ab                            | LengthRange  | is        |
	| LengthRange/abcdefghij                    | LengthRange  | is        |
	| LengthRange/abcdefghijk                   | LengthRange  | is not    |
	| Min/0                                     | Min          | is not    |
	| Min/1                                     | Min          | is        |
	| Max/10                                    | Max          | is        |
	| Max/11                                    | Max          | is not    |
	| Range/0                                   | Range        | is not    |
	| Range/1                                   | Range        | is        |
	| Range/10                                  | Range        | is        |
	| Range/11                                  | Range        | is not    |
	| Regex/Howdy                               | Regex        | is        |
	| Regex/BoyHowdy                            | Regex        | is not    |
	| Compound/5                                | Compound     | is        |
	| Compound/5.0                              | Compound     | is not    |
	| Enum/red                                  | Enum         | is        |
	| Enum/taupe                                | Enum         | is not    |
	| EnumValue/1                               | EnumValue    | is        |
	| EnumValue/10                              | EnumValue    | is not    |
	| WithOptional                              | WithOptional | is        |
	| WithDefault                               | WithDefault  | is        |

Scenario Outline: Matching inline route constraints in the querystring
	# MVC
	Given I have registered the routes for the InlineRouteConstraintsController
	When a request for "Inline-Constraints/<url>" is made
	Then the <action> action <condition> matched
	# Web API - NOTE: these won't work until web api vNext.
	#Given I have registered the routes for the HttpInlineRouteConstraintsController
	#When a request for "Http-Inline-Constraints/<url>" is made
	#Then the <action> action <condition> matched
	Examples:
	| url                                       | action       | condition |
	| Querystring?x=123&y=hello                 | Querystring  | is        |
	| Querystring?x=abc&y=hello                 | Querystring  | is not    |
	| Querystring?x=abc                         | Querystring  | is not    |
	| Querystring?y=hello                       | Querystring  | is not    |
