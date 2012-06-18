using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Threading;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
	public class AsyncActionController : AsyncController
	{
		[GET("WithAsync/Synchronous")]
		public ActionResult Test1()
		{
			return View();
		}

		[GET("WithAsync/NotSynchronous")]
		public void Test2Async()
		{
			AsyncManager.OutstandingOperations.Increment();
			Task.Factory.StartNew(() =>
				{
					Thread.Sleep(5000);
					AsyncManager.OutstandingOperations.Decrement();
				});
		}

		public ViewResult Test2Completed()
		{
			return View();
		}
	}
}
