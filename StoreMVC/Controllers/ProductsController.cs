using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreMVC.Models;
using StoreMVC.Util;


namespace StoreMVC.Controllers
{
	public class ProductsController : Controller
	{
		private DBStoreMVC db = new DBStoreMVC();
		string imagesDirectoryPath = "/Files/Images/";

		// GET: Products
		public ActionResult Index()
		{
			return View(db.Products.ToList());
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
			if (ModelState.IsValid)
			{
				product.imgName = ImageFuctionality.UploadImage(file, Server.MapPath("~"), imagesDirectoryPath);
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
		public ActionResult Edit([Bind(Include = "ProductId,Name,Description,Price,Category,imgName")] Product product, HttpPostedFileBase file)
		{

			if (ModelState.IsValid)
			{
				if (file != null) // ????????????????????????????????????????????????????????????????????????????????????????????????????????????
				{
					product.imgName = ImageFuctionality.UploadImage(file, Server.MapPath("~"), imagesDirectoryPath);
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

			ImageFuctionality.DeleteImageFromServer(product.imgName, Server.MapPath("~"), imagesDirectoryPath);

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

		public ActionResult ProductsDataPartial()
		{
			return PartialView("_ProductsDataPartial", db.Products.ToList());
		}

		[HttpPost]
		public ActionResult ProductsSearch(string name)
		{
			var products = db.Products.Where(a => a.Name.Contains(name)).ToList();
			if (products.Count <= 0)
			{
				return HttpNotFound();
			}
			//return PartialView("_ProductsSearchPartial", products);
			return PartialView("_ProductsDataPartial", products);
		}

		public ActionResult Upload()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Upload(HttpPostedFileBase file)
		{
			ImageFuctionality.UploadImage(file, Server.MapPath("~"), imagesDirectoryPath);
			return View();
		}
	}
}
