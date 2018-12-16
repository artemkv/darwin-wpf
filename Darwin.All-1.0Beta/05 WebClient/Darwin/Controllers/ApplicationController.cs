using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Darwin.Controllers
{
	[Authorize]
	public class ApplicationController : Controller
    {
        public ActionResult ShowUI()
        {
            return View();
        }
    }
}
