using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttributeRouting.Web.Controllers.Admin
{
    public class UsersController : AdminControllerBase
    {
        [GET("Users")]
        public ActionResult Index()
        {
            return View();
        }
    }
}