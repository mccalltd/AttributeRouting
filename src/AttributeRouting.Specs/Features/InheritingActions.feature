Feature: Inheriting Actions

Background:
	Given I generate the routes defined in the subject controllers

Scenario: Inheriting actions in a derived controller
	When I fetch the routes for the SuperController's Index action
	Then the route url is "InheritedActions/Index"
	When I fetch the routes for the DerivedController's Index action
	Then the route url is "InheritedActions/Index"

Scenario: Inheriting actions in a derived controller overriding the url of an action
	When I fetch the routes for the SuperController's Index action
	Then the route url is "InheritedActions/Index"
	When I fetch the routes for the DerivedWithOverrideController's Index action
	Then the route url is "InheritedActions/IndexDerived"

Scenario: Inheriting actions in a derived controller that specifies an area
	When I fetch the routes for the SuperWithAreaController's Index action
	Then the route url is "Super/InheritedActions/Index"
	When I fetch the routes for the DerivedWithAreaController's Index action
	Then the route url is "Derived/InheritedActions/Index"

Scenario: Inheriting actions in a derived controller that specifies a prefix
	When I fetch the routes for the SuperWithPrefixController's Index action
	Then the route url is "InheritedActions/Super/Index"
	When I fetch the routes for the DerivedWithPrefixController's Index action
	Then the route url is "InheritedActions/Derived/Index"
