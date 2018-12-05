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
using StoreMVC.Util;

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
			List<Order> orders = Utility.GetOrdersAll(db);
			return View(orders);
		}

		// GET: Orders/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = Utility.GetOrderByOrderId(db, id);
			if (!isUserGotPrivilegesOnOrder(order))
			{
				return View("Error");
			}

			return View(order);
		}

		// GET: Orders/Create
		public ActionResult Create()
		{
			ViewBag.UserId = Utility.UsersIdSelectList(db);
			ViewBag.ProductId = Utility.ProductsIdSelectList(db);
			return View();
		}

		// POST: Orders/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.

		// new big method
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ProductId")] Order order)
		{
			if (!WebSecurity.IsAuthenticated)
			{
				return RedirectToAction("Register", "Account");
			}

			Product orderedProduct = Utility.GetProductById(db, order.ProductId);

			if (orderedProduct.Count < 1)
			{
				return View();
			}

			int productsOrderedCount = 1;

			order.UserId = WebSecurity.GetUserId(User.Identity.Name);

			//ViewBag.UserId = Utility.UsersIdSelectList(order.UserId);
			//ViewBag.ProductId = Utility.ProductsIdSelectList(order.ProductId);

			Order orderInDb = Utility.GetOrderByProductIdAndUserId(db, order.ProductId, order.UserId);

			if (orderInDb == null)
			{
				if (ModelState.IsValid)
				{
					order.Date = DateTime.Now;
					order.Count += productsOrderedCount;
					db.Orders.Add(order);
					db.SaveChanges();
				}
			}
			else
			{
				if (orderInDb.Count + productsOrderedCount > orderedProduct.Count)
				{
					AddModalError_ProductCountOnStorage();
				}
				if (ModelState.IsValid)
				{
					orderInDb.Count += productsOrderedCount;
					orderInDb.Date = DateTime.Now;
					db.Entry(orderInDb).State = EntityState.Modified;
					db.SaveChanges();
				}

			}
			return RedirectToAction("OrdersUser");
		}

		// GET: Orders/Edit/5
		//[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = Utility.GetOrderByOrderId(db, id);

			if (!isUserGotPrivilegesOnOrder(order))
			{
				return View("Error");
			}

			return View(order);
		}

		// POST: Orders/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		//[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Edit([Bind(Include = "Count")] int count)
		{
			int orderId = 0;
			if (!Int32.TryParse(RouteData.Values["id"].ToString(), out orderId))
			{
				return View("Error");
			}

			Order order = Utility.GetOrderByOrderId(db, orderId);

			if (count > order.Product.Count)
			{
				AddModalError_ProductCountOnStorage();
			}

			if (!isUserGotPrivilegesOnOrder(order))
			{
				return View("Error");
			}

			if (ModelState.IsValid)
			{
				order.Date = DateTime.Now;
				order.Count = count;
				db.Entry(order).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("OrdersUser");
			}

			return View(order);
		}

		// GET: Orders/Delete/5
		//[Authorize(Roles = "Admin, Moderator")]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = Utility.GetOrderByOrderId(db, id);

			if (!isUserGotPrivilegesOnOrder(order))
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
			return RedirectToAction("OrdersUser");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Buy(int orderId)
		{
			//int orderId = 0;
			//if (!Int32.TryParse(RouteData.Values["id"].ToString(), out orderId))
			//{
			//	return View("Error");
			//}

			Order order = Utility.GetOrderByOrderId(db, orderId);

			if (!isUserGotPrivilegesOnOrder(order))
			{
				return View("Error");
			}

			if (ModelState.IsValid)
			{
				order.Product.Count -= order.Count;

				db.Entry(order.Product).State = EntityState.Modified;
				db.Orders.Remove(Utility.GetOrderByOrderId(db, orderId));
				db.SaveChanges();
				return RedirectToAction("OrdersUser");
			}

			return RedirectToAction("OrdersUser");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		public ActionResult OrdersUser()
		{
			int userId = WebSecurity.CurrentUserId;
			List<Order> ordersToShow = Utility.GetOrdersByUserId(userId, db);
			return View(ordersToShow);
			//return PartialView("_OrdersUserDataPartial", ordersToShow);
		}

		public ActionResult OrdersAll()
		{
			List<Order> ordersToShow = Utility.GetOrdersAll(db);

			return PartialView("_OrdersDataPartial", ordersToShow);
		}

		public ActionResult OrdersSearchById(int? userId)
		{
			if (userId == null || userId <= 0)
			{
				return View("Error");
			}

			return PartialView("_OrdersUserDataPartial", Utility.GetOrdersByUserId(userId, db));
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
				return RedirectToAction("OrdersUser");
		}

		//private Order GetOrderByOrderId(int? orderId)
		//{
		//	return db.Orders.Include(o => o.UserProfile).Include(o => o.Product).Where(o => o.OrderId == orderId).First();
		//}

		private bool isUserGotPrivilegesOnOrder(Order order)
		{
			if (order == null ||
				(order.UserId != WebSecurity.CurrentUserId &&
				(!User.IsInRole("Admin") && !User.IsInRole("Moderator"))))
			{
				return false;
			}
			return true;
		}

		private void AddModalError_ProductCountOnStorage()
		{
			ModelState.AddModelError("Count", "Not enough product on shop storage");
		}
	}
}
