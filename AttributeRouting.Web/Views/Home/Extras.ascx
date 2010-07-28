<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<h3>Extra! Extra!</h3>

<h4>HtmlHelpers that Generate PUT-, POST-, and DELETE-Enabled Forms</h4>
<p>
    Because the AttributeRouting library supports PUT and DELETE requests,
    but most browsers don't, the library resorts to some tomfoolery to work around this limitation.
    The trick involves using the MVC HttpMethodOverride html helper. To facilitate
    generating forms that post to the correct action and include an HttpMethodOverride call,
    there are three HtmlHelpers available:
</p>
<pre class="code">
    <code class="xml csharp boc-highlight[1,2,3]">
        Html.BeginFormPUT("Create")
        Html.BeginFormPOST("Update")
        Html.BeginFormDELETE("Destroy")
    </code>
</pre>
<p>
    And yes, there is a BeginFormPOST helper. It is available because to the routes generated from
    the AttribtueRouting library constrain POST routes on the http method POST.
    In order to generate the correct action for the form, this constraint must be 
    specified in the route data when generating the url. You could do it yourself,
    but it would be a repititious pain in the butt. Let the helper do it for you!
</p>

<h4>Log All Registered Routes to the Browser</h4>
<p>
    The AttributeRouting library includes a LogRoutesHandler that emits the 
    routes in the RouteTable to a browser. To use it, add the following line
    to your web.config:
</p>
<pre class="code">
    <code class="xml csharp boc-highlight[2]">
        <%: @"
        <httpHandlers>
          <add path=""routes.axd"" verb=""GET"" type=""AttributeRouting.LogRoutesHandler, AttributeRouting, Version=1.0.0.0, Culture=neutral""/>
        </httpHandlers>" %>
    </code>
</pre>
<p>
    The output won't win a beauty contest, but it will help you see what routes 
    have been registered by your application and in what order.
</p>

<h4>CodeGen Restful Controllers and Views With T4</h4>
<p>
    The web project in the AttributeRouting solution includes T4 templates for generating
    restful controllers decorated with RouteAttributes and for generating restful
    Create, Edit, and Delete views that use PUT, POST, and DELETE to hit the correct routes.
</p>
