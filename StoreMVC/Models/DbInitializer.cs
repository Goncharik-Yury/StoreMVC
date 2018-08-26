using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StoreMVC.Models
{
	public class DBInitializer : DropCreateDatabaseAlways<DBStoreMVC>
	{
		protected override void Seed(DBStoreMVC context)
		{
			ProductsInitializer(context);
			base.Seed(context);
		}

		protected void ProductsInitializer(DBStoreMVC context)
		{
			context.Products.Add(new Product() { ProductId = 0, Name = "Product1", Description = "", Price = 100, Category = "cat1" });
			context.Products.Add(new Product() { ProductId = 1, Name = "Product2", Description = "", Price = 200, Category = "cat1" });
			context.Products.Add(new Product() { ProductId = 2, Name = "Product3", Description = "", Price = 300, Category = "cat2" });
			context.Products.Add(new Product() { ProductId = 3, Name = "Product4", Description = "", Price = 400, Category = "cat2" });
			context.Products.Add(new Product() { ProductId = 4, Name = "Product5", Description = "", Price = 400, Category = "cat1" });
		}
	}
}