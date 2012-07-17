using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RoutePrefix("InheritedActions")]
    public class SuperController : Controller
    {
        [GET("Index")]
        public virtual string Index()
        {
            return null;
        }
    }

    public class DerivedController : SuperController { }
    
    public class DerivedWithOverrideController : SuperController
    {
        [GET("IndexDerived")]
        public override string Index()
        {
            return base.Index();
        }
    }

    [RouteArea("Super")]
    public class SuperWithAreaController : SuperController { }

    [RouteArea("Derived")]
    public class DerivedWithAreaController : SuperWithAreaController { }

    [RoutePrefix("InheritedActions/Super")]
    public class SuperWithPrefixController : SuperController { }

    [RoutePrefix("InheritedActions/Derived")]
    public class DerivedWithPrefixController : SuperController { }
}
