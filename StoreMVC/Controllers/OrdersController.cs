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
		[Authorize(Roles = "Admin, Moderator")]
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
			AddViewBag_UsersIdSelectList();
			AddViewBag_ProductsIdSelectList();
			return View();
		}

		// POST: Orders/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "OrderId,ProductId,Count")] Order order)
		{
			order.Date = DateTime.Now;
			order.UserId = WebSecurity.GetUserId(User.Identity.Name);

			AddViewBag_UsersIdSelectList(order.UserId);
			AddViewBag_ProductsIdSelectList(order.ProductId);

			if (ModelState.IsValid)
			{
				Order orderInDb = db.Orders.Where(o =>
					o.ProductId == order.ProductId &&
					o.UserId == order.UserId).
					FirstOrDefault();

				if (orderInDb != null)
				{
					orderInDb.Count++;
					orderInDb.Date = DateTime.Now;
					db.Entry(orderInDb).State = EntityState.Modified;
					db.SaveChanges();
				}
				else
				{
					order.Count++;
					db.Orders.Add(order);
					db.SaveChanges();
				}
				return RedirectToAction("UserOrders");
			}

			return View(order);
		}

		// GET: Orders/Edit/5
		//[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = db.Orders.Find(id);

			if (order == null || order.UserId != WebSecurity.CurrentUserId)
			{
				return HttpNotFound();
			}

			AddViewBag_UsersIdSelectList(order.UserId);
			AddViewBag_ProductsIdSelectList(order.ProductId);

			return View(order);
		}

		// POST: Orders/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		//[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Edit([Bind(Include = "OrderId,UserId,ProductId,Count")] Order order)
		{
			if (order == null || order.UserId != WebSecurity.CurrentUserId)
			{
				return HttpNotFound();
			}

			AddViewBag_UsersIdSelectList(order.UserId);
			AddViewBag_ProductsIdSelectList(order.ProductId);

			if (ModelState.IsValid)
			{
				order.Date = DateTime.Now;
				db.Entry(order).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("OrdersPageChoosing");
			}

			return View(order);
		}

		// GET: Orders/Delete/5
		[Authorize(Roles = "Admin, Moderator")]
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

		public ActionResult OrdersSearchAll()
		{
			List<Order> ordersToShow = GetOrdersAll();

			return PartialView("_OrdersDataPartial", ordersToShow);
		}

		public ActionResult OrdersSearchById()
		{
			//if (productsToShow.Count <= 0)
			//{
			//	return HttpNotFound();
			//}
			int userId = WebSecurity.CurrentUserId;
			List<Order> ordersToShow = new List<Order>();
			ordersToShow = GetOrdersByUserId(userId);

			return PartialView("_OrdersDataPartial", ordersToShow);
		}

		public ActionResult OrdersPageChoosing()
		{
			if (User.IsInRole("Moderator"))
			{
				return RedirectToAction("Index");
			}
			else if (User.IsInRole("Admin"))
			{
				return RedirectToAction("Index");
			}
			else
				return RedirectToAction("UserOrders");
		}

		private void AddViewBag_UsersIdSelectList(int? userId = null)
		{
			ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", userId);
		}

		private void AddViewBag_ProductsIdSelectList(int? productId = null)
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
