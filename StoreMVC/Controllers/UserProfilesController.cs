using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StoreMVC.Models;
using WebMatrix.WebData;

namespace StoreMVC.Controllers
{
	public class UserProfilesController : Controller
	{
		private DBStoreMVC db = new DBStoreMVC();
		SimpleRoleProvider rolesProvider = (SimpleRoleProvider)Roles.Provider;

		// GET: UserProfiles
		public ActionResult Index()
		{
			List<UserProfileFull> UserProfilesFull = new List<UserProfileFull>();
			List<UserProfile> UserProfiles = db.UserProfiles.ToList();


			foreach (UserProfile userProfile in UserProfiles)
			{
				UserProfilesFull.Add(new UserProfileFull(userProfile));
				UserProfilesFull.Last().Roles = rolesProvider.GetRolesForUser(userProfile.UserName);
			}

			return View(UserProfilesFull);
		}

		// GET: UserProfiles/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			UserProfileFull userProfileFull = new UserProfileFull(db.UserProfiles.Find(id));
			if (userProfileFull == null)
			{
				return HttpNotFound();
			}
			userProfileFull.Roles = rolesProvider.GetRolesForUser(userProfileFull.UserName);
			return View(userProfileFull);
		}

		// GET: UserProfiles/Create
		//public ActionResult Create()
		//{
		//    return View();
		//}

		// POST: UserProfiles/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		//     [HttpPost]
		//     [ValidateAntiForgeryToken]
		//     public ActionResult Create([Bind(Include = "UserId,UserName,FirstName,LastName,Patronymic,Email")] UserProfile userProfile)
		//     {
		//isLoginIsAvailable(userProfile.UserName);

		//if (ModelState.IsValid)
		//         {
		//             db.UserProfiles.Add(userProfile);
		//             db.SaveChanges();
		//             return RedirectToAction("Index");
		//         }

		//         return View(userProfile);
		//     }

		// GET: UserProfiles/Edit/5

		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			UserProfileFull userProfileFull = new UserProfileFull(db.UserProfiles.Find(id));
			if (userProfileFull == null)
			{
				return HttpNotFound();
			}
			//, new[] { "" }
			string[] roles = rolesProvider.GetRolesForUser(userProfileFull.UserName);
			if (roles.Length <= 0)
			{
				userProfileFull.Roles = new[] { "" };
			}
			else
			{
				userProfileFull.Roles = roles;
			}

			return View(userProfileFull);
		}

		// POST: UserProfiles/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "UserId,UserName,FirstName,LastName,Patronymic,Email")] UserProfile userProfile, string[] Roles)
		{
			isLoginIsAvailable(userProfile.UserName);

			if (ModelState.IsValid)
			{
				db.Entry(userProfile).State = EntityState.Modified;

				RemoveUserFromRoles(userProfile.UserName);
				AddUserToRoles(userProfile.UserName, Roles);

				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(userProfile);
		}

		// GET: UserProfiles/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			UserProfileFull userProfileFull = new UserProfileFull(db.UserProfiles.Find(id));
			if (userProfileFull == null)
			{
				return HttpNotFound();
			}
			userProfileFull.Roles = rolesProvider.GetRolesForUser(userProfileFull.UserName);

			return View(userProfileFull);
		}

		// POST: UserProfiles/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			UserProfile userProfile = db.UserProfiles.Find(id);
			db.UserProfiles.Remove(userProfile);
			RemoveUserFromRoles(userProfile.UserName);
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

		private bool isLoginIsAvailable(string name)
		{
			if (db.Customers.FirstOrDefault(n => n.Name == name) != null)
			{
				ModelState.AddModelError("UserName", "This login is already taken. Please choose another.");
				return false;
			}
			return true;
		}

		private void RemoveUserFromRoles(string userName)
		{
			string[] roles = rolesProvider.GetRolesForUser(userName);
			rolesProvider.RemoveUsersFromRoles(new[] { userName }, roles);
		}
		private void RemoveUsersFromRoles(string[] userNames)
		{
			foreach (string userName in userNames)
			{
				string[] roles = rolesProvider.GetRolesForUser(userName);
				rolesProvider.RemoveUsersFromRoles(new[] { userName }, roles);
			}
		}

		private void AddUserToRoles(string userName, string[] roles)
		{
			foreach (string role in roles)
			{
				if (!string.IsNullOrEmpty(role))
				{
					rolesProvider.AddUsersToRoles(new[] { userName }, new[] { role });
				}
			}
		}
		private void AddUsersToRoles(string[] userNames, string[] roles)
		{
			foreach (string userName in userNames)
			{
				foreach (string role in roles)
				{
					if (!string.IsNullOrEmpty(role))
					{
						rolesProvider.AddUsersToRoles(new[] { userName }, new[] { role });
					}
				}
			}
		}
	}
}
