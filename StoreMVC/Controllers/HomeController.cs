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
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Name = "Goncharik Yuri";
			ViewBag.Phone = "+375(29) 121-01-41";
			ViewBag.Email = "SeleSt.GYV@gmail.com";

			return View();
		}

		[Authorize] // Запрещены анонимные обращения к данной странице
		public ActionResult Cabinet()
		{
			ViewBag.Message = "Private Page.";

			return View();
		}

		[Authorize(Roles = "Admin")] // К данному методу действия могут получать доступ только пользователи с ролью Admin
		public ActionResult AdminPanel()
		{
			ViewBag.Message = "Admin Panel.";

			return View();
		}

		[Authorize(Roles = "Admin, Moderator")] // К данному методу действия могут получать доступ только пользователи с ролью Admin и Moderator
		public ActionResult ModeratorPanel()
		{
			ViewBag.Message = "Moderator Panel.";

			return View();
		}

	}
}