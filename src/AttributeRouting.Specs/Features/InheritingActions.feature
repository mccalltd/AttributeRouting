Feature: Inheriting Actions

Scenario: Inheriting actions in a derived controller
	Given I have registered the routes for the SuperController
	When I fetch the routes for the SuperController's Index action
	Then the route url is "InheritedActions/Index"
	Given I have registered the routes for the DerivedController
	When I fetch the routes for the DerivedController's Index action
	Then the route url is "InheritedActions/Index"

Scenario: Inheriting actions in a derived controller overriding the url of an action
	Given I have registered the routes for the SuperController
	When I fetch the routes for the SuperController's Index action
	Then the route url is "InheritedActions/Index"
	Given I have registered the routes for the DerivedWithOverrideController
	When I fetch the routes for the DerivedWithOverrideController's Index action
	Then the route url is "InheritedActions/IndexDerived"

Scenario: Inheriting actions in a derived controller that specifies an area
	Given I have registered the routes for the SuperWithAreaController
	When I fetch the routes for the SuperWithAreaController's Index action
	Then the route url is "Super/InheritedActions/Index"
	Given I have registered the routes for the DerivedWithAreaController
	When I fetch the routes for the DerivedWithAreaController's Index action
	Then the route url is "Derived/InheritedActions/Index"

Scenario: Inheriting actions in a derived controller that specifies a prefix
	Given I have registered the routes for the SuperWithPrefixController
	When I fetch the routes for the SuperWithPrefixController's Index action
	Then the route url is "InheritedActions/Super/Index"
	Given I have registered the routes for the DerivedWithPrefixController
	When I fetch the routes for the DerivedWithPrefixController's Index action
	Then the route url is "InheritedActions/Derived/Index"
