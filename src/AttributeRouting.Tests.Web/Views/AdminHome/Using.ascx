<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<h3>More Advanced Usage of the AttributeRouting Library</h3>

<h4>Multiple Routes Mapped to a Single Action</h4>
<p>
    You can specify multiple routes that map to a single action simply
    by adding more than one route attribute. To specify the precedence 
    of these routes, use the Order parameter. You can also use multiple
    routes with different verbs.
</p>
<pre class="code">
    <code class="csharp boc-highlight[1,2,3,9,10]">
        [GET("", Order = 1)]
        [GET("Posts", Order = 2)]
        [GET("Posts/Index", Order = 3)]
        public void Index()
        {
            return View();
        }

        [GET("Posts/Search")]
        [POST("Posts/Search")]
        public void Search()
        {
            return View();
        }
    </code>
</pre>

<h4>Route Defaults</h4>
<p>
    To specify a route default, use the RouteDefaultAttribute.
    This attribute takes the route parameter name and a default value.
</p>
<pre class="code">
    <code class="csharp boc-highlight[1,2,3]">
        [GET("Example/RouteDefault/{param1}/{param2}")]
        [RouteDefault("param1", "frankie")]
        [RouteDefault("param2", "baby")]
        public ActionResult Index(string param1, string param2)
        {
            return View();
        }
    </code>
</pre>

<h4>Route Constraints</h4>
<p>
    To specify a route constraint, you can use the built-in RegexRouteConstraintAttribute.
</p>
<pre class="code">
    <code class="csharp boc-highlight[1,2]">
        [GET("Example/RouteConstraint/{param}")]
        [RegexRouteConstraint("param", @"^rats$")]
        public ActionResult Index(string param)
        {
            return View();
        }
    </code>
</pre>
<p>
    If you need to do more than what regex can handle (which I'm sure you will),
    the AttributeRouting library provides an abstract RouteConstraintAttribute,
    which you may derive from and implement in your own code, hooking in a custom constraint. 
</p>
<pre class="code">
    <code class="csharp boc-highlight[15]">
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
        public abstract class RouteConstraintAttribute : Attribute
        {
            protected RouteConstraintAttribute(string key)
            {
                if (key == null) throw new ArgumentNullException("key");

                Key = key;
            }

            public string Key { get; private set; }

            public string ForRouteNamed { get; set; }

            public abstract IRouteConstraint Constraint { get; }
        }
    </code>
</pre>
<p>
    To roll your own RouteConstraintAttribute, simply implement the Constraint property
    by returning an IRouteConstraint that will be applied against the route parameter 
    with the supplied Key. Then decorate your action methods with your custom attribute.
</p>

<h4>Route Defaults and Constraints for Actions with Multiple Route</h4>
<p>
    This is where things can turn into attribute spaghetti!
    If you specify multiple routes for an action and these routes have different defaults
    and constraints, then you need to use the RouteName and ForRouteNamed attribute properties
    to keep things straight. This is best illustrated with an example:
</p>
<pre class="code">
    <code class="csharp boc-highlight[1,2,3,4,5,6,7,8,9]">
        [GET("Example/MakingAMess/{p1}/{p2}", Order = 1, RouteName = "FirstRoute")]
        [RouteDefault("p1", "fly", ForRouteNamed = "FirstRoute")]
        [RouteDefault("p2", "me", ForRouteNamed = "FirstRoute")]
        [RegexRouteConstraint("p2", "^me$|^thee$", ForRouteNamed = "FirstRoute")]
        [GET("Example/MakingAMess/{p1}/{p2}/{p3}", Order = 2, RouteName = "SecondRoute")]
        [RouteDefault("p1", "to", ForRouteNamed = "SecondRoute")]
        [RouteDefault("p2", "the", ForRouteNamed = "SecondRoute")]
        [RouteDefault("p3", "moon", ForRouteNamed = "SecondRoute")]
        [RegexRouteConstraint("p3", "^moon$|^mooooo-oooooon$", ForRouteNamed = "SecondRoute")]
        public ActionResult MakingAMess(string p1, string p2, string p3)
        {
            return View();
        }
    </code>
</pre>
    
<h4>Areas</h4>
<p>
    Areas can be created with the class-level RouteAreaAttribute.
    All the routes defined in the controller will be mapped to the area specified.
</p>
<pre class="code">
    <code class="csharp boc-highlight[1,4]">
        [RouteArea("Admin")]
        public class PostsController : ControllerBase
        {
            [GET("Posts/Index")]
            public ActionResult Index()
            {
                return View();
            }
        }
    </code>
</pre>
<p>
    If you are defining more than one controller for an area, consider using a base
    controller decorated with the RouteAreaAttribute, and deriving all the controllers
    in the area from the base controller.
</p>
<pre class="code">
    <code class="csharp boc-highlight[1,4,5,6]">
        [RouteArea("Admin")]
        public abstract class AdminControllerBase : Controller {}

        public class PostsController : AdminControllerBase {}
        public class CommentsController : AdminControllerBase {}
        public class TagsController : AdminControllerBase {}
    </code>
</pre>
<p>
    In the above example, the routes defined in PostsController, CommentsController,
    and TagsController will all be organized into an "Admin" area.
</p>
<p>
    NOTE: If you use areas, remember to generate urls with an "area" route value:
</p>
<pre class="code">
    <code class="csharp">
        Url.Action("Index", "Products", new { area = "Admin" });
    </code>
</pre>
    
<h4>Nested Routes</h4>
<p>
    Nesting routes is simple using the class-level RoutePrefixAttribute.
    All the routes in a decorated controller will have the specified url prefix
    shoved in front of the url defined in the route.
</p>
<pre class="code">
    <code class="csharp boc-highlight[1,4,10]">
        [RoutePrefix("Posts/{postId}")]
        public class CommentsController : ControllerBase    
        {
            [GET("Comments")]
            public ActionResult Index(int postId)
            {
                return View();
            }

            [GET("Comments/{id}")]
            public ActionResult Show(int postId, int id)
            {
                return View();
            }
        }
    </code>
</pre>
<p>
    Of course, you can always type out the full nested path in the RouteAttribute
    decorating the action method. It's your call. The above example rewritten looks like this:
</p>
<pre class="code">
    <code class="csharp boc-highlight[3,9]">
        public class CommentsController : ControllerBase    
        {
            [GET("Posts/{postId}/Comments")]
            public ActionResult Index(int postId)
            {
                return View();
            }

            [GET("Posts/{postId}/Comments/{id}")]
            public ActionResult Show(int postId, int id)
            {
                return View();
            }
        }
    </code>
</pre>
