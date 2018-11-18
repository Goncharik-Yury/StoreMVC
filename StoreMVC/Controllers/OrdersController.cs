using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreMVC.Models;
using StoreMVC.Filters;
using WebMatrix.WebData;

namespace StoreMVC.Controllers
{
	//[Authorize(Roles = "Admin, Moderator")]
	public class OrdersController : Controller
	{
		private DBStoreMVC db = new DBStoreMVC();

		// GET: Orders
		public ActionResult Index()
		{
			List<Order> orders = GetOrdersAll();
			return View(orders);
		}

		// GET: Orders/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = GetOrderByOrderId(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}

		// GET: Orders/Create
		public ActionResult Create()
		{
			Add_ViewBag_UserId();
			Add_ViewBag_ProductId();
			return View();
		}

		// POST: Orders/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult Create([Bind(Include = "OrderId,UserId,ProductId,Date")] Order order)
		{
			order.Date = DateTime.Now;
			order.UserId = WebSecurity.GetUserId(User.Identity.Name);

			if (ModelState.IsValid)
			{
				db.Orders.Add(order);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			Add_ViewBag_UserId(order.UserId);
			Add_ViewBag_ProductId(order.ProductId);
			return View(order);
		}

		// GET: Orders/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = db.Orders.Find(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			Add_ViewBag_UserId(order.UserId);
			Add_ViewBag_ProductId(order.ProductId);
			return View(order);
		}

		// POST: Orders/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "OrderId,UserId,ProductId,Date")] Order order)
		{
			if (ModelState.IsValid)
			{
				db.Entry(order).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			Add_ViewBag_UserId(order.UserId);
			Add_ViewBag_ProductId(order.ProductId);
			return View(order);
		}

		// GET: Orders/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = GetOrderByOrderId(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}

		// POST: Orders/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Order order = db.Orders.Find(id);
			db.Orders.Remove(order);
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

		public ActionResult UserOrders()
		{
			int userId = WebSecurity.CurrentUserId;
			List<Order> orders = GetOrdersByUserId(userId);
			return View(orders);
		}

		private void Add_ViewBag_UserId(int? userId = null)
		{
			ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", userId);
		}

		private void Add_ViewBag_ProductId(int? productId = null)
		{
			ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", productId);
		}

		private List<Order> GetOrdersAll()
		{
			return db.Orders.Include(o => o.UserProfile).Include(o => o.Product).ToList();
		}
		private Order GetOrderByOrderId(int? id)
		{
			return db.Orders.Include(o => o.UserProfile).Include(o => o.Product).Where(o => o.OrderId == id).First();
		}

		private List<Order> GetOrdersByUserId(int? id)
		{
			return db.Orders.Include(o => o.UserProfile).Include(o => o.Product).Where(o => o.UserId == id).ToList();
		}

	}
}
