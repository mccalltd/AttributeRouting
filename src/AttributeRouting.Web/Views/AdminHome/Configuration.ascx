<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<h3>Configuration Options When Generating Routes</h3>

<h4>Overview</h4>
<p>
    To map the routes defined via RouteAttributes, you must use an overload of the
    MapAttributeRoutes extension. There are three ways to map your routes.
    The first looks like this:
</p>
<pre class="code">
    <code class="csharp boc-highlight[3]">
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapAttributeRoutes();

        routes.MapRoute("CatchAll",
                        "{*path}",
                        new { controller = "home", action = "filenotfound" },
                        new[] { typeof(HomeController).Namespace });    
    </code>
</pre>
<p>
    This overload tells the AttributeRouting library to scan the calling assembly
    for any RouteAttributes it can find, and create routes in the MVC RouteTable 
    for each one found.
</p>
<p>
    Other overloads of MapAttributeRoutes allow you to customize the default behavior
    by passing a configuration object as an argument, or by specifying a configuration 
    initialization expression via an Action delegate.
</p>

<h4>Scanning Assemblies</h4>
<p>
    To specify the assemblies to scan for RouteAttributes, use either the ScanAssemblyOf or ScanAssembly method.
</p>
<pre class="code">
    <code class="csharp boc-highlight[3,4]">
        routes.MapAttributeRoutes(config =>
        {
            config.ScanAssembly(Assembly.GetExecutingAssembly());
            config.ScanAssemblyOf&lt;TestController&gt;();
        });
    </code>
</pre>

<h4>Route Precedence</h4>
<p>
    By default, the AttributeRouting route generator adds routes into the RouteTable in
    the order that the routes are found via reflection. To control the precedence of the
    generated routes, use the AddRoutesFromController method.
</p>
<pre class="code">
    <code class="csharp boc-highlight[4,5]">
        routes.MapAttributeRoutes(config =>
        {
            config.ScanAssembly(Assembly.GetExecutingAssembly());
            config.AddRoutesFromController&lt;PostsController&gt;();
            config.AddRoutesFromController&lt;HomeController&gt;();
            config.AddTheRemainingScannedRoutes();
        });
    </code>
</pre>
<p>
    In the above example, the routes from the PostsController will be added
    before the route for the HomeController. When specifying route precendence,
    you <i>must</i> explicitly call the AddRemainingScannedRoutes method
    if you want to add the remaining routes to the end of the RouteTable. 
</p>
<p>
    You can also choose to simply configure the routes for a single controller
    or set of controllers, without scanning all the routes in a given assembly.
</p>
<pre class="code">
    <code class="csharp boc-highlight[1]">
        routes.MapAttributeRoutes(config =>
        {
            config.AddRoutesFromController&lt;PostsController&gt;();
            config.AddRoutesFromController&lt;HomeController&gt;();
        });
    </code>
</pre>
<p>
    There is another method, AddRoutesFromControllersOfType, which is useful if you
    want to register all the routes from an area, say, before the other routes
    in your application. This is convenient if you use a base class for an area.
</p>
<pre class="code">
    <code class="csharp boc-highlight[4]">
        routes.MapAttributeRoutes(config =>
        {
            config.ScanAssembly(Assembly.GetExecutingAssembly());
            config.AddRoutesFromControllersOfType&lt;AdminController&gt;();
            config.AddTheRemainingScannedRoutes();
        });
    </code>
</pre>
<p>
    So what about specifying the route precendence among the actions inside your controller? 
    The routes for the actions will be added in the order in which the actions are declared.
    And remember, if you have multiple routes mapped to a single action,
    you can specify a value for the Order property of the RouteAttributes.
</p>

<h4>Constraining Route Parameters Mapped to Primitive Types</h4>
<p>
    By default, when generating routes, if an action method has a primitive parameter
    in its signature, and the route for the action contains a parameter of the same name,
    the generated route will constrain the route parameter on valid values 
    for the action method parameter. What a mouthful - here's an example:
</p>
<pre class="code">
    <code class="csharp boc-highlight[1,2]">
        [GET("Posts/{id}")]
        public ActionResult Show(int id)
        {
            return View();
        }
    </code>
</pre>
<p>
    The {id} parameter in the url is also present in the action method. The generated
    route will constrain the id on valid values for the int type. As a result, a request
    for "/Posts/ABC" will not match the route defined on the Show action and will fall
    through the route table. If you would like to disable this feature, do this:
</p>
<pre class="code">
    <code class="csharp boc-highlight[4]">
        routes.MapAttributeRoutes(config =>
        {
            config.ScanAssembly(Assembly.GetExecutingAssembly());
            config.ConstrainPrimitiveRouteParameters = false;
        });
    </code>
</pre>
<p>
    NOTE: Currently only int parameters are automatically constrained.
</p>

<h4>Lowercase Url Convention</h4>
<p>
    To have the AttributeRoutes created by MapAttributeRoutes generate
    lowercase outbound urls, you can set the UseLowercaseRoutes property in 
    the configuration object:
</p>
<pre class="code">
    <code class="csharp boc-highlight[4]">
        routes.MapAttributeRoutes(config =>
        {
            config.ScanAssembly(Assembly.GetExecutingAssembly());
            config.UseLowercaseRoutes = true;
        });
    </code>
</pre>
