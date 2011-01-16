Feature: Logging

Scenario: Log the routes to the standard output
	Given I generate the routes defined in the subject controllers
	When I log the routes
	Then ta-da!
