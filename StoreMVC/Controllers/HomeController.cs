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
			//ViewBag.Categories = GetCategories();
			ViewBag.imagesDirectoryPath = ImageFuctionality.imagesDirectoryPath;
			ViewBag.folderName = "Categories/";
			ViewBag.imageExtention = ".jpeg";
			return View(ProductsCategories.CategoriesDictionary);
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

		private SelectList GetCategoriesSelectList(int selectedItem = 1)
		{
			var categoriesSelectList = ProductsCategories.CategoriesSelectList;
			categoriesSelectList[0].Text = "";
			return new SelectList(categoriesSelectList, "Value", "Text");
		}

	}
}