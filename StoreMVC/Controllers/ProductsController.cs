using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreMVC.Models;
using System.IO;
using System.Drawing;
using System.Diagnostics;

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
		[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Create()
		{
			return View();
		}

		// POST: Products/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Create([Bind(Include = "ProductId,Name,Description,Price,Category")] Product product, HttpPostedFileBase file)
		{
			if (ModelState.IsValid)
			{
				product.imgName = UploadImage(file);
				db.Products.Add(product);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(product);
		}

		// GET: Products/Edit/5
		[Authorize(Roles = "Admin, Moderator")]
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
		[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Edit([Bind(Include = "ProductId,Name,Description,Price,Category")] Product product, HttpPostedFileBase file)
		{
			if (ModelState.IsValid)
			{
				product.imgName = UploadImage(file);
				db.Entry(product).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			
			return View(product);
		}

		// GET: Products/Delete/5
		[Authorize(Roles = "Admin, Moderator")]
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

		[Authorize(Roles = "Admin, Moderator")]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Product product = db.Products.Find(id);

			DeleteImageFromServer(product.imgName);

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

		// Uploading files on server
		public string UploadImage(HttpPostedFileBase file)
		{
			string filePath = Server.MapPath("~" + imagesDirectoryPath);
			string fileName = "default.png";

			if (file != null)
			{
				Image image = null;
				try
				{
					image = Image.FromStream(file.InputStream);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("File to Image convertation exception" + ex);
					return filePath + fileName;
				}

				fileName = Path.GetFileName(file.FileName);

				do
					fileName = String.Format(@"{0}{1}", Guid.NewGuid(), Path.GetExtension(file.FileName));
				while (System.IO.File.Exists(filePath + fileName));

				int requiredHeight = 500;
				int requiredWidth = 500;

				image = ImageScale(image, requiredHeight, requiredWidth);
				image.Save(filePath + fileName);
			}
			return fileName;
		}

		// Scaling the Image
		public static Image ImageScale(Image originalBitmap, int requiredHeight, int requiredWidth)
		{
			int[] heightWidthRequiredDimensions;

			// Pass dimensions to worker method depending on image type required
			heightWidthRequiredDimensions = ImageRightSize(originalBitmap.Height, originalBitmap.Width, requiredHeight, requiredWidth);


			Bitmap resizedBitmap = new Bitmap(heightWidthRequiredDimensions[1],
											   heightWidthRequiredDimensions[0]);

			const float resolution = 72;

			resizedBitmap.SetResolution(resolution, resolution);

			Graphics graphic = Graphics.FromImage((Image)resizedBitmap);

			graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			graphic.DrawImage(originalBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

			graphic.Dispose();
			originalBitmap.Dispose();
			//resizedBitmap.Dispose(); // Still in use


			return resizedBitmap;
		}

		// Finding correct image size
		private static int[] ImageRightSize(int originalHeight, int originalWidth, int requiredHeight, int requiredWidth)
		{
			int imgHeight = 0;
			int imgWidth = 0;

			imgWidth = requiredHeight;
			imgHeight = requiredWidth;


			int requiredHeightLocal = originalHeight;
			int requiredWidthLocal = originalWidth;

			double ratio = 0;

			// Check height first
			// If original height exceeds maximum, get new height and work ratio.
			if (originalHeight > imgHeight)
			{
				ratio = double.Parse(((double)imgHeight / (double)originalHeight).ToString());
				requiredHeightLocal = imgHeight;
				requiredWidthLocal = (int)((decimal)originalWidth * (decimal)ratio);
			}

			// Check width second. It will most likely have been sized down enough
			// in the previous if statement. If not, change both dimensions here by width.
			// If new width exceeds maximum, get new width and height ratio.
			if (requiredWidthLocal >= imgWidth)
			{
				ratio = double.Parse(((double)imgWidth / (double)originalWidth).ToString());
				requiredWidthLocal = imgWidth;
				requiredHeightLocal = (int)((double)originalHeight * (double)ratio);
			}

			int[] heightWidthDimensionArr = { requiredHeightLocal, requiredWidthLocal };

			return heightWidthDimensionArr;
		}

		private bool DeleteImageFromServer(string imgName)
		{
			if (imgName != "default.png")
			{
				try
				{
					System.IO.File.Delete(Server.MapPath("~" + imagesDirectoryPath + imgName));
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Unable to delete the Image from server." + ex);
					return false;
				}
			}
			return true;
		}

		public ActionResult Upload()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Upload(HttpPostedFileBase file)
		{
			UploadImage(file);
			return View();
		}
	}
}
