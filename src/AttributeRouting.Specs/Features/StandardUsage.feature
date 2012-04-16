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
		| method | action    | url                   |
		| GET    | Index     | Index                 |
		| HEAD   | Index     | Index                 |
		| POST   | Create    | Create                |
		| PUT    | Update    | Update/{id}           |
		| DELETE | Destroy   | Destroy/{id}          |
		| GET    | Wildcards | Wildcards/{*pathInfo} |
		|        | AnyVerb   | AnyVerb               |

Scenario Outline: Generating routes for an API controller
	When I fetch the routes for the HttpStandardUsage controller's <action> action
	Then the route is constrained to <method> requests
	 And the route url is "<url>"
	 And the default for "controller" is "HttpStandardUsage"
	 And the default for "action" is "<action>"
	 And the namespace is "AttributeRouting.Specs.Subjects.Http"
	
	Examples:
		| method | action    | url                       |
		| GET    | Get       | api                       |
		| HEAD   | Get       | api                       |
		| POST   | Post      | api                       |
		| PUT    | Put       | api/{id}                  |
		| DELETE | Delete    | api/{id}                  |
		| GET    | Wildcards | api/Wildcards/{*pathInfo} |
		|        | AnyVerb   | api/AnyVerb               |