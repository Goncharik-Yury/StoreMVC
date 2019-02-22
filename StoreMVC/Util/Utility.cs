using StoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using StoreMVC.Filters;
using WebMatrix.WebData;
using StoreMVC.Util;

namespace StoreMVC.Util
{
	public static class Utility
	{
		
		public static UserProfile GetUserById(DBStoreMVC db, int userId)
		{
			return db.UserProfiles.Find(userId);
		}

		public static List<Product> GetProductsByName(DBStoreMVC db, string productNameToSearch = "", string categoryNameToSearch = "all")
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
		public static Product GetProductById(DBStoreMVC db, int? productIdToSearch)
		{
			if (productIdToSearch == null || productIdToSearch <= 0)
			{
				return new Product();
			}
			/*productsOfCategory.Where(model => model.Name.ToLower().Contains(productIdToSearch.ToLower())).ToList();*/
			return db.Products.Where(model => model.ProductId == productIdToSearch).FirstOrDefault();
		}

		public static Order GetOrderByOrderId(DBStoreMVC db, int? orderId)
		{
			return db.Orders.Include(o => o.UserProfile).Include(o => o.Product).Where(o => o.OrderId == orderId).First();
		}

		public static Order GetOrderByProductIdAndUserId(DBStoreMVC db, int productId, int userId)
		{
			return db.Orders.Where(o =>
				o.ProductId == productId &&
				o.UserId == userId).
				FirstOrDefault();
		}

		public static List<Order> GetOrdersByUserId(int? userId, DBStoreMVC db)
		{
			return db.Orders.Include(o => o.UserProfile).Include(o => o.Product).Where(o => o.UserId == userId).ToList();
		}

		public static List<Order> GetOrdersAll(DBStoreMVC db)
		{
			return db.Orders.Include(o => o.UserProfile).Include(o => o.Product).ToList();
		}

		public static SelectList UsersIdSelectList(DBStoreMVC db, int? userId = null)
		{
			return new SelectList(db.UserProfiles, "UserId", "UserName", userId);
		}

		public static SelectList ProductsIdSelectList(DBStoreMVC db, int? productId = null)
		{
			return new SelectList(db.Products, "ProductId", "Name", productId);
		}

		public static SelectList CategoriesSelectList(int selectedItem = 0)
		{
			List<SelectListItem> categoriesSelectList = ProductsCategories.CategoriesSelectList;
			categoriesSelectList[0].Text = "";
			return new SelectList(categoriesSelectList, "Value", "Text");
		}

		public static SelectList ToSelectList(string[] str)
		{
			List<SelectListItem> selectList = str.Select((name, index) =>
			{
				return new SelectListItem
				{

					Value = name,
					Text = name
				};
			}).ToList();
			return new SelectList(selectList, "Value", "Text");
		}

	}
}