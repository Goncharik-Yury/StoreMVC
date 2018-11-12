using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreMVC.Models;


namespace StoreMVC.Controllers
{
	public class ProductsController : Controller
	{
		private DBStoreMVC db = new DBStoreMVC();

		// GET: Products
		public ActionResult Index(String categoryNameToSearch="all", String productNameToSearch="")
		{
			Add_ViewBag_CategoriesSelectList();
			ViewBag.categoryNameToSearch = categoryNameToSearch;
			ViewBag.productNameToSearch = productNameToSearch;
			return View(/*db.Products.ToList()*/);
		}

		// GET: Products/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}

			return View(product);
		}


		// GET: Products/Create
		//[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Create()
		{
			/*ViewBag.Category = */
			Add_ViewBag_CategoriesSelectList();
			return View();
		}

		// POST: Products/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		//[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Create([Bind(Include = "ProductId,Name,Description,Price,Category")] Product product, HttpPostedFileBase file)
		{
			/*ViewBag.Category = */
			Add_ViewBag_CategoriesSelectList();
			if (product.Category == "all")
			{
				ModelState.AddModelError("Category", "Выберите категорию продукта");
			}
			if (ModelState.IsValid)
			{
				product.imgName = ImageFuctionality.UploadImage(file, Server.MapPath("~"), ImageFuctionality.imagesDirectoryPath);
				db.Products.Add(product);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(product);
		}

		// GET: Products/Edit/5
		//[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Edit(int? id)
		{
			/*ViewBag.Category = */
			Add_ViewBag_CategoriesSelectList();

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}

			return View(product);
		}

		// POST: Products/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		//[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Edit([Bind(Include = "ProductId,Name,Description,Price,Category,imgName")] Product product, HttpPostedFileBase file, string imgName_old)
		{
			Add_ViewBag_CategoriesSelectList();

			if (product.Category == "all")
			{
				ModelState.AddModelError("Category", "Выберите категорию продукта");
			}

			if (ModelState.IsValid)
			{
				if (file != null) // ????????????????????????????????????????????????????????????????????????????????????????????????????????????
				{
					product.imgName = ImageFuctionality.UploadImage(file, Server.MapPath("~"), ImageFuctionality.imagesDirectoryPath);
					ImageFuctionality.DeleteImageFromServer(imgName_old, Server.MapPath("~"), ImageFuctionality.imagesDirectoryPath);
				}

				db.Entry(product).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(product);
		}

		// GET: Products/Delete/5
		//[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}

		//[Authorize(Roles = "Admin, Moderator")]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Product product = db.Products.Find(id);

			ImageFuctionality.DeleteImageFromServer(product.imgName, Server.MapPath("~"), ImageFuctionality.imagesDirectoryPath);

			db.Products.Remove(product);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		public ActionResult ProductsSearch(string categoryNameToSearch, string productNameToSearch)
		{
			//if (productsToShow.Count <= 0)
			//{
			//	return HttpNotFound();
			//}

			List<Product> productsToShow = GetProductsFromDB(categoryNameToSearch, productNameToSearch);
			return PartialView("_ProductsDataPartial", productsToShow);
		}

		private List<Product> GetProductsFromDB(string categoryNameToSearch = "all", string productNameToSearch = "")
		{
			if (String.IsNullOrEmpty(categoryNameToSearch))
				categoryNameToSearch = "all";
			if (String.IsNullOrEmpty(productNameToSearch))
				productNameToSearch = "";

			if (!ProductsCategories.CategoriesDictionary.ContainsKey(categoryNameToSearch))
			{
				return new List<Product>();
			}

			if (categoryNameToSearch == "all")
				categoryNameToSearch = "";

			List<Product> productsOfCategory = db.Products.Where(model => model.Category.Contains(categoryNameToSearch)).ToList();

			if (String.IsNullOrEmpty(productNameToSearch))
			{
				return productsOfCategory;
			}
			else
			{
				List<Product> productsToShow = productsOfCategory.Where(model => model.Name.ToLower().Contains(productNameToSearch.ToLower())).ToList();
				return productsToShow;
			}
		}
		//public ActionResult GetProductsOfCategory(string categoryNameToSearch)
		//{
		//	ViewBag.Categories = GetCategoriesSelectList();
		//	return View("Index", productsOfCategory.ToList());
		//}

		public ActionResult Upload()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Upload(HttpPostedFileBase file)
		{
			ImageFuctionality.UploadImage(file, Server.MapPath("~"), ImageFuctionality.imagesDirectoryPath);
			return View();
		}

		private void Add_ViewBag_CategoriesSelectList(int selectedItem = 1)
		{
			ViewBag.Categories = CategoriesSelectList(selectedItem);
		}
		private SelectList CategoriesSelectList(int selectedItem = 1)
		{
			var categoriesSelectList = ProductsCategories.CategoriesSelectList;
			categoriesSelectList[0].Text = "";
			return new SelectList(categoriesSelectList, "Value", "Text");
		}
	}
}
