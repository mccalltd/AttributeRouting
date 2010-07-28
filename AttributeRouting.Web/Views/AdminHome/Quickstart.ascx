<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<h3>Quickstart</h3>
<p>
    Use RouteAttributes to decorate actions in your controllers. 
    There is a specific RouteAtribute for each of the four HTTP verbs: 
    GET, PUT, POST, and DELETE.
</p>
<p>
    Below is an example of defining routes for a resource in a RESTful style.
    Note that all four verbs are represented.
</p>
<pre class="code">
    <code class="csharp boc-highlight[3,4,10,16,23,29,35,42,48]">
        public class ResourcesController : ControllerBase
        {
            [GET("Resources")]
            [GET("Resources/Index")]
            public ActionResult Index()
            {
                return View();
            }

            [GET("Resources/New")]
            public ActionResult New()
            {
                return View();
            }

            [PUT("Resources")]
            public ActionResult Create()
            {
                Flash("Resource Created");
                return RedirectToAction("show", new { id = 1 });
            }

            [GET("Resources/{id}")]
            public ActionResult Show(int id)
            {
                return View();
            }

            [GET("Resources/{id}/Edit")]
            public ActionResult Edit(int id)
            {
                return View();
            }

            [POST("Resources/{id}")]
            public ActionResult Update(int id)
            {
                Flash("Resource Updated");
                return RedirectToAction("show");
            }

            [GET("Resources/{id}/Delete")]
            public ActionResult Delete(int id)
            {
                return View();
            }

            [DELETE("Resources/{id}")]
            public ActionResult Destroy(int id)
            {
                Flash("Resource Destroyed");
                return RedirectToAction("index");
            }
        }
    </code>
</pre>
<p>
    Once you have added the RouteAttributes to your action methods,
    you will need to have the AttributeRouting library find them and create
    corresponding routes in the MVC RouteTable. You can do this in your Global.asax:
</p>
<pre class="code">
    <code class="csharp boc-highlight[5]">
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapAttributeRoutes();
        }
    </code>
</pre>
<p>
    When you're done, hit F5. That's it.
</p>
