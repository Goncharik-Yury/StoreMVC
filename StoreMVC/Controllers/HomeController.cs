using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreMVC.Controllers
{
	public class HomeController : Controller
	{
		// GET: Home
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Name = "Goncharik Yuri";
			ViewBag.Phone = "+375(29) 121-01-41";
			ViewBag.Email = "SeleSt.GYV@gmail.com";

			return View();
		}

	}
}